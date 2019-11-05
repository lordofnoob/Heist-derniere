using System.Collections;
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
            Debug.Log(mySelectedScript.roomParent.childCount);
            /*for (int i = mySelectedScript.roomParent.GetChildCount(); i>0; i++)
            {
                DestroyImmediate(mySelectedScript.roomParent.GetChild(i).gameObject);
            }*/
            while (mySelectedScript.roomParent.childCount>0)
            {
                DestroyImmediate(mySelectedScript.roomParent.GetChild(0).gameObject);
            }
        }
        EditorGUI.EndChangeCheck();
    }
}
