using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Ed_Mb_Generator))]
public class In_RoomCreator : Editor
{
    Ed_Mb_Generator mySelectedScript;
    SerializedProperty allRoomTransform, characterPrefab, charactSpectToInstantiateProperty, hostagePrefabProperty, hostageSpecProperty, playerTransformProperty, hostageTransformProperty;
    SerializedProperty wallConfigProperty;
    //Pour donner toutes les sorties au gameManager
    private List<Tile> exitList;
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
        allRoomTransform = serializedObject.FindProperty("allRoomTransform");
        wallConfigProperty = serializedObject.FindProperty("wallConfig");
        resetAllColor();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
      
        //Generate Room Button
        #region
        serializedObject.Update();

        if (GUILayout.Button("GenerateRoom", GUILayout.MinHeight(50)))
        {
            if (mySelectedScript.roomParent.childCount == 0)
                mySelectedScript.InstantiateRoom();
            else
                Debug.Log("AlreadyProduced");

        }
        #endregion

        //clean Room Button
        #region
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("CleanRoom", GUILayout.MinHeight(50)))
        {
            while (mySelectedScript.roomParent.childCount > 0)
            {
                Undo.DestroyObjectImmediate(mySelectedScript.roomParent.GetChild(0).gameObject);
            }
        }
        /*
        if (GUILayout.Button("CleanAllRooms", GUILayout.MinHeight(50)))
        {
            for (int i = 0; i < mySelectedScript.roomParent.childCount; i++)
            {
                while (mySelectedScript.roomParent.transform.GetChild(i).transform.childCount > 0)
                {
                    Undo.DestroyObjectImmediate(mySelectedScript.roomParent.GetChild(i).GetChild(0).gameObject);
                }
            }
        }*/
        GUILayout.EndHorizontal();
        #endregion

        //Generate Grid Button
        #region
        if (GUILayout.Button("GenerateGrid", GUILayout.MinHeight(50)))
        {
            GenerateGrid();
           
            UpdateAgentsLists();
            UpdateExits();
            SetWallGraphs();

        }
        #endregion

        //AddPlayerPart Button
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
        GUI.backgroundColor = Color.gray;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

        //Add Hostage Button
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
        iaManage.IAList = GameObject.FindObjectsOfType<Mb_IAAgent>();

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
    //Editor Button etc
    #region
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
    #endregion



    // ALED SIMON LE CONTROL Z D UN OBJET QUE JE CREER DANS UNE SCENE CA MARCHE     PAS

    void SetWallGraphs()
    {
        Tile[] allTiles = GameObject.FindObjectsOfType<Tile>();
        List<Tile> temporaryTileList = new List<Tile>();
        Tile[] allWalls;
        foreach (Tile tile in allTiles)
        {
            if (tile.tileType == Tile.TileType.Wall)
                temporaryTileList.Add(tile);
        }
        allWalls = temporaryTileList.ToArray();

        foreach (Tile wallTile in allWalls)
        {
            wallTile.SetStructNeighbours();
            List<CombinableWallType> WallTypeList = new List<CombinableWallType>();

            if (wallTile.neighbours.North != null)
                if (wallTile.neighbours.North.tileType != Tile.TileType.Other)
                    WallTypeList.Add(CombinableWallType.Up);
            if (wallTile.neighbours.South != null)
                if (wallTile.neighbours.South.tileType != Tile.TileType.Other)
                    WallTypeList.Add(CombinableWallType.Down);
            if (wallTile.neighbours.East != null)
                if (wallTile.neighbours.East.tileType != Tile.TileType.Other)
                    WallTypeList.Add(CombinableWallType.Right);
            if (wallTile.neighbours.West != null)
                if (wallTile.neighbours.West.tileType != Tile.TileType.Other)
                    WallTypeList.Add(CombinableWallType.Left);


            //Quand on voudra detecter l exterieur 
            /*         if (wallTile.neighbours.NE.tileType == Tile.TileType.Wall)
                         WallTypeList.Add(CombinableWallType.LeftUp);
                     if (wallTile.neighbours.NW.tileType == Tile.TileType.Wall)
                         WallTypeList.Add(CombinableWallType.RightUp);
                     if (wallTile.neighbours.SE.tileType == Tile.TileType.Wall)
                         WallTypeList.Add(CombinableWallType.LeftDown);
                     if (wallTile.neighbours.SW.tileType == Tile.TileType.Wall)
                         WallTypeList.Add(CombinableWallType.RightDown);*/

            CombinableWallType DefinitiveType = CombinableWallType.None;
            for (int i = 0; i < WallTypeList.Count; i++)
            {
                DefinitiveType |= WallTypeList[i];

            }

            Sc_WallConfiguration wallConfig = wallConfigProperty.objectReferenceValue as Sc_WallConfiguration;

            int randomWeightCumulated = 0;
            int totalWeight = 0;

            for (int i = 0; i < wallConfig.wallConfiguration.Length; i++)
            {

                if (DefinitiveType == wallConfig.wallConfiguration[i].walltype)
                {
                    //Pour l'aléat pondéré faudra reprendre la ligne la
                    Tile newGameObject = PrefabUtility.InstantiatePrefab(wallConfig.wallConfiguration[i].associatedTiles[0].associatedTile) as Tile;
                    newGameObject.transform.position = wallTile.transform.position;
                    newGameObject.transform.SetParent(wallTile.transform.parent);
                    DestroyImmediate(wallTile.gameObject);
                    GenerateGrid();
                }

                /* for (int j = 0; j < wallConfig.wallConfiguration[i].associatedTiles.Length; j++)
               {
                   totalWeight += wallConfig.wallConfiguration[i].associatedTiles[j].weight;
               }

               int randomTileToGenerate = Random.Range(0, totalWeight);

               for (int k= 0; k < wallConfig.wallConfiguration[i].associatedTiles.Length; k++)
               {
               randomWeightCumulated += wallConfig.wallConfiguration[i].associatedTiles[k].weight;

                   Debug.Log(randomTileToGenerate);
                   Debug.Log(randomWeightCumulated);
                   if (randomTileToGenerate > randomWeightCumulated)
                   {
                       Debug.Log("goPourvoir");
                       Debug.Log(randomWeightCumulated);*/

                //}
            }


        }


    }

    //AddPlayerPart OnSceneGUI + erasePlayer
    void GenerateGrid()
    {

        Mb_Door[] doorListTemp;
        doorListTemp = GameObject.FindObjectsOfType<Mb_Door>();
        GameObject.FindObjectOfType<Ma_LevelManager>().allDoor = doorListTemp;


        
            Tile[] listOfTile;
           List<Tile> listOfTileOfWalkableTile = new List<Tile>();

            exitList = new List<Tile>();

   
            listOfTile = GameObject.FindObjectsOfType<Tile>();
            for (int i = 0; i < listOfTile.Length; i++)
                if (listOfTile[i].walkable == true)
                    listOfTileOfWalkableTile.Add(listOfTile[i]);
            GameObject.FindObjectOfType<Ma_LevelManager>().allWalkableTile = listOfTileOfWalkableTile.ToArray();
            GameObject.FindObjectOfType<Ma_LevelManager>().allTiles = listOfTile;
            Tile firstTile = listOfTile[0];

            //test
            #region
            /*
            for (int j = 0; j < mySelectedScript.allRoomTransform.transform.childCount; j++)
            {
                for (int i = 0; i < mySelectedScript.allRoomTransform.GetChild(j).childCount; i++)
                {
                    if (mySelectedScript.allRoomTransform.transform.GetChild(j).GetChild(i).GetComponent<Tile>() == true)
                    {
                        listOfTile.Add(mySelectedScript.allRoomTransform.GetChild(j).GetChild(i).GetComponent<Tile>());
                        if (mySelectedScript.allRoomTransform.GetChild(j).GetChild(i).GetComponent<Tile>().walkable == true)
                            listOfTileOfWalkableTile.Add(mySelectedScript.allRoomTransform.GetChild(j).GetChild(i).GetComponent<Tile>());

                    }
                }
            }*/

            //nettoyer toiute la liste des tuiles de sorties


            /*
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

            }*/
            #endregion

            List<Tile> tempListOfTile = new List<Tile>();
            foreach (Tile tileToAdd in listOfTile)
            {
                tempListOfTile.Add(tileToAdd);
            }
            tempListOfTile.Remove(firstTile);

            listOfTile =tempListOfTile.ToArray();
            firstTile.SetColumnAndRow(0, 0);


            EditorUtility.SetDirty(GameObject.FindObjectOfType<Ma_LevelManager>());
  
            // SI LA TAILLE DES TUILES CHANGE FAUDRA CHANGER ICI
            for (int i = 0; i < listOfTile.Length; i++)
            {
                string newName= "";
                listOfTile[i].SetColumnAndRow(Mathf.RoundToInt(firstTile.transform.position.x - listOfTile[i].transform.position.x), Mathf.RoundToInt(firstTile.transform.position.z - listOfTile[i].transform.position.z));
                if (listOfTile[i].GetComponentInChildren<Mb_Chest>())
                    newName = ("Chest " + listOfTile[i].row + " - " + listOfTile[i].column);
                else if (listOfTile[i].GetComponent<Tile>().tileType == Tile.TileType.Wall)
                    newName = ("Wall " + listOfTile[i].row + " - " + listOfTile[i].column);
                else if (listOfTile[i].GetComponentInChildren<Mb_Door>())
                    newName = ("Door " + listOfTile[i].row + " - " + listOfTile[i].column);
                else if (listOfTile[i].GetComponentInChildren<Mb_LockedDoor>())
                    newName = ("LockedDoor " + listOfTile[i].row + " - " + listOfTile[i].column);
                else if (listOfTile[i].GetComponentInChildren<Mb_HostageStockArea>())
                    newName = ("HostageStock " + listOfTile[i].row + " - " + listOfTile[i].column);
                else
                    newName = ("Tile " + listOfTile[i].row + " - " + listOfTile[i].column);

                listOfTile[i].name = newName;
                EditorUtility.SetDirty(listOfTile[i]);
            }

         
        SetUiManagerLists();
    }

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
                        CreateHostage(charactSpectToInstantiateProperty.objectReferenceValue as Sc_AiSpecs, hitInfo.collider.GetComponent<Tile>());
                        serializedObject.ApplyModifiedProperties();
                    }
                break;

                case UsedMode.RemoveHostage:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Mb_IAAgent>() == true)
                    {
                        EraseHostage(hitInfo.collider.GetComponent<Mb_IAAgent>());
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;

                case UsedMode.AddPlayer:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().avaible == true)
                    {
                        CreateCharacter(charactSpectToInstantiateProperty.objectReferenceValue as Sc_PlayerSpecs, hitInfo.collider.GetComponent<Tile>());
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

    void CreateCharacter(Sc_PlayerSpecs characterProperty, Tile hisTile)
    {
        //
        Mb_Player NewGameObject = PrefabUtility.InstantiatePrefab(characterPrefab.objectReferenceValue) as Mb_Player;
        Vector3 newpos = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        NewGameObject.transform.position = newpos;
        NewGameObject.charaPerks = characterProperty;
        NewGameObject.AgentTile = hisTile;
        NewGameObject.transform.SetParent(playerTransformProperty.objectReferenceValue as Transform);
        Selection.activeGameObject = NewGameObject.gameObject;
        EditorUtility.SetDirty(NewGameObject);
        EditorUtility.SetDirty(hisTile);
        EditorSceneManager.MarkAllScenesDirty();
    }

    void CreateHostage(Sc_AiSpecs characterProperty, Tile hisTile)
    {
        Mb_IAAgent NewGameObject = PrefabUtility.InstantiatePrefab(hostagePrefabProperty.objectReferenceValue) as Mb_IAAgent;
        Vector3 newpos = new Vector3(hisTile.transform.position.x, hisTile.transform.position.y + hisTile.transform.localScale.y / 2, hisTile.transform.position.z);
        NewGameObject.transform.position = newpos;
        NewGameObject.aiCharacteristics = characterProperty;
        NewGameObject.AgentTile = hisTile;
        NewGameObject.transform.SetParent(hostageTransformProperty.objectReferenceValue as Transform);
        Debug.Log(hostageTransformProperty);
        Selection.activeGameObject = NewGameObject.gameObject;
        EditorUtility.SetDirty(NewGameObject);
        EditorUtility.SetDirty(hisTile);
        EditorSceneManager.MarkAllScenesDirty();
    }

    void EraseCharacter(Mb_Player player)
    {
        player.AgentTile.avaible = true;
        Undo.DestroyObjectImmediate(player.gameObject);
    }

    void EraseHostage(Mb_IAAgent hostage)
    {
        hostage.AgentTile.avaible = true;
        Undo.DestroyObjectImmediate(hostage.gameObject);
    }

    void SetUiManagerLists()
    {
        List<Mb_HostageStockArea> tempList = new List<Mb_HostageStockArea>();
        tempList.AddRange(FindObjectsOfType<Mb_HostageStockArea>());
        FindObjectOfType<UIManager>().hostageStockArea.Clear();
        FindObjectOfType<UIManager>().hostageStockArea.AddRange(tempList);

    }

    void UpdateExits()
    {
        Ma_LevelManager level = FindObjectOfType<Ma_LevelManager>();
        level.escapeTrial = FindObjectOfType<Mb_Escape>();
    }

    //GENRE JUSQUE LA

    enum UsedMode
    {
        AddPlayer, RemovePlayer, AddHostage, RemoveHostage, none
    }
}

