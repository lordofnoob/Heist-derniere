using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mb_IAPoliceman : Mb_Agent
{
    [Header("Policeman Infos")]
    public float speed;
    public Mb_PlayerTrial targetPlayer;

    public override void SetAgentTile(Tile newAgentTile, bool isSwitchingTile = false)
    {
        base.SetAgentTile(newAgentTile, isSwitchingTile);
    }

    public override void AddDeplacement(List<Tile> path)
    {
        foreach(Tile tile in path)
        {
            actionsToPerform.Add(new Deplacement(speed, this, tile));
        }
    }

    public override void PerformAction()
    {
        if(targetPlayer != null)
        {
            GoTo(targetPlayer);
        }

        if(actionsToPerform.Count != 0 && nextAction)
        {
            if (actionsToPerform.First() is Deplacement)
            {
                Deplacement depla = actionsToPerform.First() as Deplacement;
                if (depla.destination.cost > Ma_ClockManager.instance.tickInterval)
                {
                    if (depla.destination.agentOnTile != null && depla.destination.agentOnTile.GetActionState() == StateOfAction.Moving)
                    {
                        Debug.Log("WAIT");
                        List<Action> temp = actionsToPerform;
                        actionsToPerform.Clear();
                        actionsToPerform.Add(new Wait(1, this));
                        actionsToPerform.AddRange(temp);
                    }
                }
            }

            nextAction = false;
            actionsToPerform.First().PerformAction();
            actionsToPerform.Remove(actionsToPerform.First());
        }
    }

    public override void FindAnOtherPath()
    {
        //Debug.Log("IA Find a new path");

        if (onGoingInteraction)
        {
            GoTo(onGoingInteraction);
        }
        else if (destination != null)
        {
            GoTo(destination);
        }

        nextAction = true;
    }

    public void SetTargetPlayer(Mb_PlayerTrial player)
    {
        targetPlayer = player;
    }

    public override void Interact()
    {
        SetNewActionState(StateOfAction.Interacting);

        if(onGoingInteraction.listOfUser.Count == 0)
        {
            onGoingInteraction.listOfUser.Add(this);
            onGoingInteraction.StartInteracting();
        }
        else
        {
            for (int i = 0; i < onGoingInteraction.listOfUser.Count; i++)
            {
                if (onGoingInteraction.listOfUser[i] != this)
                {
                    onGoingInteraction.listOfUser.Add(this);
                    onGoingInteraction.ReUpduateTiming();
                }
            }
        }
        base.Interact();
    }

    public override void SetNextInteraction()
    {
        if (trialsToGo.Count > 0)
        {
            onGoingInteraction = trialsToGo.First();
            actionsToPerform.Add(new Interact(onGoingInteraction.trialParameters.timeToAccomplishTrial, this, onGoingInteraction));
        }
    }

    public override void SetFirstActionToPerform(Action action)
    {
        List<Action> temp = actionsToPerform;
        actionsToPerform.Clear();
        actionsToPerform.Add(action);
        actionsToPerform.AddRange(temp);
        //Debug.Log(actionsToPerform[0]);
    }

    public override void ResetInteractionParameters()
    {
        SetNewActionState(StateOfAction.Idle);
        onGoingInteraction = null;
        nextAction = true;
        //Debug.Log(actionsToPerform.Count);
    }
    public override void SetNewActionState(StateOfAction agentState)
    {
        //Debug.Log(agentState);
        base.SetNewActionState(agentState);

        if (agentState == StateOfAction.Moving)
        {
            animator.SetBool("Idle00_To_Move", true);
            animator.SetFloat("Speed", 8.5f);
        }
        else if (agentState == StateOfAction.Idle)
        {
            animator.SetBool("Idle00_To_Move", false);
            animator.SetFloat("Speed", 0);
        }
    }
}
