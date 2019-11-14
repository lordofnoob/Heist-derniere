﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class En_WallConstuctor : MonoBehaviour
{
  
}

[System.Serializable]
public enum CombinableWallType
{
   [HideInInspector] None = (0 << 0),
   [HideInInspector] Left = (1 << 0),
   [HideInInspector] Right = (1 << 1),
   [HideInInspector] Up = (1 << 2),
   [HideInInspector] Down = (1 << 3),

    LeftUp = Left | Up,
    RightUp = Right | Up,
    UpDown = Up | Down,
    LeftDown = Left | Down,
    RightDown = Right | Down,
    RightLeft = Right | Left,


    LeftDownRight = Left | Down | Right,
    DownRightUp = Down | Right | Up,
    RightUpLeft = Right | Up | Left,
    UpLeftDown = Up | Left | Down,

    All = Left | Down | Up | Right
}

