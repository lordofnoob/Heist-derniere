using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ma_LevelManager : MonoBehaviour
{

    public static Ma_LevelManager instance;

    public Ma_ClockManager clock;
    


    [SerializeField] public Tile[] allWalkableTile;
    [SerializeField] public Mb_Door[] allDoor;
    [SerializeField] public Mb_Escape escapeTrial;
    [SerializeField] public Tile[] allTiles;
    [SerializeField] Sc_LevelParameters levelBaseParameters;
    public float timeRemaining;
    private float interval;
    private int minuteRemaining;
    private int secondsRemaining;

    public float cashAmount = 0;

    public void Awake()
    {
        instance = this;
        Ma_ClockManager.instance.tickTrigger.AddListener(this.TimeShattering);
        interval = Ma_ClockManager.instance.tickInterval;
        timeRemaining = levelBaseParameters.timeAvaibleBeforePolice;
        clock = GetComponentInChildren<Ma_ClockManager>();

       
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
        secondsRemaining = Mathf.RoundToInt(timeRemaining - minuteRemaining * 60);
        string timeSpentToDisplay;
        if (secondsRemaining>10)
            timeSpentToDisplay = minuteRemaining + " : " + secondsRemaining;
        else
            timeSpentToDisplay = minuteRemaining + " : 0" + secondsRemaining;
        UIManager.instance.timeElpased.text = timeSpentToDisplay;

        SetTileWeight();
        if (timeRemaining == 0)
            PoliceArrive();
    }

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
                if (tileToSet.avaible==false)
                    tileToSet.cost = Ma_ClockManager.instance.tickInterval + (doorToSet.trialParameters.timeToAccomplishTrial - doorToSet.currentTimeSpentOn) * Ma_ClockManager.instance.tickInterval;

        }
    }
}
