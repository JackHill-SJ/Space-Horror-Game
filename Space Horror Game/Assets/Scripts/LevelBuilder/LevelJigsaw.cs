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

        int maxCount = 100, totalTries = 0, p = 1;

        //Start Creating rooms
        for(int i = 0; i < maxRooms && totalTries < maxCount; ++i)
        {
            totalTries++;
            GameObject nextRoom;

            //If there is only 1 more path, we need to build a new pathway.
            if (exits.Count < 2 && i < minRooms)
            {
                nextRoom = Instantiate(pathPool[prng.Next(pathPool.Count)]);
                nextRoom.name = "Pathway " + p.ToString();
                --i;
                ++p;
            }
            else if (exits.Count == 0) break;
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

            exits.RemoveAt(baseDir);

            for(int a = 0; a < getData.exits.Count; ++a)
            {
                if(enterDir != a)
                {
                    exits.Add(getData.exits[a]);
                }
            }
            roomsBuilt.Add(getData);
        }
    }
}
