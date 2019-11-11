﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ma_LevelManager : MonoBehaviour
{

    public static Ma_LevelManager Instance;
    public GameObject WallPrefab;
    public GameObject FreePrefab;
    public GameObject PlayerPrefab;
    public GameObject IAPrefab;

    public Ma_ClockManager clock;
    //public NavMeshSurface navMeshSurface;

    [SerializeField]public Tile[] allWalkableTile;

    public void Awake()
    {
        Instance = this;
        clock = GetComponentInChildren<Ma_ClockManager>();
    }

    public void InitLevel()
    {/*
        //TO CHANGE
        string[,] array = new string[,]{    { "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W"},
                                            { "W", "F", "H", "F", "F", "W", "F", "F", "F", "W", "F", "F", "F", "F", "F", "F", "F", "F", "F", "W"},
                                            { "W", "F", "F", "F", "F", "W", "F", "W", "F", "W", "F", "W", "W", "F", "W", "F", "P", "P", "F", "W"},
                                            { "W", "H", "F", "H", "F", "W", "W", "W", "F", "W", "F", "W", "F", "F", "W", "F", "W", "W", "F", "W"},
                                            { "W", "W", "F", "W", "W", "W", "W", "F", "F", "F", "F", "W", "W", "W", "W", "W", "F", "F", "F", "W"},
                                            { "W", "F", "F", "F", "W", "F", "W", "W", "F", "W", "F", "F", "F", "W", "F", "W", "W", "W", "F", "W"},
                                            { "W", "F", "W", "F", "W", "F", "F", "W", "F", "F", "F", "W", "F", "F", "F", "F", "W", "F", "F", "W"},
                                            { "W", "F", "W", "W", "W", "F", "W", "W", "F", "W", "W", "W", "W", "W", "W", "F", "F", "F", "W", "W"},
                                            { "W", "F", "F", "F", "F", "F", "W", "F", "F", "F", "F", "F", "F", "F", "W", "W", "W", "F", "F", "W"},
                                            { "W", "W", "W", "F", "W", "W", "W", "F", "W", "W", "W", "W", "W", "F", "F", "F", "W", "W", "F", "W"},
                                            { "W", "F", "F", "F", "W", "F", "F", "F", "F", "F", "F", "F", "W", "F", "W", "W", "W", "F", "F", "W"},
                                            { "W", "W", "F", "W", "W", "F", "W", "W", "F", "W", "F", "W", "W", "F", "W", "F", "W", "W", "F", "W"},
                                            { "W", "F", "F", "F", "F", "F", "F", "W", "W", "W", "F", "W", "F", "F", "F", "F", "F", "F", "F", "W"},
                                            { "W", "F", "W", "W", "W", "W", "F", "F", "F", "W", "F", "W", "F", "W", "W", "W", "W", "W", "F", "W"},
                                            { "W", "F", "F", "F", "F", "W", "W", "W", "F", "F", "F", "W", "F", "W", "F", "F", "F", "W", "F", "W"},
                                            { "W", "F", "W", "W", "W", "W", "F", "F", "F", "W", "F", "F", "F", "W", "F", "W", "F", "W", "F", "W"},
                                            { "W", "F", "F", "F", "W", "W", "F", "W", "W", "W", "F", "W", "W", "W", "F", "W", "F", "F", "F", "W"},
                                            { "W", "F", "F", "F", "W", "F", "F", "F", "F", "W", "F", "F", "F", "F", "F", "W", "W", "W", "W", "W"},
                                            { "W", "F", "F", "F", "F", "F", "W", "W", "F", "F", "F", "W", "F", "W", "F", "F", "F", "F", "F", "W"},
                                            { "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W", "W"}
                                            };
        grid = gameObject.AddComponent<Grid>();
        grid.BuildGridLevel(array);
        //navMeshSurface.BuildNavMesh();*/
    }

    public Tile GetTile(int row, int column)
    {
        Tile res = null;
        for(int i = 0; i < allWalkableTile.Length; i++)
        {
            if(allWalkableTile[i].column == column)
            {
                if(allWalkableTile[i].row == row)
                {
                    res = allWalkableTile[i];
                }
            }
        }
        return res;
    }
}
