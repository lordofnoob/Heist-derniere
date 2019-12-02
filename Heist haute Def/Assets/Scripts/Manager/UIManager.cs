using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("AllUiSlots")]
    public  Text timeElpased;

    public List<Mb_HostageStockArea> hostageStockArea = new List<Mb_HostageStockArea>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
        UpdateHostageStressBar();
        UpdateHostageStockAreaStressBar();
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

    }

}
