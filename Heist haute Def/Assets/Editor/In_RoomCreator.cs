﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Ed_Mb_Generator))]
public class In_RoomCreator : Editor
{
    Ed_Mb_Generator mySelectedScript;
    SerializedProperty characterPrefab;
    SerializedProperty charactSpectToInstantiateProperty;
    private bool placingPlayerCharacter;

    private void OnEnable()
    {
        mySelectedScript = target as Ed_Mb_Generator;
        characterPrefab = serializedObject.FindProperty("playerPrefab");
        charactSpectToInstantiateProperty = serializedObject.FindProperty("charactSpectToInstantiate");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();

        if (GUILayout.Button("GenerateRoom", GUILayout.MinHeight(50)))
        {
            if (mySelectedScript.roomParent.childCount == 0)
                mySelectedScript.InstantiateRoom();
            else
                Debug.Log("AlreadyProduced");
           
        }
        #region
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("CleanRoom", GUILayout.MinHeight(50)))
        {
            while (mySelectedScript.roomParent.childCount>0)
            {
                Undo.DestroyObjectImmediate(mySelectedScript.roomParent.GetChild(0).gameObject);
            }
        }
        if (GUILayout.Button("CleanAllRooms", GUILayout.MinHeight(50)))
        {
            for (int i = 0; i < mySelectedScript.transform.childCount; i++)
            {
                while (mySelectedScript.transform.GetChild(i).transform.childCount > 0)
                {
                    Undo.DestroyObjectImmediate(mySelectedScript.transform.GetChild(i).GetChild(0).gameObject);
                }
            }
        }
        GUILayout.EndHorizontal();
        #endregion

        if (GUILayout.Button("GenerateGrid", GUILayout.MinHeight(50)))
        {
            List<Tile> listOfTile =  new List<Tile>();
            List<Tile> listOfTileOfWalkableTile = new List<Tile>();
            for (int j = 0; j < mySelectedScript.transform.childCount; j++)
            {
                for (int i = 0; i < mySelectedScript.transform.GetChild(j).childCount; i++)
                { 
                if (mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>() == true)
                    {
                        listOfTile.Add(mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>());
                        if (mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>().walkable == true)
                            listOfTileOfWalkableTile.Add(mySelectedScript.transform.GetChild(j).GetChild(i).GetComponent<Tile>());
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
                       // Debug.Log(bestXtile.transform.position.x);
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
            firstTile.SetColumnAndRow(0, 0);


            GameObject.FindObjectOfType<Ma_LevelManager>().allWalkableTile = listOfTileOfWalkableTile;
        /*    for (int i =0; i < listOfTileOfWalkableTile.Count; i++)
                GameObject.FindObjectOfType<Ma_LevelManager>().allWalkableTile.Add(listOfTileOfWalkableTile[i]);*/
            for (int i = 0; i < listOfTile.Count; i++)
            {
                listOfTile[i].SetColumnAndRow( Mathf.RoundToInt(firstTile.transform.position.x - listOfTile[i].transform.position.x), Mathf.RoundToInt(firstTile.transform.position.z - listOfTile[i].transform.position.z));
            }

      
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(characterPrefab);
        EditorGUILayout.PropertyField(charactSpectToInstantiateProperty);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();

        var style = new GUIStyle(GUI.skin.box);
        //style.normal.background = Color.red;
        if (GUILayout.Button("AddPlayer", style, GUILayout.MinHeight(50)))
        {
            placingPlayerCharacter = !placingPlayerCharacter;
            if (placingPlayerCharacter == true)
            {
                Selection.activeGameObject = mySelectedScript.gameObject;
               // style.
            }
        }


        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        Placingcharacter();
    }
    private void OnDisable()
    {
        placingPlayerCharacter = false;
    }

    void Placingcharacter()
    {
      
        if (Input.GetKey(KeyCode.W) && placingPlayerCharacter == true)
        {
           
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().avaible == true)
            {
                /*
                // Load the current prefab
                string path = "Assets/Prefabs/" + __typeStrings[__currentType] + ".prefab";
                GameObject anchor_point = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                // Instance this prefab
                GameObject prefab_instance = PrefabUtility.InstantiatePrefab(anchor_point) as GameObject;
                // Place the prefab at correct position (position of the hit).
                prefab_instance.transform.position = hitInfo.point;
                prefab_instance.transform.parent = __objectGroup.transform;
                // Mark the instance as dirty because we like dirty
                EditorUtility.SetDirty(prefab_instance); */
             
                hitInfo.collider.GetComponent<Tile>().avaible = false;
                Debug.Log(hitInfo.collider.GetComponent<Tile>().avaible);
                Object newObject = PrefabUtility.InstantiatePrefab(characterPrefab.objectReferenceValue);
                Debug.Log(characterPrefab.objectReferenceValue);

                if (newObject is Mb_Player)
                {
                    GameObject newGameObject = (newObject as GameObject);
                    Mb_Player newPlayer = newGameObject.GetComponent<Mb_Player>();
                    Selection.activeGameObject = newGameObject;
                    newPlayer.characterProperty = (charactSpectToInstantiateProperty.objectReferenceValue as Sc_Charaspec);
                    newGameObject.transform.position = new Vector3(hitInfo.collider.transform.position.x, hitInfo.collider.transform.position.y + hitInfo.collider.transform.localScale.y / 2, hitInfo.collider.transform.position.z);
    
                    EditorUtility.SetDirty(newGameObject);
                }
            }
        }
    }

}
