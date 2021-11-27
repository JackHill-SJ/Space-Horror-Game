using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class LevelJigsaw : MonoBehaviour
{
    //Static reference to the Level Builder for other classes/functions to call
    public static LevelJigsaw Builder { get; private set; }

    //For new Nav Mesh Builder for prefabs
    [SerializeField]
    List<NavMeshSurface> navMeshSurfaces;

    //A list of all possible rooms/pathways to choose from
    public List<GameObject> roomPool;
    public List<GameObject> pathPool;
    public List<GameObject> doors;

    //May make a dictionary later, this is the rooms already built
    [SerializeField] List<Room> roomsBuilt;
    [SerializeField] List<GameObject> doorsBuilt;
    [SerializeField] List<Connector> exits;

    //Total Rooms possible Max
    public int minRooms;
    public int maxRooms;
    public int requiredUniqueRooms;
    public bool addDoors;
    [Range(0f, 1f)]
    public float extraHallChance = 0.45f;
    public bool rebuildNavData;

    [Header("Ran Gen: Debug Settings")]
    public int useSeed;
    //Placed here for now, will remove once working fully
    public List<RoomBounds> rooms; 
    public bool randomSeed;
    System.Random prng;


    void Awake()
    {
        if(Builder != null && Builder != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Builder = this;
        }
    }

    void Start()
    {
        NewLevel();
    }

    void Update()
    {
        if(rebuildNavData)
        {
            RebuildNavMesh();
            rebuildNavData = false;
        }
    }

    public void NewLevel()
    {
        if(randomSeed)
        {
            useSeed = Random.Range(int.MinValue,int.MaxValue);
        }

        NewLevel(useSeed);
    }

    /// <summary>
    ///  This is used when a door opens or closes, so that the "enemy" can move between rooms properly.
    /// </summary>
    public void RebuildNavMesh()
    {
        for (int i = 0; i < navMeshSurfaces.Count; ++i)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

    public void NewLevel(int seed)
    {
        // Start the random Seed Generator
        prng = new System.Random(seed);
        rooms = new List<RoomBounds>();
        navMeshSurfaces = new List<NavMeshSurface>();

        // Check for unique rooms and starting room(s), adding them to a list
        List<GameObject> startPool = new List<GameObject>();
        List<GameObject> uniquePool = new List<GameObject>();
        List<GameObject> genericPool = new List<GameObject>();
        int uniqueRoomCount = 0;

        foreach (GameObject roomList in roomPool)
        {
            Room roomCheck = roomList.GetComponent<Room>();
            //In case a room forgot to add the Room component
            if (roomCheck != null)
            {
                if (roomCheck.startRoom)
                {
                    // Pool of start rooms. A random one will be chosen if multiple start rooms are in the pool
                    startPool.Add(roomList); 
                }
                else if (roomCheck.isUnique)
                {
                    // Unique rooms, such as puzzle rooms or orb locations
                    uniquePool.Add(roomList);
                }
                else
                {
                    // All other rooms
                    genericPool.Add(roomList);
                }
            }
        }

        // If we don't have a list, make one. If we do, destroy the rooms and clear the list
        if (roomsBuilt == null)
        {
            roomsBuilt = new List<Room>();
        }
        else
        {
            foreach(Room removeRoom in roomsBuilt)
            {
                removeRoom.RemoveRoom();
            }
            roomsBuilt.Clear();
        }

        // Clear the doors if this is a new level
        if(doorsBuilt == null)
        {
            doorsBuilt = new List<GameObject>();
        }
        else
        {
            foreach (GameObject removeDoor in doorsBuilt)
            {
                Destroy(removeDoor);
            }

            roomsBuilt.Clear();
        }

        //Add the initial room
        GameObject useRoom;

        // If there is a start room pool, choose one of the start rooms first
        if(startPool.Count > 0)
        {
            useRoom = startPool[prng.Next(startPool.Count)];
        }
        // If not, and we have a required number of unique rooms, use a unique room
        else if (uniquePool.Count > 0 && requiredUniqueRooms > 0)
        {
            useRoom = uniquePool[prng.Next(uniquePool.Count)];
            // Remove the unique room from the pool
            uniquePool.Remove(useRoom);
            uniqueRoomCount++;
        }
        // Otherwise, just choose a random room.
        else
        {
            useRoom = genericPool[prng.Next(genericPool.Count)];
        }
        // Add the room
        GameObject newRoom = Instantiate(useRoom);
        newRoom.name = "Start Room";
        newRoom.transform.SetParent(transform);
        exits = new List<Connector>();
        foreach(Connector newRoomExit in newRoom.GetComponent<Room>().exits)
        {
            exits.Add(newRoomExit);
        }

        // Add room nav mesh surface
        navMeshSurfaces.Add(newRoom.GetComponent<NavMeshSurface>());

        roomsBuilt.Add(newRoom.GetComponent<Room>());
        // Vector2.zero since this is the first room
        rooms.Add(new RoomBounds(roomsBuilt[0].roomMesh, Vector2.zero));

        int maxCount = 100, totalTries = 0, p = 1;        

        //Start Creating rooms
        for(int i = 0; i < maxRooms && totalTries < maxCount; ++i)
        {
            totalTries++;
            GameObject nextRoom;
            bool placingRoom = true; //it is either a room or a path

            if (exits.Count == 0) break; //Can't add rooms if there is nowhere to add them

            // If there is only 1 more path, we need to build a new pathway.
            // Also, 45 percent chance to build a hall instead of a room
            if ((exits.Count < 2 && i < minRooms) || prng.Next(101) / 100f < extraHallChance)
            {
                nextRoom = Instantiate(pathPool[prng.Next(pathPool.Count)]);
                nextRoom.name = "Pathway " + p.ToString();
                --i;
                ++p;
                placingRoom = false;
            }            
            else
            {
                GameObject roomChoice;

                if (uniquePool.Count > 0 && requiredUniqueRooms > uniqueRoomCount)
                {
                    roomChoice = uniquePool[prng.Next(uniquePool.Count)];
                    // Remove the unique room from the pool
                    uniquePool.Remove(roomChoice);
                    uniqueRoomCount++;
                }
                else
                {
                    roomChoice = genericPool[prng.Next(genericPool.Count)];
                }

                nextRoom = Instantiate(roomChoice);
                nextRoom.name = "Room " + i.ToString();
            }            

            nextRoom.transform.SetParent(transform);
            
            Room getRoomData = nextRoom.GetComponent<Room>();
            
            int baseDir = prng.Next(exits.Count);
            int enterDir = prng.Next(getRoomData.exits.Count);
            int rot = 0, cur = (int)getRoomData.exits[enterDir].direction;

            Vector3 placePoint = exits[baseDir].transform.position;

            while(((int)exits[baseDir].direction + 2) % 4 != cur)
            {
                rot++;
                cur = (cur + 1) % 4;
            }

            getRoomData.RotateRoomClockwise(rot);

            placePoint -= getRoomData.exits[enterDir].transform.position;

            nextRoom.transform.position = placePoint;
            RoomBounds roomBounds = new RoomBounds(getRoomData.roomMesh, placePoint);

            bool canPlace = true;

            //Check the pre-existing room boundaries
            foreach(RoomBounds testRoom in rooms)
            {
                if(testRoom.WithinBounds(roomBounds))
                {
                    canPlace = false;
                    break; //Don't need to keep going if this happens
                }
            }

            if ((canPlace && addDoors) ||(!canPlace))
            {
                if (addDoors)
                {
                    GameObject newDoor = Instantiate(doors[0],
                        exits[baseDir].transform.position,
                        ((int)exits[baseDir].direction % 2 == 0) ? Quaternion.identity : Quaternion.Euler(0f, 90f, 0f),
                        transform);
                    doorsBuilt.Add(newDoor);
                }
            }

            //We still want to remove this exit point, as it will keep using it even if
            //we can't use it, and may cause an infinite loop
            exits.RemoveAt(baseDir);

            if (canPlace)
            {
                for (int a = 0; a < getRoomData.exits.Count; ++a)
                {
                    if (enterDir != a)
                    {
                        exits.Add(getRoomData.exits[a]);
                    }
                }
                roomsBuilt.Add(getRoomData);
                rooms.Add(roomBounds);

                //Add room nav mesh surface
                navMeshSurfaces.Add(getRoomData.GetComponent<NavMeshSurface>());
            }
            else
            {
                Destroy(nextRoom);
                if (placingRoom) --i; //If a room didn't place, then remove it from the total rooms count
                
            }
        }

        //If we reached the max rooms but still have opennings
        while (exits.Count > 0)
        {
            GameObject newDoor = Instantiate(doors[0],
                        exits[0].transform.position,
                        ((int)exits[0].direction % 2 == 0) ? Quaternion.identity : Quaternion.Euler(0f, 90f, 0f),
                        transform);
            doorsBuilt.Add(newDoor);
            exits.RemoveAt(0);
        }

        //Once rooms are completed, build the nav mesh components
        RebuildNavMesh();
    }
}
