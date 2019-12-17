using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Newobjective", menuName = "CreateObjective")]
public class Sc_Objective : ScriptableObject
{
    [TextArea] public string objectifDescription;
    public objectiveType objectiveType;
    public Sc_Items itemToGet;
    public float valueToGet;
}

public enum objectiveType
{
    Time,Money,Item
}
