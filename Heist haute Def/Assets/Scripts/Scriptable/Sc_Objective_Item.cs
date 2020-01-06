using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Newobjective", menuName = "CreateObjective/ItemObjective")]
public class Sc_Objective_Item : Sc_Objective
{
    public Sc_Items itemToSteal;
}
