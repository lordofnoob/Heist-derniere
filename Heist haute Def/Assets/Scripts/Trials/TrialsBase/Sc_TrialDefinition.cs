using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Trial/TrialsDefinition")]
public class Sc_TrialDefinition : ScriptableObject
{
    public AssociatedSkillValues[] skillToUse;
    public float timeToAccomplishTrial;
}

[System.Serializable]
public struct AssociatedSkillValues
{
    public CharacterSkills associatedSkill;
    public float associatedReduction;
}