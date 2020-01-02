using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ma_LevelManager : MonoBehaviour
{

    public static Ma_LevelManager instance;

    public Ma_ClockManager clock;
    

    //PathFinidingPart
    [HideInInspector] [SerializeField] public Tile[] allWalkableTile;
    [HideInInspector] [SerializeField] public Mb_Door[] allDoor;
    [HideInInspector] [SerializeField] public Mb_Escape escapeTrial;
    [HideInInspector] [SerializeField] public Tile[] allTiles;
   
    //ObjectivePart
    public Sc_LevelParameters levelBaseParameters;
    [HideInInspector] [SerializeField] List<TimeState> timeObjectives;
    [HideInInspector] [SerializeField] List<MoneyState> moneyObjectives;
    [HideInInspector] [SerializeField] List<ItemState> itemObjectives;
    [HideInInspector] public bool levelFinished;

    //clock affichage
    public float timeRemaining;
    private float interval;
    private int minuteRemaining;
    private int secondsRemaining;




    [HideInInspector] public float cashAmount = 0;

    public void Awake()
    {
        instance = this;
        Ma_ClockManager.instance.tickTrigger.AddListener(this.TimeShattering);
        interval = Ma_ClockManager.instance.tickInterval;
        timeRemaining = levelBaseParameters.timeAvaibleBeforePolice;
        clock = GetComponentInChildren<Ma_ClockManager>();


        for (int i = 0; i < levelBaseParameters.allObjectives.Length; i++)
            if (levelBaseParameters.allObjectives[i].GetType() == typeof(Sc_Objective_Item))
            {
                ItemState toAdd = new ItemState();
                toAdd.objectiveTocheck = levelBaseParameters.allObjectives[i] as Sc_Objective_Item;
                itemObjectives.Add(toAdd);
            }
            else if ((levelBaseParameters.allObjectives[i].GetType() == typeof(Sc_Objective_Money)))
            {
                MoneyState toAdd = new MoneyState();
                toAdd.objectiveTocheck = levelBaseParameters.allObjectives[i] as Sc_Objective_Money;
                moneyObjectives.Add(toAdd);
            }
            else if ((levelBaseParameters.allObjectives[i].GetType() == typeof(Sc_Objective_Time)))
            {
                TimeState toAdd = new TimeState();
                toAdd.objectiveTocheck = levelBaseParameters.allObjectives[i] as Sc_Objective_Time;
                timeObjectives.Add(toAdd);
            }
    }

    public float GetCashAmount()
    {
        return cashAmount;
    }

    public void AddCash(float cash)
    {
        cashAmount += cash;
        UIManager.instance.UpdateMoneyCounter();
    }

    public Tile GetWalkableTile(int row, int column)
    {
        Tile res = null;
        for(int i = 0; i < allWalkableTile.Length; i++)
        {
            if(allWalkableTile[i].column == column)
            {
                if(allWalkableTile[i].row == row)
                {
                    res = allWalkableTile[i];
                }
            }
        }
        return res;
    }

    public Tile GetTile(int row, int column)
    {
        Tile res = null;
        for (int i = 0; i < allTiles.Length; i++)
        {
            if (allTiles[i].column == column)
            {
                if (allTiles[i].row == row)
                {
                    res = allTiles[i];
                }
            }
        }
        return res;
    }

    void TimeShattering()
    {
        timeRemaining -= interval;
        //Setup du timing a afficher pour la clock
        minuteRemaining = Mathf.FloorToInt(timeRemaining / 60);
        secondsRemaining =Mathf.Clamp(  Mathf.RoundToInt(timeRemaining - minuteRemaining * 60), 0,59);
        string timeSpentToDisplay;
        if (secondsRemaining>10)
            timeSpentToDisplay = minuteRemaining + " : " + secondsRemaining;
        else
            timeSpentToDisplay = minuteRemaining + " : 0" + secondsRemaining;
        UIManager.instance.timeElpased.text = timeSpentToDisplay;
        UIManager.instance.vignetTimeElapse.fillAmount =
            timeRemaining / Ma_LevelManager.instance.levelBaseParameters.timeAvaibleBeforePolice;

        SetTileWeight();
        if (timeRemaining == 0)
            PoliceArrive();
    }

    //TO MODIFY
    void PoliceArrive()
    {
        Debug.Log("PoliceArrive");
    }

    void SetTileWeight()
    {
        foreach (Tile tileToSet in allWalkableTile )
        {
            if (tileToSet.avaible == false)
            {
                if (tileToSet.agentOnTile != null && tileToSet.agentOnTile.actionsToPerform.Count>0)
                    tileToSet.cost = tileToSet.agentOnTile.actionsToPerform[0].timeToPerform * Ma_ClockManager.instance.tickInterval;
                else
                {
                    tileToSet.cost = 300;
                   
                }
            }
            else
                tileToSet.cost = Ma_ClockManager.instance.tickInterval;
        }
        foreach (Mb_Door doorToSet in allDoor)
        { 
            foreach (Tile tileToSet in doorToSet.tileAssociated)
            {
                if (tileToSet.avaible == false)
                    tileToSet.cost = Ma_ClockManager.instance.tickInterval + (doorToSet.trialParameters.timeToAccomplishTrial - doorToSet.currentTimeSpentOn) * Ma_ClockManager.instance.tickInterval;
            }

        }
    }

    public void CheckMoneyObjectives(float totalCash)
    {
        for (int i =0; i< moneyObjectives.Count; i++)
        {
            if (totalCash >= moneyObjectives[i].objectiveTocheck.moneyToGet)
            {
                
                MoneyState newObjCompleted;
                newObjCompleted.objectiveTocheck = moneyObjectives[i].objectiveTocheck;
                newObjCompleted.isCompleted = true;
                moneyObjectives[i] = newObjCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
            else
            {
                MoneyState newObjNotCompleted;
                newObjNotCompleted.objectiveTocheck = moneyObjectives[i].objectiveTocheck;
                newObjNotCompleted.isCompleted = false;
                moneyObjectives[i] = newObjNotCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjNotCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
        }
       
    }

    public void CheckTimeObjective(float timeSpent)
    {
        for (int i = 0; i < timeObjectives.Count; i++)
        {
            if (timeSpent >= timeObjectives[i].objectiveTocheck.timeToDo)
            {
                TimeState newObjCompleted;
                newObjCompleted.objectiveTocheck = timeObjectives[i].objectiveTocheck;
                newObjCompleted.isCompleted = true;
                timeObjectives[i] = newObjCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
            else
            {
                TimeState newObjNotCompleted;
                newObjNotCompleted.objectiveTocheck = timeObjectives[i].objectiveTocheck;
                newObjNotCompleted.isCompleted = false;
                timeObjectives[i] = newObjNotCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjNotCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
        }
    }

    public void CheckItemObjective(Sc_Items itemGathered)
    {
        for (int i = 0; i < itemObjectives.Count; i++)
        {
            if (itemGathered == itemObjectives[i].objectiveTocheck.itemToSteal)
            {
                Debug.LogError("CHECKMONEYOBJECTIVE");
                ItemState newObjCompleted;
                newObjCompleted.objectiveTocheck = itemObjectives[i].objectiveTocheck;
                newObjCompleted.isCompleted = true;
                itemObjectives[i] = newObjCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        Debug.LogError(j);
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
            else
            {
                ItemState newObjNotCompleted;
                newObjNotCompleted.objectiveTocheck = itemObjectives[i].objectiveTocheck;
                newObjNotCompleted.isCompleted = false;
                itemObjectives[i] = newObjNotCompleted;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (newObjNotCompleted.objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
        }
    }
    public void CheckEscape()
    {
        bool haveAllEscaped = false;
        for (int i =0; i <Ma_PlayerManager.instance.playerList.Length; i++)
        {
            if (Ma_PlayerManager.instance.playerList[i].state != StateOfAction.Escaped || Ma_PlayerManager.instance.playerList[i].state != StateOfAction.Captured)
            {
                haveAllEscaped = false;
                break;
            }
            else
                haveAllEscaped = true;
        }

        if (haveAllEscaped == true)
            EndLevel();
    }

    public void EndLevel()
    {
        
    }

}

[System.Serializable]
public struct MoneyState
{
    public Sc_Objective_Money objectiveTocheck;
    public bool isCompleted;
}

[System.Serializable]
public struct ItemState
{
    public Sc_Objective_Item objectiveTocheck;
    public bool isCompleted;
}

[System.Serializable]
public struct TimeState
{
    public Sc_Objective_Time objectiveTocheck;
    public bool isCompleted;
}
