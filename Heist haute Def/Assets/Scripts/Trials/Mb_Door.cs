﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Door : Mb_Trial
{
    public Animation door;
    public Tile tileAssociated;
    public bool close = true;

    public override void DoThings()
    {
        if (close)
        {
            tileAssociated.cost = 1;
        }
        else
        {
            tileAssociated.cost = base.trialParameters.timeToAccomplishTrial * Ma_ClockManager.Instance.tickInterval;
        }

        close = !close;
        GetComponentInChildren<MeshRenderer>().enabled = close;
        tileAssociated.avaible = !tileAssociated.avaible;

        //   door.
        //door.Play();

        foreach (Mb_Agent agent in listOfUser)
        {
            if(agent is Mb_IAAgent)
            {
                List<Tile> recalculatedPath = agent.pathfinder.SearchForShortestPath(agent.AgentTile, new List<Tile> { agent.destination }, true);
                agent.AddDeplacement(recalculatedPath);
            }
        }

        ResetValues();
    }        
}
