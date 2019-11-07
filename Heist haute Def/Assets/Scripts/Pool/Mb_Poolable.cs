using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Poolable : MonoBehaviour
{
    public bool poolable=true;
    public ObjectType objectType;
}

public enum ObjectType
{
    Chest, Door, Enclosure, Tile, Wall
}
