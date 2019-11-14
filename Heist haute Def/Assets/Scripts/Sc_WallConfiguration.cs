using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "WallConfig")]
public class Sc_WallConfiguration : ScriptableObject
{
    public WallStruct[] wallConfiguration;
}


[System.Serializable]
public struct WallStruct
{
    public CombinableWallType walltype;
    public TileGeneration[] associatedTiles;
}

[System.Serializable]
public struct TileGeneration
{
    public Tile associatedTile;
    public int weight;
}