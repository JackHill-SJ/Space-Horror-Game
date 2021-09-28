using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For testing, will remove later as this is not needed. But System.Serializable makes the
//class viewable in the inspector for debug/testing
[System.Serializable]
public class RoomBounds
{
    private static float RoomBufferZone = 0.2f;

    public float topPoint;
    public float bottomPoint;
    public float leftPoint;
    public float rightPoint;

    /// <summary>
    /// Sets the bounds of a room based on its edges
    /// </summary>
    public RoomBounds(Vector2 a, Vector2 b)
    {
        topPoint = a.y > b.y ? a.y : b.y;
        bottomPoint = a.y > b.y ? b.y : a.y;

        rightPoint = a.x > b.x ? a.x : b.x;
        leftPoint = a.x > b.x ? b.x : a.x;
    }

    /// <summary>
    /// Sets the bounds of a room based on its edges
    /// </summary>
    public RoomBounds(Vector3 a, Vector3 b)
    {
        topPoint = a.z > b.z ? a.z : b.z;
        bottomPoint = a.z > b.z ? b.z : a.z;

        rightPoint = a.x > b.x ? a.x : b.x;
        leftPoint = a.x > b.x ? b.x : a.x;
    }


    /// <summary>
    /// Sets the bounds of a room based on its edges
    /// </summary>
    /// <param name="model">The mesh of the 3d model used for the room</param>
    /// <param name="anchorPoint"> the "start point" of the room based on its connector</param>
    public RoomBounds(Mesh model, Vector3 anchorPoint)
    {
        //First, set the values to a max in order to ensure the values
        //populate properly.
        topPoint = float.MinValue;
        bottomPoint = float.MaxValue;
        rightPoint = float.MinValue;
        leftPoint = float.MaxValue;

        //Loop through all Vertices in a model
        for(int vert = 0; vert < model.vertices.Length; ++vert)
        {
            //Anchor point serves as an offset, and needs to be added.
            //Since math calculations can be slightly slower, do this here.
            //Also, looks cleaner
            float x = anchorPoint.x + model.vertices[vert].x;
            float y = anchorPoint.z + model.vertices[vert].z;

            if (y > topPoint) topPoint = y;
            if (y < bottomPoint) bottomPoint = y;
            if (x > rightPoint) rightPoint = x;
            if (x < leftPoint) leftPoint = x;
        }
    }

    public bool WithinBounds(RoomBounds other)
    {
        //Check all 4 points in both rooms and center
        return
            PointWithinBounds(other.UpperLeft) ||
            PointWithinBounds(other.LowerLeft) ||
            PointWithinBounds(other.UpperRight) ||
            PointWithinBounds(other.LowerRight) ||
            PointWithinBounds(other.Center) ||
            other.PointWithinBounds(UpperLeft) ||
            other.PointWithinBounds(LowerLeft) ||
            other.PointWithinBounds(UpperRight) ||
            other.PointWithinBounds(LowerRight) ||
            other.PointWithinBounds(Center);
    }

    private bool PointWithinBounds(Vector2 point)
    {
        return
            leftPoint + RoomBufferZone < point.x && rightPoint - RoomBufferZone > point.x && //Not using <= and >= to ensure rooms can touch
            bottomPoint + RoomBufferZone < point.y && topPoint - RoomBufferZone > point.y;   //for proper connections
    }

    //Room Corners
    public Vector2 UpperLeft
    {
        get { return new Vector2(leftPoint, topPoint); }
    }
    public Vector2 LowerLeft
    {
        get { return new Vector2(leftPoint, bottomPoint); }
    }

    public Vector2 UpperRight
    {
        get { return new Vector2(rightPoint, topPoint); }
    }
    public Vector2 LowerRight
    {
        get { return new Vector2(rightPoint, bottomPoint); }
    }

    public Vector2 Center
    {
        get
        {
            float x = (rightPoint - leftPoint) / 2f + leftPoint;
            float y = (topPoint - bottomPoint) / 2f + bottomPoint;

            return new Vector2(x, y);
        }
    }
}
