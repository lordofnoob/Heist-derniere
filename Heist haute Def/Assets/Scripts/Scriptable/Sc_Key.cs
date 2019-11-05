using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Item/Key")]
public class Sc_Key : Sc_Items
{
    public KeyColor colorAssociated;

}

public enum KeyColor
{
    Red, Blue, Green, Yellow, Purple, Grey
}
