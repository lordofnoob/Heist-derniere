using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mb_HostageStockArea : Mb_Trial
{

    private int areaCapacity = 0;
    public Tile[] hostagesPos;
    public Image stressBar;
    public float areaGlobalStress;
    private int stockedHostageNumber;

    private void Awake()
    {
        base.Awake();
        Ma_ClockManager.Instance.tickTrigger.AddListener(IncreaseAreaGlobalStress);
    }

    private void Start()
    {
        UIManager.Instance.hostageStockArea.Add(this);
        SetStockedHostageNumber(0);
    }

    public void SetStockedHostageNumber(int number)
    {
        if(number == 0)
        {
            stressBar.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            stressBar.transform.parent.gameObject.SetActive(true);
        }
        stockedHostageNumber = number;
    }

    public int GetStockedHostageNumber()
    {
        return stockedHostageNumber;
    }

    public override void DoThings()
    {
        foreach (Mb_Player player in listOfUser)
        {
            Ma_IAManager.Instance.StockHostagesInArea(this, player.capturedHostages);
        }

        ResetValues();
    }

    public void IncreaseAreaGlobalStress()
    {
        if(stockedHostageNumber > 0)
        {
            float areaStressPercentage = 0;
            foreach (Tile tile in hostagesPos)
            {
                if (tile.agentOnTile is Mb_IAAgent)
                {
                    Mb_IAAgent hostage = tile.agentOnTile as Mb_IAAgent;
                    areaStressPercentage += hostage.stress;
                }
            }
            areaGlobalStress = areaStressPercentage / stockedHostageNumber;
        }
    }
}
