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
    [HideInInspector] public List<TimeState> timeObjectives;
    [HideInInspector] public List<MoneyState> moneyObjectives;
    [HideInInspector] public List<ItemState> itemObjectives;
    public List<ObjectiveState> allObjectiveState;
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

                moneyObjectives[i].isCompleted = true;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (moneyObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
            else
            {
                moneyObjectives[i].isCompleted = false;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (moneyObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
        }
        UpdateObjectiveListeState();


    }

    public void CheckTimeObjective(float timeSpent)
    {
    
        for (int i = 0; i < timeObjectives.Count; i++)
        {
            if (timeSpent >= timeObjectives[i].objectiveTocheck.timeToDo)
            {
                
                timeObjectives[i].isCompleted = false;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (timeObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
            else
            {
                timeObjectives[i].isCompleted = true;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (timeObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
        }
        UpdateObjectiveListeState();
    }

    public void CheckItemObjective(Sc_Items itemGathered)
    {
        for (int i = 0; i < itemObjectives.Count; i++)
        {
            if (itemGathered == itemObjectives[i].objectiveTocheck.itemToSteal)
            {
                
                itemObjectives[i].isCompleted = true;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (itemObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        Debug.LogError(j);
                        UIManager.instance.CheckObjectiveUI(j, true);
                    }
                }
            }
            else
            {
                itemObjectives[i].isCompleted = false;
                for (int j = 0; j < levelBaseParameters.allObjectives.Length; j++)
                {
                    if (itemObjectives[i].objectiveTocheck == levelBaseParameters.allObjectives[j])
                    {
                        UIManager.instance.CheckObjectiveUI(j, false);
                    }
                }
            }
        }
        UpdateObjectiveListeState();
    }

    void UpdateObjectiveListeState()
    {
        allObjectiveState.Clear();
        allObjectiveState.AddRange(timeObjectives);
      
        allObjectiveState.AddRange(moneyObjectives);
      
        allObjectiveState.AddRange(itemObjectives);
        for (int i = 0; i < timeObjectives.Count; i++)
        {
            allObjectiveState[i].objectifDescription = timeObjectives[i].objectiveTocheck.objectifDescription;
        }
        for (int i = timeObjectives.Count; i < timeObjectives.Count + moneyObjectives.Count; i++)
        {
            allObjectiveState[i].objectifDescription = moneyObjectives[i- timeObjectives.Count].objectiveTocheck.objectifDescription;
        }
        for (int i = timeObjectives.Count + moneyObjectives.Count; i < itemObjectives.Count+ timeObjectives.Count + moneyObjectives.Count; i++)
        {
            allObjectiveState[i].objectifDescription = itemObjectives[i - timeObjectives.Count - moneyObjectives.Count].objectiveTocheck.objectifDescription;
        }
    }

    public void CheckEscape()
    {
        bool haveAllEscaped = false;
        for (int i =0; i <Ma_PlayerManager.instance.playerList.Length; i++)
        {
            if (Ma_PlayerManager.instance.playerList[i].state == StateOfAction.Escaped || Ma_PlayerManager.instance.playerList[i].state == StateOfAction.Captured)
            {
                Debug.Log("yes");
                haveAllEscaped = true;
                
            }
            else
            {
                Debug.Log("no");
                haveAllEscaped = false;
                break;
            }
        }
        Debug.Log(haveAllEscaped);
        if (haveAllEscaped == true)
            EndLevel();
    }

    public void CheckEndLevel()
    {
        bool asAlreadyAllEscaped = false;
        for (int i = 0; i < Ma_PlayerManager.instance.playerList.Length; i++)
        {
            if (Ma_PlayerManager.instance.playerList[i].state == StateOfAction.Captured | Ma_PlayerManager.instance.playerList[i].state == StateOfAction.Escaped)
            {
                asAlreadyAllEscaped = true;
            }
            else
            {
                asAlreadyAllEscaped = false;
                break;
            }
        }

        if (asAlreadyAllEscaped == true)
        {
            Ma_ClockManager.instance.PauseGame();
            UIManager.instance.EndCanvas();
        }
    }


    public void EndLevel()
    {
        UIManager.instance.EndCanvas();
    }

}

public class ObjectiveState : Sc_Objective
{
    public bool isCompleted;
}

public class MoneyState : ObjectiveState
{
    public Sc_Objective_Money objectiveTocheck;
}

public class ItemState : ObjectiveState
{
    public Sc_Objective_Item objectiveTocheck;
}


public class TimeState : ObjectiveState
{
    public Sc_Objective_Time objectiveTocheck;
}
