using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //List of all Connectors attached to the prefab
    public List<Connector> exits;
    //Declares if the room can spawn more than once
    public bool isUnique;
    //Is this a hallway or a room
    public bool isPath;
    //Is this piece a potential ending point
    public bool winRoom;
    //Is this piece a potential start point
    public bool startRoom;
    //Needed for room boundaries
    public Mesh roomMesh;

    public void UsedExit(Connector.ConnectDirection exitDirection)
    {
        for(int i = exits.Count - 1; i >= 0; --i)
        {
            if(exits[i].direction == exitDirection)
            {
                exits.RemoveAt(i);
            }
        }
    }

    public void RotateRoomClockwise(int rotations)
    {
        if (rotations == 0 || exits == null) return;

        transform.Rotate(new Vector3(0, rotations * 90, 0));
        
        for(int c = 0; c < exits.Count; ++c)
        {
            exits[c].direction = (Connector.ConnectDirection)(((int)exits[c].direction + rotations) % 4);
        }
    }

    public void RemoveRoom()
    {
        Destroy(gameObject);
    }
}
