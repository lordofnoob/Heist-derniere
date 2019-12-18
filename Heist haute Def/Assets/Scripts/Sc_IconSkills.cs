using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "UI/IconSkill")]
public class Sc_IconSkills : MonoBehaviour
{
    public IconAssociation[] iconRule;
}
public struct IconAssociation
{
    public CharacterSkills skillToIcon;
    public Sprite iconAssociated; 
}
