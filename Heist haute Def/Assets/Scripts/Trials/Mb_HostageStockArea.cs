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
        IAManager.Instance.StockHostagesInArea(this, listOfUser[0].capturedHostages);
        ResetValues();
    }
}
