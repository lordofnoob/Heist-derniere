using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mb_Chest : Mb_Trial
{
    public List<Sc_Items> itemList;

    public override void DoThings()
    {
        listOfUser[0].AddItem(itemList[0]);
        if(itemList[0] is Sc_Money)
        {
            Sc_Money cash = itemList[0] as Sc_Money;
            Ma_LevelManager.instance.AddCash(cash.valueOfTheItem);
            print(Ma_LevelManager.instance.cashAmount);
            Ma_LevelManager.instance.CheckMoneyObjectives(Ma_LevelManager.instance.cashAmount);
        }
        print(itemList[0]);
        Ma_LevelManager.instance.CheckItemObjective(itemList[0]);
        itemList.Remove(itemList[0]);
        foreach (Mb_Player playerUiToUpdate in listOfUser)
        {
            UIManager.instance.UpdateSpecificUI(playerUiToUpdate);
        }
        base.DoThings();
    }
}
