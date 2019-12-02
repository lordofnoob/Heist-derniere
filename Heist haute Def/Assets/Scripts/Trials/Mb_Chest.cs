using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mb_Chest : Mb_Trial
{
    public List<Sc_Items> itemList;

    public override void DoThings()
    {
        listOfUser[0].AddItem(itemList[0]);
        itemList.Remove(itemList[0]);
        base.DoThings();
    }
}
