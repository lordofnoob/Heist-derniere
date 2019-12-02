using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ma_LevelManager : MonoBehaviour
{

    public static Ma_LevelManager Instance;

    public Ma_ClockManager clock;
    

    [SerializeField]public Tile[] allWalkableTile;
    [SerializeField]public Tile[] allExitTile;
    [SerializeField] public Tile[] allTiles;
    [SerializeField] Sc_LevelParameters levelBaseParameters;
    public float timeRemaining;
    private float interval;
    private int minuteRemaining;
    private int secondsRemaining;

    public void Awake()
    {
        Instance = this;
        Ma_ClockManager.Instance.tickTrigger.AddListener(this.TimeShattering);
        interval = Ma_ClockManager.Instance.tickInterval;
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
        UIManager.Instance.timeElpased.text = timeSpentToDisplay;

        if (timeRemaining == 0)
            PoliceArrive();
    }

    void PoliceArrive()
    {
        Debug.Log("PoliceArrive");
    }
}
