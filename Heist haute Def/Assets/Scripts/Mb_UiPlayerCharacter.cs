using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mb_UiPlayerCharacter : MonoBehaviour
{
    public Image[] skillsIconSlots;
    public Image[] itemIconSlots;

    public void UpdateUi(Mb_Player player)
    {
        

        foreach (Image slot in itemIconSlots)
            slot.sprite = null;

        for (int i = 0; i < player.itemsHold.Count; i++)
        {
            itemIconSlots[i].sprite = player.itemsHold[i].itemIcon;
        }
    }
}
