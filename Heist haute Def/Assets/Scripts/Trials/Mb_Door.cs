using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Door : Mb_Trial
{
    public Animation door;
    public Tile tileAssociated;
    public bool close = true;
    public bool isExitDoor = false;

    public override void DoThings()
    {
        if (close)
        {
            tileAssociated.Cost = 1;
        }
        else
        {
            tileAssociated.Cost = 3;
        }

        close = !close;
        GetComponentInChildren<MeshRenderer>().enabled = close;
        tileAssociated.avaible = !tileAssociated.avaible;

        //   door.
        //door.Play();

        foreach (Mb_Agent agent in listOfUser)
        {
            if(agent is Mb_IAHostage)
            {
                List<Tile> recalculatedPath = agent.pathfinder.SearchForShortestPath(agent.agentTile, new List<Tile> { agent.destination }, true);
                agent.AddDeplacement(recalculatedPath);
            }
        }

        ResetValues();
    }        
}
