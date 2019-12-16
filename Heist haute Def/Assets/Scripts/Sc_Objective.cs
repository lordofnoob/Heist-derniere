using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Objective : MonoBehaviour
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
