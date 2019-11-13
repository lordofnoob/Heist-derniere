using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_WallConfiguration : ScriptableObject
{
    public WallStruct wallConfiguration;
}

public enum WallType
{
    ISimpleSide, IBothSide, LSimpleSideInt, LsimpleSideExt, LBothSide, TSimpleSideLarge, TSimpleSideRight, TSimpleSideLeft, TEverySide, XSimpleSide, XBothSidePoint, XEveryside 
}
public struct WallStruct
{
    public WallType type;
    public Tile[] associatedTiles;
}