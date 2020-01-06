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
        //Mb_player
        #region
        if (agent is Mb_Player)
        {
            Mb_Player player = agent as Mb_Player;

            //checker si il s est bien déplacé sur la case
            if (!destination.avaible && destination == player.destination)
            {
                player.nextAction = true;
                return;
            }
            //bool findNewPath = false;
            bool findNewPath = true;

            /*
            //IF PLAYER PATH HAS CHANGE DURING DEPLACEMENT
            foreach (Action action in player.actionsToPerform)
            {
                if (action is Deplacement)
                {
                    Deplacement deplacement = action as Deplacement;
                    if (!deplacement.destination.avaible)
                    {
                        findNewPath = true;
                        break;
                    }
                }
            }
            */
            //Debug.Log(destination.agentOnTile is Mb_IAAgent);
            if (destination.avaible || destination.agentOnTile is Mb_IAAgent)
            {
                player.SetNewActionState(StateOfAction.Moving);

                //CHECK FOR SWITCH POSITION WITH HOSTAGE

                //Debug.Log(destination.agentOnTile);
                if (destination.agentOnTile is Mb_IAAgent)
                {
                    Debug.Log("Switch");
                    destination.agentOnTile.transform.DOLookAt(player.GetAgentTile().transform.position, 0.2f, AxisConstraint.Y);
                    destination.agentOnTile.transform.DOMove(new Vector3(player.GetAgentTile().transform.position.x,
                                                                            player.GetAgentTile().transform.position.y + player.GetAgentTile().transform.localScale.y / 2,
                                                                            player.GetAgentTile().transform.position.z),
                                                                            Ma_ClockManager.instance.tickInterval * timeToPerform)
                                                        .SetEase(Ease.Linear);
                    player.SetAgentTile(destination, true);
                }
                else
                {
                    player.SetAgentTile(destination);
                }

                //Debug.Log("MOVE TO : "+ destination.transform.position);
                player.transform.DOLookAt(destination.transform.position,0.2f, AxisConstraint.Y);
                player.transform.DOMove(new Vector3(destination.transform.position.x,
                                                    destination.transform.position.y + destination.transform.localScale.y/2,
                                                    destination.transform.position.z),
                                                    Ma_ClockManager.instance.tickInterval * timeToPerform)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    //destination.SetOutlinesEnabled(false);
                                    destination.highlighted = false;

                                    if (destination == player.destination)
                                    {
                                        player.SetNewActionState(StateOfAction.Idle);
                                        return;
                                    }

                                    if (!findNewPath)
                                        player.nextAction = true;
                                });                
            }
            else
            {
                findNewPath = true;
            }

            //RECALCULER SI  CEST PAS BIEN 
            if (findNewPath)
            {
                player.FindAnOtherPath();
            }
        }
        #endregion
        //Mb_AI
        #region
        else if (agent is Mb_IAAgent)
        {
            Mb_IAAgent hostage = agent as Mb_IAAgent;

            //OLD
            /*
            bool findNewPath = false;
            //IF HOSTAGE PATH HAS CHANGE DURING DEPLACEMENT
            foreach (Action action in hostage.actionsToPerform)
            {
                if (action is Deplacement)
                {
                    Deplacement deplacement = action as Deplacement;
                    //Debug.Log(deplacement.destination.avaible);

                    if (!deplacement.destination.avaible && deplacement.destination.GetComponentInChildren<Mb_Door>() == null)
                    {
                        findNewPath = true;
                        break;
                    }
                }
            }
            */

            if (destination.avaible)
            {
                hostage.SetNewActionState(StateOfAction.Moving);
                hostage.SetAgentTile(destination);

                //Debug.Log("MOVE TO : "+ destination.transform.position);
                hostage.transform.DOLookAt(destination.transform.position, 0.2f, AxisConstraint.Y);
                hostage.transform.DOMove(new Vector3(destination.transform.position.x, 
                                                     destination.transform.position.y + destination.transform.localScale.y / 2,
                                                     destination.transform.position.z),
                                                     Ma_ClockManager.instance.tickInterval * timeToPerform)
                                 .SetEase(Ease.Linear)
                                 .OnComplete(() =>
                                 {
                                     //destination.SetOutlinesEnabled(false);
                                     destination.highlighted = false;

                                     if (hostage.GetAgentTile() == hostage.destination)
                                     {
                                         hostage.SetNewActionState(StateOfAction.Idle);
                                         destination = null;
                                     }

                                     hostage.UpdatePositionToGo();
                                     hostage.nextAction = true;
                                 });
            }
            else if(destination.GetComponentInChildren<Mb_Door>() != null)
            {
                //Debug.Log("Set interaction");
                Mb_Trial trial = destination.GetComponentInChildren<Mb_Trial>();
                hostage.onGoingInteraction = trial;
                hostage.SetFirstActionToPerform(new Interact(trial.trialParameters.timeToAccomplishTrial, hostage, trial));

                hostage.nextAction = true;
            }

        }
        #endregion
    }
}
