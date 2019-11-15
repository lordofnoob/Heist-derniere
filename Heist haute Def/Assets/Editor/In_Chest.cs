using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mb_Chest))]
public class In_Chest : Editor
{
    Mb_Chest mySelectedScript;
    SerializedProperty positionToGoProperty;
    UsedMode mode;

    public void OnEnable()
    {

        mySelectedScript = target as Mb_Chest;
        positionToGoProperty = serializedObject.FindProperty("positionToGo");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        if (GUILayout.Button("AddTileToGo", GUILayout.MinWidth(50)) )
        {
            if (mode == UsedMode.AddTileToGo)
                mode = UsedMode.None;
            else
                mode = UsedMode.AddTileToGo;
        }
        if (GUILayout.Button("RemoveTileToGo", GUILayout.MinWidth(50)))
        {
            if (mode == UsedMode.RemoveTileToGo)
                mode = UsedMode.None;
            else
                mode = UsedMode.RemoveTileToGo;
        }
        if (GUILayout.Button("CleanAllTiles", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.None;
            positionToGoProperty.arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnSceneGUI()
    {
        CheckEffect();
    }


    public void CheckEffect()
    {
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.A)
        {
            Debug.Log("before" + positionToGoProperty.arraySize);
            
            
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            switch (mode)
            {
                case UsedMode.AddTileToGo:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().walkable == true)
                    {
                        positionToGoProperty.arraySize += 1;
                        Debug.Log(positionToGoProperty.GetArrayElementAtIndex(0).objectReferenceValue);
                        Debug.Log(hitInfo.collider.gameObject);
                     
                        positionToGoProperty.GetArrayElementAtIndex(positionToGoProperty.arraySize - 1).objectReferenceValue = hitInfo.collider.GetComponent<Tile>();
                        bool alreadyHere = false;

                        for (int i = 0; i < positionToGoProperty.arraySize; i++)
                        {
                            if (positionToGoProperty.GetArrayElementAtIndex(i).objectReferenceValue != hitInfo.collider.GetComponent<Tile>() && alreadyHere == false)
                            {
                                alreadyHere = true;
                                Debug.Log("YAS");
                                break;
                            }
                            else
                            {
                                Debug.Log("Already in the list");
                                positionToGoProperty.arraySize -= 1;
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
