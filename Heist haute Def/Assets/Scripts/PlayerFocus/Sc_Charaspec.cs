using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chara/CharaSpecs")]
public class Sc_Charaspec : ScriptableObject
{
    [Header("Interface")]
    public string characterName;
    public Sprite characterPortrait;

    [Header("Gameplay")]
    public int surveillanceLimit;
    public float speed;
    public CharacterSkills[] characterSkills;
   

}

[System.Serializable]
public enum CharacterSkills
{
    LockPicking, NotLockPicking, Hacker, NotHacker, Inteligent, NotInteligent, Quick, NotQuick, Charismatic, NotCharismatic
}
