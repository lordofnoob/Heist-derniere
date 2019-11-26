using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("AllUiSlots")]
    public  Text timeElpased;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateHostageStressBar();
    }

    void UpdateHostageStressBar()
    {
        foreach(Mb_IAAgent hostage in Ma_IAManager.Instance.IAList)
        {
            hostage.stressBar.fillAmount = hostage.stress/100;
        }
    }
}
