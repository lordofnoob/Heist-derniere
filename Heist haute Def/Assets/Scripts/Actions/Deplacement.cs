using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : Action
{
    public Tile destination;

    public Deplacement(float timeToPerform, Mb_Agent toMove, Tile destination) : base(timeToPerform, toMove)
    {
        this.destination = destination;
    }

    public override void PerformAction()
    {
        agent = agent as Mb_Player;
        if(!destination.avaible && destination == agent.destination)
        {
            agent.nextAction = true;
            return;
        }
        bool findNewPath = false;

        //IF PLAYER PATH HAS CHANGE DURING DEPLACEMENT
        foreach (Action action in agent.actionsToPerform)
        {
            if (action is Deplacement)
            {
                Deplacement deplacement = action as Deplacement;
                //Debug.Log(deplacement.destination.avaible);

                if (!deplacement.destination.avaible)
                {
                    findNewPath = true;
                    break;
                }
            }
        }

        if (destination.avaible)
        {
            destination.avaible = false;
            agent.agentTile.avaible = true;
            agent.agentTile = destination;

            //Debug.Log("MOVE TO : "+ destination.transform.position);
            agent.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f, destination.transform.position.z), Ma_LevelManager.Instance.clock.tickInterval * timeToPerform).SetEase(Ease.Linear).OnComplete(() => {
                destination.SetOutlinesEnabled(false);
                destination.highlighted = false;

                if (destination == agent.destination)
                {
                    agent.state = StateOfAction.Idle;
                    agent.destination = null;
                }

                if(!findNewPath)
                    agent.nextAction = true;
            });
        }
        else
        {
            findNewPath = true;
        }

        if (findNewPath)
        {
            Debug.Log("FIND ANOTHER PATH");
            agent.FindAnOtherPath();
        }

    }
}
