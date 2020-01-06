using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sc_Charaspec : ScriptableObject
{
    [Header("Interface")]
    public string characterName;
    public Sprite characterPortrait;

    [Header("Gameplay")]
    [Tooltip(" tick/case 1 is super fast 5 slow")] public float speed;
    public CharacterSkills[] characterSkills;
   

}

[System.Serializable]
public enum CharacterSkills
{
    LockPicking, NotLockPicking, Hacker, NotHacker, Inteligent, NotInteligent, Quick, NotQuick, Charismatic, NotCharismatic
}
