using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_HostageStockArea : Mb_Trial
{

    private int areaCapacity = 0;
    //Dictionary des slots : true = free
    public Transform[] hostagesPos;

    // Update is called once per frame
    void Update()
    {
        Counting();
    }

    public override void DoThings()
    {
        Mb_Player player = listOfUser[0] as Mb_Player;
        Ma_IAManager.Instance.StockHostagesInArea(this, player.capturedHostages);
        ResetValues();
    }
}
