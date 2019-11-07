using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SkillsIconsInfo : MonoBehaviour
{
    public SkillInfos[] allSkillsIcon;
}

public struct SkillInfos
{
    public CharacterSkills skillAssociated;
    public Sprite skillIcon;
}
