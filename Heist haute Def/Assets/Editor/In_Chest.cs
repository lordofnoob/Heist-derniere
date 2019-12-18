using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Mb_Chest))]
public class In_Chest : Editor
{
    Mb_Chest mySelectedScript;
    SerializedProperty positionToGoProperty;
    UsedMode mode;
    Color addTileColor = Color.red, RemoveTileColor = Color.red;
    Color CleanAllTilesColor = Color.cyan;

    public void OnEnable()
    {

        mySelectedScript = target as Mb_Chest;
        positionToGoProperty = serializedObject.FindProperty("positionToGo");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        GUI.backgroundColor = addTileColor;
        if (GUILayout.Button("AddTileToGo", GUILayout.MinWidth(50)) )
        {
            ResetAllColors();
            if (mode == UsedMode.AddTileToGo)
            {
               
                mode = UsedMode.None;
            }
            else
            {
                addTileColor = Color.green;
                mode = UsedMode.AddTileToGo;
            }
             
        }
        GUI.backgroundColor = RemoveTileColor;
        if (GUILayout.Button("RemoveTileToGo", GUILayout.MinWidth(50)))
        {
            ResetAllColors();
            if (mode == UsedMode.RemoveTileToGo)
            {
                mode = UsedMode.None;
            }
            else
            {
               RemoveTileColor = Color.green;
                mode = UsedMode.RemoveTileToGo;
            }
        }
        GUI.backgroundColor = CleanAllTilesColor;
        if (GUILayout.Button("CleanAllTiles", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.None;
            positionToGoProperty.arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        }
    }
    void ResetAllColors()
    {
        addTileColor = RemoveTileColor = Color.red;
    }

    private void OnSceneGUI()
    {
        CheckEffect();
    }

    private void ExitGUI()
    {
        ResetAllColors();
    }

    public void CheckEffect()
    {
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.A)
        {
            
            Debug.Log("before" + positionToGoProperty.arraySize);
            Debug.Log(mode);
            
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            switch (mode)
            {
                case UsedMode.AddTileToGo:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().walkable == true)
                    {
                        //nettoyage de la liste si elle a 1 element vide 
                       if (positionToGoProperty.arraySize >0)
                           if(positionToGoProperty.GetArrayElementAtIndex(0).objectReferenceValue == null)
                            positionToGoProperty.arraySize = 0;


                        serializedObject.ApplyModifiedProperties();

                        positionToGoProperty.arraySize += 1;
                        List<Tile> tempListTile = new List<Tile>();

                        foreach (Tile tilestoGo in mySelectedScript.positionToGo)
                        {
                            tempListTile.Add(tilestoGo);
                        }
                        
                        
                        
                        bool alreadyHere = false;

                        for (int i = 0; i < tempListTile.Count; i++)
                        {
                            if (tempListTile[i] != hitInfo.collider.GetComponent<Tile>() && alreadyHere == false)
                            {
                                tempListTile.Add(hitInfo.collider.GetComponent<Tile>());
                                alreadyHere = true;
                                Debug.Log("YAS");

                                for (int y = 0; y < tempListTile.Count; y++)
                                {
                                    positionToGoProperty.GetArrayElementAtIndex(y).objectReferenceValue = tempListTile[y];
                                    
                                }
                                serializedObject.ApplyModifiedProperties();
                                break;
                            }
                            else
                            {
                                positionToGoProperty.arraySize -= 1;
                                Debug.Log("Already in the list");
                                break;  
                            }
                        }
                       
                    } 
                    break;

                case UsedMode.RemoveTileToGo:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>())
                    {
            
                        foreach (Tile potentialRemovableTile in positionToGoProperty)
                        {
                            if (hitInfo.collider.gameObject == potentialRemovableTile.gameObject)
                            {
                                List<Tile> positionToGoTempList = new List<Tile>();
                                for (int i = 0; i < mySelectedScript.positionToGo.Length; i++)
                                {
                                    positionToGoTempList.Add(mySelectedScript.positionToGo[i]);
                                }
                                positionToGoTempList.Remove(hitInfo.collider.GetComponent<Tile>());
                                positionToGoProperty.arraySize = positionToGoTempList.Count;
                                for (int i = 0; i < positionToGoTempList.Count; i++)
                                {
                                    positionToGoProperty.GetArrayElementAtIndex(i).objectReferenceValue = positionToGoTempList[i];
                                }
                            }
                        }
            
            
                    }
                    break;
            }

            
        }
        serializedObject.ApplyModifiedProperties();
    }

    enum UsedMode
    {
        AddTileToGo, RemoveTileToGo, AddTileHostage, RemoveTileHostage, AddAffectedDoorTile, RemoveAffectedDoorTile, None
    }

}
