using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mb_HostageStockArea : Mb_Trial
{

    private int areaCapacity = 0;
    public Tile[] hostagesPos;
    public List<Mb_IAAgent> stockedHostages = new List<Mb_IAAgent>();
    public Image stressBar;
    public TextMeshProUGUI hostageNumeberText;
    public float areaGlobalStress;
    private int stockedHostageNumber;

  public override void Awake()
       {
        base.Awake();
        Ma_ClockManager.instance.tickTrigger.AddListener(IncreaseAreaGlobalStress);
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
        hostageNumeberText.text = number.ToString();
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
            foreach (Mb_IAAgent hostage in stockedHostages)
            {
                areaStressPercentage += hostage.stress;
            }
            areaGlobalStress = areaStressPercentage / stockedHostageNumber;
        }
    }
}
