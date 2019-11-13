using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Ed_Mb_Generator))]
public class In_RoomCreator : Editor
{
    Ed_Mb_Generator mySelectedScript;
    SerializedProperty characterPrefab, charactSpectToInstantiateProperty, hostagePrefabProperty, hostageSpecProperty, playerTransformProperty, hostageTransformProperty;
    //Pour donner toutes les sorties au gameManager
    private List<Mb_Door> exitList;
    //Pour l'editor pour qu il sache quoi faire
    private UsedMode mode;
    private Color backGroundColorPlacingPlayer, backGroundColorErasingPlayer, backGroundColorAddHostage, backGroundColorErasingHostage = Color.red;

    // Selection.activeGameObject = mySelectedScript.gameObject;
    private void OnEnable()
    {
        mySelectedScript = target as Ed_Mb_Generator;
        characterPrefab = serializedObject.FindProperty("playerPrefab");
        charactSpectToInstantiateProperty = serializedObject.FindProperty("playerCharactSpectToInstantiate");
        hostagePrefabProperty = serializedObject.FindProperty("hostagePrefab");
        hostageSpecProperty = serializedObject.FindProperty("hostageCharactSpectToInstantiate");
        playerTransformProperty= serializedObject.FindProperty("playerTransform");
        hostageTransformProperty = serializedObject.FindProperty("hostageTransform");
        resetAllColor();
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
            Mb_Door[] temporaryList = FindObjectsOfType<Mb_Door>();
            for (int i = 0; i < temporaryList.Length; i++)
            {
                if (temporaryList[i].isExitDoor == true)
                {
                    exitList.Add(temporaryList[i]);
                }
            }
            UpdateAgentsLists();
            updateExits();
        }
        #endregion

        //AddPlayerPart
        #region
  
        EditorGUILayout.HelpBox("Lock the inspector and select the correct mode, then press A on a player or a tile to create or remove it", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(playerTransformProperty);
        EditorGUILayout.PropertyField(characterPrefab);
        EditorGUILayout.PropertyField(charactSpectToInstantiateProperty);
        EditorGUI.EndChangeCheck();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
        EditorGUILayout.BeginVertical();
        GUI.backgroundColor = backGroundColorPlacingPlayer;
        if (GUILayout.Button("AddPlayer", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.AddPlayer;
            CheckButtonColor();
        }
        GUI.backgroundColor = backGroundColorErasingPlayer;
        if (GUILayout.Button("RemovePlayer", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.RemovePlayer;
            CheckButtonColor();
        }
        GUI.backgroundColor = Color.grey;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

        //Add Hostage
        #region
        EditorGUILayout.HelpBox("Lock the inspector and select the correct mode, then press A on a Hostage or a tile to create or remove it", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(hostageTransformProperty);
        EditorGUILayout.PropertyField(hostagePrefabProperty);
        EditorGUILayout.PropertyField(hostageSpecProperty);
        EditorGUI.EndChangeCheck();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.BeginVertical();

        GUI.backgroundColor = backGroundColorAddHostage;
        if (GUILayout.Button("AddHostage", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.AddHostage;
            CheckButtonColor();
        }

        GUI.backgroundColor = backGroundColorErasingHostage;
        if (GUILayout.Button("RemoveHostage", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.RemoveHostage;
            CheckButtonColor();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        #endregion

    }
    void UpdateAgentsLists()
    {
        Ma_IAManager iaManage = GameObject.FindObjectOfType<Ma_IAManager>();
        iaManage.IAList = GameObject.FindObjectsOfType<Mb_IAHostage>();

        Ma_PlayerManager playerManage = GameObject.FindObjectOfType<Ma_PlayerManager>();
        playerManage.playerList = GameObject.FindObjectsOfType<Mb_Player>();
    }

    private void OnSceneGUI()
    {
        PlacingAndErasing();
    }

    private void OnDisable()
    {
        mode = UsedMode.none;
    }

    void updateExits()
    {
        Ma_LevelManager level = FindObjectOfType<Ma_LevelManager>();
        level.allExitDoors = exitList.ToArray();
    }

    void CheckButtonColor()
    {
        switch (mode)
        {
            case UsedMode.AddPlayer:
                resetAllColor();
                backGroundColorPlacingPlayer = Color.green;
                break;
            case UsedMode.RemovePlayer:
                resetAllColor();
                backGroundColorErasingPlayer = Color.green;
                break;
            case UsedMode.AddHostage:
                resetAllColor();
                backGroundColorAddHostage = Color.green;
                break;
            case UsedMode.RemoveHostage:
                resetAllColor();
                backGroundColorErasingHostage = Color.green;
                break;
            case UsedMode.none:
                resetAllColor();
                break;
        }
    }

    void resetAllColor()
    {
        backGroundColorPlacingPlayer = Color.red;
        backGroundColorErasingPlayer = Color.red;
        backGroundColorAddHostage = Color.red;
        backGroundColorErasingHostage = Color.red;
    }

    // ALED SIMON LE CONTROL Z D UN OBJET QUE JE CREER DANS UNE SCENE CA MARCHE     PAS

    //AddPlayerPart OnSceneGUI + erasePlayer

    void PlacingAndErasing()
    {
        if (Event.current.keyCode == KeyCode.A && Event.current.type == EventType.KeyUp)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            switch (mode)
            { 
                case UsedMode.AddHostage:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().avaible == true)
                    {
                        CreateHostage(charactSpectToInstantiateProperty.objectReferenceValue as Sc_Charaspec, hitInfo.collider.GetComponent<Tile>());
                        serializedObject.ApplyModifiedProperties();
                    }
                break;

                case UsedMode.RemoveHostage:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Mb_IAHostage>() == true)
                    {
                        EraseHostage(hitInfo.collider.GetComponent<Mb_IAHostage>());
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;

                case UsedMode.AddPlayer:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().avaible == true)
                    {
                        CreateCharacter(charactSpectToInstantiateProperty.objectReferenceValue as Sc_Charaspec, hitInfo.collider.GetComponent<Tile>());
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;

                case UsedMode.RemovePlayer:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Mb_Player>() == true)
                    {
                        EraseCharacter(hitInfo.collider.GetComponent<Mb_Player>());
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;
                    
            }
            UpdateAgentsLists();
        }
    }

    void CreateCharacter(Sc_Charaspec characterProperty, Tile hisTile)
    {
        //
        Mb_Player NewGameObject = PrefabUtility.InstantiatePrefab(characterPrefab.objectReferenceValue) as Mb_Player;
        Vector3 newpos = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        NewGameObject.transform.position = newpos;
        NewGameObject.GetComponent<Mb_Player>().charaPerks = characterProperty;
        NewGameObject.GetComponent<Mb_Player>().agentTile = hisTile;
        NewGameObject.transform.SetParent(playerTransformProperty.objectReferenceValue as Transform);
        hisTile.avaible = false;
        Selection.activeGameObject = NewGameObject.gameObject;
        EditorSceneManager.MarkAllScenesDirty();
    }

    void CreateHostage(Sc_Charaspec characterProperty, Tile hisTile)
    {
        Mb_IAHostage NewGameObject = PrefabUtility.InstantiatePrefab(hostagePrefabProperty.objectReferenceValue) as Mb_IAHostage;
        Vector3 newpos = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        NewGameObject.transform.position = newpos;
        NewGameObject.GetComponent<Mb_IAHostage>().charaPerks = characterProperty;
        NewGameObject.GetComponent<Mb_IAHostage>().agentTile = hisTile;
        NewGameObject.transform.SetParent(hostageTransformProperty.objectReferenceValue as Transform);
        hisTile.avaible = false;
        Selection.activeGameObject = NewGameObject.gameObject;
        EditorSceneManager.MarkAllScenesDirty();
    }

    void EraseCharacter(Mb_Player player)
    {
        player.agentTile.avaible = true;
        Undo.DestroyObjectImmediate(player.gameObject);
    }

    void EraseHostage(Mb_IAHostage hostage)
    {
        hostage.agentTile.avaible = true;
        Undo.DestroyObjectImmediate(hostage.gameObject);
    }

    //GENRE JUSQUE LA

    enum UsedMode
    {
        AddPlayer, RemovePlayer, AddHostage, RemoveHostage, none
    }
}
    