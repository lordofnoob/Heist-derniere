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
            Ma_LevelManager.instance.cashAmount += cash.valueOfTheItem;
        }
        itemList.Remove(itemList[0]);
        base.DoThings();
    }
}
