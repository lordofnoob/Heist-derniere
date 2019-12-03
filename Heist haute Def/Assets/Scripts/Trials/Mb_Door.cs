using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Door : Mb_Trial
{
    public Animator doorAnim;
    public Tile tileAssociated;
    public bool close = true;

    public override void DoThings()
    { 
        /*
        if (close)
        {
            tileAssociated.cost = 1;
        }
        else
        {
            tileAssociated.cost = base.trialParameters.timeToAccomplishTrial * Ma_ClockManager.instance.tickInterval;
        }*/

        close = !close;
        doorAnim.SetTrigger("DoThings");
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
        base.DoThings();
    }        
}
