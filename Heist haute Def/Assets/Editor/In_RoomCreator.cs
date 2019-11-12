using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Ed_Mb_Generator))]
public class In_RoomCreator : Editor
{
    Ed_Mb_Generator mySelectedScript;
    SerializedProperty characterPrefab;
    SerializedProperty charactSpectToInstantiateProperty;
    //Pour donner toutes les sorties au gameManager
    private List<Mb_Door> exitList;
    //Pour l'editor pour qu il sache quoi faire
    private bool placingPlayerCharacter;
    private bool erasingPlayerCharacter;
    private Color backGroundColorPlacingPlayer, backGroundColorErasingPlayer, backGroundColorAddHostage, backGroundColorErasingHostage = Color.red;

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
            //nettoyer toiute la liste des tuiles de sorties
            exitList = new List<Mb_Door>();
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

            // check de chacune des portes pour verifier si faut les rajouter en sortie ou pas
            Mb_Door[] temporaryList= FindObjectsOfType<Mb_Door>();
            for (int i = 0; i < temporaryList.Length; i++)
            {
                if (temporaryList[i].isExitDoor == true)
                {
                    Debug.Log(temporaryList[i]);
                    exitList.Add(temporaryList[i]);
                }
            }

            updateExits();
        }
        #endregion

        //AddPlayerPart
        #region
        EditorGUILayout.HelpBox("Lock the inspector and select the correct mode, then press A on a player or a tile to create or remove it", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(characterPrefab);
        EditorGUILayout.PropertyField(charactSpectToInstantiateProperty);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
        EditorGUILayout.BeginVertical();
        GUI.backgroundColor = backGroundColorPlacingPlayer;
        if (GUILayout.Button("AddPlayer", GUILayout.MinWidth(50)))
        {
            placingPlayerCharacter = !placingPlayerCharacter;
            erasingPlayerCharacter = false;
            CheckButtonColor();
        }
        GUI.backgroundColor = backGroundColorErasingPlayer;
        if (GUILayout.Button("RemovePlayer", GUILayout.MinWidth(50)))
        {
            placingPlayerCharacter = false;
            erasingPlayerCharacter = !erasingPlayerCharacter;
            CheckButtonColor();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

        //Add Hostage
        EditorGUILayout.HelpBox("Lock the inspector and select the correct mode, then press A on a Hostage or a tile to create or remove it", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(characterPrefab);
        EditorGUILayout.PropertyField(charactSpectToInstantiateProperty);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
        EditorGUILayout.BeginVertical();
        GUI.backgroundColor = backGroundColorPlacingPlayer;
        if (GUILayout.Button("AddPlayer", GUILayout.MinWidth(50)))
        {
            placingPlayerCharacter = !placingPlayerCharacter;
            erasingPlayerCharacter = false;
            CheckButtonColor();
        }
        GUI.backgroundColor = backGroundColorErasingPlayer;
        if (GUILayout.Button("RemovePlayer", GUILayout.MinWidth(50)))
        {
            placingPlayerCharacter = false;
            erasingPlayerCharacter = !erasingPlayerCharacter;
            CheckButtonColor();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


    }

    void CheckButtonColor()
    {
        if (placingPlayerCharacter == true)
        {
            backGroundColorPlacingPlayer = Color.green;
        }
        else
            backGroundColorPlacingPlayer = Color.red;

        if (erasingPlayerCharacter == true)
        {
            backGroundColorErasingPlayer = Color.green;
        }
        else
            backGroundColorErasingPlayer = Color.red;
    }

    private void OnSceneGUI()
    {
        Placingcharacter();
        ErasingCharacter();
    }

    private void OnDisable()
    {
        placingPlayerCharacter = false;
    }

    //AddPlayerPart OnSceneGUI + erasePlayer
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
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
    //Erase Player
    void ErasingCharacter()
    {
        if (Event.current.keyCode == KeyCode.A && Event.current.type == EventType.KeyUp)
        {
            if (erasingPlayerCharacter == true)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Mb_Player>() == true)
                {
                    Debug.Log("yes");
                    EraseCharacter(hitInfo.collider.GetComponent<Mb_Player>());
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
    void updateExits()
    {
        Ma_LevelManager level = FindObjectOfType<Ma_LevelManager>();
        level.allExitDoors = exitList.ToArray();
    }


    // ALED SIMON LE CONTROL Z D UN OBJET QUE JE CREER DANS UNE SCENE CA MARCHE     PAS
    void CreateCharacter(Sc_Charaspec characterProperty, Tile hisTile)
    {
        //
        Mb_Player NewGameObject = PrefabUtility.InstantiatePrefab(characterPrefab.objectReferenceValue) as Mb_Player;
        Vector3 newpos = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        NewGameObject.transform.position = newpos;
        NewGameObject.GetComponent<Mb_Player>().characterProperty = characterProperty;
        NewGameObject.GetComponent<Mb_Player>().agentTile = hisTile;
        hisTile.avaible = false;
        Selection.activeGameObject = NewGameObject.gameObject;
   /*     Undo.RecordObject(hisTile,"newObject");
        Undo.RegisterCreatedObjectUndo(NewGameObject as Object,"lastItemCreated");
        // Undo.RegisterSceneUndo(EditorSceneManager.GetActiveScene().ToString());
    //    Undo.RegisterCreatedObjectUndo(hisTile as Object, "lastItemCreated");*/
        EditorSceneManager.MarkAllScenesDirty();
  
    }

    void EraseCharacter(Mb_Player player)
    {
        player.agentTile.avaible = true;
        Undo.DestroyObjectImmediate(player.gameObject);
    }
}
    