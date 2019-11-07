﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Ed_Mb_Generator))]
public class In_RoomCreator : Editor
{
    Ed_Mb_Generator mySelectedScript;


    private void OnEnable()
    {
        mySelectedScript = target as Ed_Mb_Generator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("GenerateRoom", GUILayout.MinHeight(50)))
        {
            if (mySelectedScript.roomParent.childCount == 0)
                mySelectedScript.InstantiateRoom();
            else
                Debug.Log("AlreadyProduced");
           
        }
        if (GUILayout.Button("CleanRoom", GUILayout.MinHeight(50)))
        {
            while (mySelectedScript.roomParent.childCount>0)
            {
                DestroyImmediate(mySelectedScript.roomParent.GetChild(0).gameObject);
            }
        }
        if (GUILayout.Button("GenerateGrid", GUILayout.MinHeight(50)))
        {
            List<Tile> listOfTile =  new List<Tile>();
            for (int j = 0; j < mySelectedScript.transform.childCount; j++)
            {
                for (int i = 0; i < mySelectedScript.transform.GetChild(j).childCount; i++)
                { 
                if (mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>() == true)
                    {
                       // Debug.Log(i);
                        listOfTile.Add(mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>());
                    }
                }
            }
           
              
            Tile firstTile = listOfTile[0];
            
            for (int i = 0; i< listOfTile.Count; i++)
            {
                
                Tile worstXtile = listOfTile[0];
                Tile worstYtile = listOfTile[0];
                Tile bestXtile = listOfTile[0];
                Tile bestYtile = listOfTile[0];
  
                if (i >0)
                {
                    if (listOfTile[i].transform.position.x < worstXtile.transform.position.x)
                    {
                        worstXtile = listOfTile[i];
                    }
                    else if (listOfTile[i].transform.position.x > bestXtile.transform.position.x)
                    {
                        bestXtile = listOfTile[i];
                    }
                    else if (listOfTile[i].transform.position.y < worstYtile.transform.position.y)
                    {
                        worstYtile = listOfTile[i];
                    }
                    else if (listOfTile[i].transform.position.y > bestYtile.transform.position.y)
                    {
                        bestYtile = listOfTile[i];
                    }
                }
                float bestX = Mathf.Abs(bestXtile.transform.position.x + bestXtile.transform.position.z);
                float worstX = Mathf.Abs(worstXtile.transform.position.x + worstXtile.transform.position.z);
                float bestY = Mathf.Abs(bestYtile.transform.position.x + bestYtile.transform.position.z);
                float worstY = Mathf.Abs(worstYtile.transform.position.x + worstYtile.transform.position.z);

                if (bestX > worstX || bestX > bestY || bestX > worstY)
                {
                    firstTile = bestXtile;
                }
                else if (worstX > bestX || worstX > bestY || worstX > worstY)
                {
                    firstTile = worstXtile;
                }
                else if (bestY > worstX || bestY > bestX || bestY > worstY)
                {
                    firstTile = bestYtile;
                }
                else if (worstY > bestX || worstY > bestY || worstY > worstX)
                {
                    firstTile = worstYtile;
                }
                
            }
            listOfTile.Remove(firstTile);
            Debug.Log("uesh");
            Selection.activeGameObject = firstTile.gameObject;
            firstTile.SetColumnAndRow(0, 0);

            for (int i = 0; i < listOfTile.Count; i++)
            {
                listOfTile[i].SetColumnAndRow( Mathf.RoundToInt(firstTile.transform.position.x - listOfTile[i].transform.position.x), Mathf.RoundToInt(firstTile.transform.position.z - listOfTile[i].transform.position.z));
            }
        }
        EditorGUI.EndChangeCheck();
    }

}
