using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJigsaw : MonoBehaviour
{
    //Static reference to the Level Builder for other classes/functions to call
    public static LevelJigsaw Builder { get; private set; }

    //A list of all possible rooms/pathways to choose from
    public List<GameObject> roomPool;
    public List<GameObject> pathPool;

    //May make a dictionary later, this is the rooms already built
    [SerializeField] List<Room> roomsBuilt;
    [SerializeField] List<Connector> exits;

    //Total Rooms possible Max
    public int minRooms;
    public int maxRooms;

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

    public void NewLevel()
    {
        if(randomSeed)
        {
            useSeed = Random.Range(int.MinValue,int.MaxValue);
        }

        NewLevel(useSeed);
    }

    public void NewLevel(int seed)
    {
        //Start the random Seed Generator
        prng = new System.Random(seed);
        rooms = new List<RoomBounds>();

        //If we don't have a list, make one. If we do, destroy the rooms and clear the list
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

        //Add the initial room
        GameObject newRoom = Instantiate(roomPool[prng.Next(roomPool.Count)]);
        newRoom.name = "Start Room";
        newRoom.transform.SetParent(transform);
        exits = new List<Connector>();
        foreach(Connector newRoomExit in newRoom.GetComponent<Room>().exits)
        {
            exits.Add(newRoomExit);
        }

        roomsBuilt.Add(newRoom.GetComponent<Room>());
        //Vector2.zero since this is the first room
        rooms.Add(new RoomBounds(roomsBuilt[0].roomMesh, Vector2.zero));

        int maxCount = 100, totalTries = 0, p = 1;

        //Start Creating rooms
        for(int i = 0; i < maxRooms && totalTries < maxCount; ++i)
        {
            totalTries++;
            GameObject nextRoom;
            bool placingRoom = true; //it is either a room or a path

            if (exits.Count == 0) break; //Can't add rooms if there is nowhere to add them

            //If there is only 1 more path, we need to build a new pathway.
            if (exits.Count < 2 && i < minRooms)
            {
                nextRoom = Instantiate(pathPool[prng.Next(pathPool.Count)]);
                nextRoom.name = "Pathway " + p.ToString();
                --i;
                ++p;
                placingRoom = false;
            }
            
            else
            {
                nextRoom = Instantiate(roomPool[prng.Next(roomPool.Count)]);
                nextRoom.name = "Room " + i.ToString();
            }            

            nextRoom.transform.SetParent(transform);
            
            Room getData = nextRoom.GetComponent<Room>();
            
            int baseDir = prng.Next(exits.Count);
            int enterDir = prng.Next(getData.exits.Count);
            int rot = 0, cur = (int)getData.exits[enterDir].direction;

            Vector3 placePoint = exits[baseDir].transform.position;

            while(((int)exits[baseDir].direction + 2) % 4 != cur)
            {
                rot++;
                cur = (cur + 1) % 4;
            }

            getData.RotateRoomClockwise(rot);

            placePoint -= getData.exits[enterDir].transform.position;

            nextRoom.transform.position = placePoint;
            RoomBounds roomBounds = new RoomBounds(getData.roomMesh, placePoint);

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

            //We still want to remove this exit point, as it will keep using it even if
            //we can't use it, and may cause an infinite loop
            exits.RemoveAt(baseDir);

            if (canPlace)
            {
                for (int a = 0; a < getData.exits.Count; ++a)
                {
                    if (enterDir != a)
                    {
                        exits.Add(getData.exits[a]);
                    }
                }
                roomsBuilt.Add(getData);
                rooms.Add(roomBounds);
            }
            else
            {
                Destroy(nextRoom);
                if (placingRoom) --i; //If a room didn't place, then remove it from the total rooms count
                
            }
        }
    }
}
