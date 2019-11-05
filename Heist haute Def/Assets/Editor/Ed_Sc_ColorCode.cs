using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="ColorCode")]
public class Ed_Sc_ColorCode : ScriptableObject
{
    public List<ColorToValue> colorCode;
}
public struct ColorToValue
{
    public Color colorAssociated;
    public GameObject prefabAssociated;
}
