using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Mb_Trial))]
public class In_Trial : Editor
{
    Mb_Trial mySelectedScript;
    SerializedProperty positionToGoProperty;
    UsedMode mode;

    public void OnEnable()
    {
        mySelectedScript = target as Mb_Trial;
        positionToGoProperty = serializedObject.FindProperty("positionToGo");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        if (GUILayout.Button("AddTileToGo", GUILayout.MinWidth(50)))
        {
            mode = UsedMode.AddTileToGo;
        }
      if (GUILayout.Button("RemoveTileToGo", GUILayout.MinWidth(50)))
      {
          mode = UsedMode.RemoveTileToGo;
      }
    }
    private void OnSceneGUI()
    {
        CheckEffect();
    }


    public void CheckEffect()
    {
        if (Event.current.keyCode == KeyCode.A && Event.current.type == EventType.KeyUp)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            switch (mode)
            {
                case UsedMode.AddTileToGo:
                    if (Physics.Raycast(worldRay, out hitInfo, 10000) && hitInfo.collider.GetComponent<Tile>().walkable == true)
                    {
                        positionToGoProperty.arraySize++;
                        positionToGoProperty.GetArrayElementAtIndex(positionToGoProperty.arraySize).objectReferenceValue = hitInfo.collider.GetComponent<Tile>();
                        serializedObject.ApplyModifiedProperties();
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
                                for (int i = 0; i < positionToGoTempList.Count; i++ )
                                {
                                    positionToGoProperty.GetArrayElementAtIndex(i).objectReferenceValue = positionToGoTempList[i];
                                }
                                serializedObject.ApplyModifiedProperties();
                            }
                        }
                   
                       
                    }
                    break;

            }
        }
    }

    enum  UsedMode
    {
        AddTileToGo, RemoveTileToGo, AddTileHostage, RemoveTileHostage, AddAffectedDoorTile, RemoveAffectedDoorTile
    }
    //POUR LES REODABLE ON VERA LA 
    /*
     *    public void OnEnable()
    {

        mySelectedScript = target as Mb_Trial;
        myList = new ReorderableList(serializedObject, serializedObject.FindProperty("positionToGo"), true, true, true, true);
        // les quatre bools du constructeur correspondent à : draggable, display header, display "add" button, display "remove" button
        positionToGoProperty =serializedObject.FindProperty("positionToGo");

       /* myList.drawHeaderCallback = MyListHeader;
        myList.drawElementCallback = MyListElementDrawer;
        myList.onAddCallback += MyListAddCallback;
        myList.onRemoveCallback += MyListRemoveCallback;
        
        myList.onReorderCallback += (ReorderableList list) => { Debug.Log("la liste vient d'être réordonnée"); };
        myList.elementHeightCallback += (int index) => { return myList.elementHeight* 2; };
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // serializedObject.Update();

        /* GUILayout.Space(8);

         serializedObject.ApplyModifiedProperties();

        myList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        // EditorUtility.SetDirty(mySelectedScript);

    }
    void MyListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "All the position to interact with the trial");
    }
    //positionToGoProperty.GetArrayElementAtIndex(index)//
    void MyListElementDrawer(Rect rect, int index, bool isActive, bool isFocused)
    {
     //   positionToGoProperty = serializedObject.FindProperty("positionToGo");
        rect.yMin += 2;
        rect.yMax -= 4;
        EditorGUI.PropertyField(rect, positionToGoProperty, new GUIContent("Position number "+ index + index.ToString()));
    }

    void MyListAddCallback(ReorderableList rlist)
    {
        positionToGoProperty.arraySize++;
        SerializedProperty newElement = positionToGoProperty.GetArrayElementAtIndex(positionToGoProperty.arraySize - 1);
        //newElement.FindPropertyRelative("positionToGo").objectReferenceValue = ;
    }

    void MyListRemoveCallback(ReorderableList rlist)
    {
        positionToGoProperty.DeleteArrayElementAtIndex(rlist.index);
    }*/
}
