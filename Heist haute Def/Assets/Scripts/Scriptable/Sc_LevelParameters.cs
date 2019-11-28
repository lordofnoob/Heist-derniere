using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevelParameters", menuName = "levelParameter") ]
public class Sc_LevelParameters : ScriptableObject
{
    public float timeAvaibleBeforePolice;
    public float totalTimeToComplete;
}
