using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mb_UiPlayerCharacter : MonoBehaviour
{
    [SerializeField] Sc_IconSkills iconRuling;
    public TextMeshPro playerName;
    public Image[] skillsIconSlots;
    public Image[] itemIconSlots;

    public void UpdateBasicUI(Mb_Player player)
    {
        playerName.text = player.charaPerks.name;
        for (int i = 0; i < player.charaPerks.characterSkills.Length; i++)
        {
            foreach (IconAssociation skillToCheck in iconRuling.iconRule)
            {
                if (skillToCheck.skillToIcon == player.charaPerks.characterSkills[i])
                    itemIconSlots[i].sprite = skillToCheck.iconAssociated;
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
