using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public ConnectDirection direction;


    public enum ConnectDirection
    {
        North,
        East,
        South,
        West
    }
}
