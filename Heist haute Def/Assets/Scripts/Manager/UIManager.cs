using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("TimeElpase")]
    public TextMeshProUGUI timeElpased;
    public Image vignetTimeElapse;
    [Header("AllUiSlots")]
    public Mb_UiPlayerCharacter[] allPlayerCards;
    public List<Mb_HostageStockArea> hostageStockArea = new List<Mb_HostageStockArea>();
    public TextMeshProUGUI cashAmountText;
    public UI_ObjectiveSetup objectiveSpot;
    public Image[] validatedSpot;
    public Image[] notValidatedSpot;
    [SerializeField] GameObject endCanvas;

    void Awake()
    {
        instance = this;
        SetupObjective();

        for(int i=0; i<allPlayerCards.Length; i++ )
        {
            allPlayerCards[i].UpdateBasicUI(Ma_LevelManager.instance.levelBaseParameters.allPlayerDescription[i]);
        }
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
        cashAmountText.text = Ma_LevelManager.instance.GetCashAmount().ToString() + " $";
    }

    public void UpdateSpecificUI(Mb_Player player)
    {
        for (int i =0; i < Ma_PlayerManager.instance.playerList.Length; i++)
        {
            if (player == Ma_PlayerManager.instance.playerList[i])
            {
                allPlayerCards[i].UpdateItemUi(player);
                break;
            }
        }
      
    }

     public void SetupObjective()
    {
        for (int i=0; i <Ma_LevelManager.instance.levelBaseParameters.allObjectives.Length; i++)
        {
            objectiveSpot.objectiveSpots[i].objectiveDescription.text = 
                Ma_LevelManager.instance.levelBaseParameters.allObjectives[i].objectifDescription;
        }
    }

    public void CheckObjectiveUI(int objectiveNumber, bool state)
    {
        if (state == true)
        {
            validatedSpot[objectiveNumber].gameObject.SetActive(true);
            notValidatedSpot[objectiveNumber].gameObject.SetActive(false);
        }
        else
        {
            validatedSpot[objectiveNumber].gameObject.SetActive(false);
            notValidatedSpot[objectiveNumber].gameObject.SetActive(true);
        }
    }

    void EndCanvas()
    {
        //METTRE ICI LES FONCTIONS D ANIMS POUR LE CANVAS DE FIN/ ON LE METS SUR LES COMPOSANTS DE L UI FINAL
        endCanvas.SetActive(true);
        Ma_ClockManager.instance.PauseGame();
    }
}
