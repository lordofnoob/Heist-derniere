using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mb_UiPlayerCharacter : MonoBehaviour
{
    [SerializeField] Sc_SkillsIconsInfo iconRuling;
    public TextMeshProUGUI playerName;
    public Image[] skillsIconSlots;
    public Image[] itemIconSlots;

    public void UpdateBasicUI(Mb_Player player)
    {
        playerName.text = player.charaPerks.name;
        for (int i = 0; i < player.charaPerks.characterSkills.Length; i++)
        {
            foreach (SkillInfos skillToCheck in iconRuling.allSkillsIcon)
            {
                if (skillToCheck.skillAssociated == player.charaPerks.characterSkills[i])
                    itemIconSlots[i].sprite = skillToCheck.skillIcon;
            }               
        }
    }


    public void UpdateItemUi(Mb_Player player)
    {
        foreach (Image slot in itemIconSlots)
            slot.sprite = null;

        for (int i = 0; i < player.itemsHold.Count; i++)
        {
            itemIconSlots[i].sprite = player.itemsHold[i].itemIcon;
        }
    }
}
