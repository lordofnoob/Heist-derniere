using System.Collections;
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
    private Color backGroundColorPlacingPlayer = Color.red;

    // Selection.activeGameObject = mySelectedScript.gameObject;
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

        //clean Room
        #region
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("CleanRoom", GUILayout.MinHeight(50)))
        {
            while (mySelectedScript.roomParent.childCount > 0)
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

        //Generate Grid
        #region
        if (GUILayout.Button("GenerateGrid", GUILayout.MinHeight(50)))
        {
            List<Tile> listOfTile = new List<Tile>();
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

            for (int i = 0; i < listOfTile.Count; i++)
            {

                Tile worstXtile = listOfTile[0];
                Tile worstYtile = listOfTile[0];
                Tile bestXtile = listOfTile[0];
                Tile bestYtile = listOfTile[0];

                if (i > 0)
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

            GameObject.FindObjectOfType<Ma_LevelManager>().allWalkableTile = listOfTileOfWalkableTile.ToArray(); ;
            EditorUtility.SetDirty(GameObject.FindObjectOfType<Ma_LevelManager>());
            /*for (int i =0; i < listOfTileOfWalkableTile.Count; i++)
                GameObject.FindObjectOfType<Ma_LevelManager>().allWalkableTile.Add(listOfTileOfWalkableTile[i]);*/
            for (int i = 0; i < listOfTile.Count; i++)
            {
                listOfTile[i].SetColumnAndRow(Mathf.RoundToInt(firstTile.transform.position.x - listOfTile[i].transform.position.x), Mathf.RoundToInt(firstTile.transform.position.z - listOfTile[i].transform.position.z));
                EditorUtility.SetDirty(listOfTile[i]);
            }


        }
        #endregion

        //AddPlayerPart
        #region
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(characterPrefab);
        EditorGUILayout.PropertyField(charactSpectToInstantiateProperty);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
        GUI.backgroundColor = backGroundColorPlacingPlayer;
        if (GUILayout.Button("AddPlayer", GUILayout.MinHeight(50)))
        {
            placingPlayerCharacter = !placingPlayerCharacter;

            if (placingPlayerCharacter == true)
            {
                GUI.backgroundColor = Color.green;
                backGroundColorPlacingPlayer = Color.green;
            }
            else
                backGroundColorPlacingPlayer = Color.red;
        }
        EditorGUILayout.EndHorizontal();
        #endregion
    }

    private void OnSceneGUI()
    {

        Placingcharacter();
    }

    private void OnDisable()
    {
        placingPlayerCharacter = false;
    }

    //AddPlayerPart OnSceneGUI
    void Placingcharacter()
    {
        if (Event.current.keyCode == KeyCode.A && Event.current.type == EventType.KeyUp)
        {
            if (placingPlayerCharacter == true)
            {
               
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().avaible == true)
                {
                    CreateCharacter(charactSpectToInstantiateProperty.objectReferenceValue as Sc_Charaspec, hitInfo.collider.GetComponent<Tile>());
                    Debug.Log(hitInfo.collider.GetComponent<Tile>().avaible);
                }
            }
        }
    }

    void CreateCharacter(Sc_Charaspec characterProperty, Tile hisTile)
    {
 ;
        Object newObject = PrefabUtility.InstantiatePrefab(characterPrefab.objectReferenceValue);
        GameObject NewGameObject = newObject as GameObject;

        NewGameObject.transform.position = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        if (NewGameObject.GetComponent<Mb_Player>() == true)
        {
            NewGameObject.GetComponent<Mb_Player>().characterProperty = characterProperty;
            NewGameObject.GetComponent<Mb_Player>().agentTile = hisTile;
        
            hisTile.avaible = false;
            Selection.activeGameObject = NewGameObject;
            EditorUtility.SetDirty(NewGameObject);
            EditorUtility.SetDirty(hisTile);
        }
         
          
 
    }
}
        //Mb_Player newMb_Player = Instantiate((characterPrefab.objectReferenceValue as GameObject).GetComponent<Mb_Player>());

      
    /**
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

               /*
               switch (e.type)
               {
                   case EventType.KeyDown:
                       {
                           if (Event.current.keyCode == (KeyCode.A))
                           {
                              
                               //&& hitInfo.collider.GetComponent<Tile>().avaible == true
                              
                           break;
                       }
               }

           */



