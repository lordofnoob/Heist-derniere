using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class In_Chara_Interface : MonoBehaviour
{
    [Header("Cosmetic")]
    [SerializeField] TextMeshProUGUI charaName;
    [SerializeField] Image charaFace;
    [SerializeField] TextMeshProUGUI charaOccupation;

    [Header("Life")]
    [SerializeField] TextMeshProUGUI charaState;
    [SerializeField] Animator charaStateAnim;

    [Header("Skills")]
    [SerializeField] Image[] oneSkillSlot;
    [SerializeField] Image[] twoSkillSlot;
    [SerializeField] Image[] threeSkillSlot;
    [SerializeField] Image[] fourSkillSlot;

    [Header("Equipment")]
    [SerializeField] Image[] itemSlot;

}
