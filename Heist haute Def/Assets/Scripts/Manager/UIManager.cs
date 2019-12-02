using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("AllUiSlots")]
    public  Text timeElpased;

    public List<Mb_HostageStockArea> hostageStockArea = new List<Mb_HostageStockArea>();
    public TextMeshProUGUI cashAmountText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
        UpdateHostageStressBar();
        UpdateHostageStockAreaStressBar();
        UpdateMoneyCounter();
    }

    void UpdateHostageStressBar()
    {
        foreach(Mb_IAAgent hostage in Ma_IAManager.Instance.IAList)
        {
            hostage.stressBar.fillAmount = hostage.stress / 100;
        }
    }

    void UpdateHostageStockAreaStressBar()
    {
        foreach(Mb_HostageStockArea hostageArea in hostageStockArea)
        {
            hostageArea.stressBar.fillAmount = hostageArea.areaGlobalStress / 100;
        }
    }

    public void UpdateMoneyCounter()
    {
        cashAmountText.text = Ma_LevelManager.Instance.cashAmount.ToString() + " $";
    }

}
