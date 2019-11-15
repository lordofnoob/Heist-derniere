using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Mb_Chest))]
public class In_Chest : In_Trial
{
    Mb_Chest mySelectedScript;
    SerializedProperty listOfReward;

    private void OnEnable()
    {
        mySelectedScript = target as Mb_Chest;
    }

    public override void OnInspectorGUI()
    {

    }
}
