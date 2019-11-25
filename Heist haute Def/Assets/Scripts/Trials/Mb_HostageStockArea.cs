using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_HostageStockArea : Mb_Trial
{

    private int areaCapacity = 0;
    //Dictionary des slots : true = free
    public Tile[] hostagesPos;



    public override void DoThings()
    {
        foreach (Mb_Player player in listOfUser)
        {
            Ma_IAManager.Instance.StockHostagesInArea(this, player.capturedHostages);
        }


        ResetValues();
    }
}
