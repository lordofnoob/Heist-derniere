using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

//[CustomEditor(typeof(Mb_Trial))]
public class In_Trial : Editor
{
    Mb_Trial mySelectedScript;
    SerializedProperty positionToGoProperty;

    public void OnEnable()
    {
        mySelectedScript = target as Mb_Trial;
        //positionToGoProperty = 
    }

    public override void OnInspectorGUI()
    {

    }
}
