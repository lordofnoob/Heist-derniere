using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewIconCode", menuName = "NewIconCode")]
public class Sc_SkillsIconsInfo : ScriptableObject
{
    public SkillInfos[] allSkillsIcon;
}

[System.Serializable]
public struct SkillInfos
{
    public CharacterSkills skillAssociated;
    public Sprite skillIcon;
}
