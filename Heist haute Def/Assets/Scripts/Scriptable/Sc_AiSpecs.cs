using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chara/AiSpecs")]
public class Sc_AiSpecs : Sc_Charaspec
{
    [Tooltip("Number of seconds lost when the ai esape")] public int escapeValue;
    [Tooltip(" tick/case 1 is super fast 5 slow")] public float normalSpeed;
    [Tooltip(" tick/case 1 is super fast 5 slow")] public float fleeingSpeed;
}
