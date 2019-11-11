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
        if(agent is Mb_Player)
        {
            Mb_Player player = agent as Mb_Player;
            if (!destination.avaible && destination == player.destination)
            {
                player.nextAction = true;
                return;
            }
            bool findNewPath = false;

            //IF PLAYER PATH HAS CHANGE DURING DEPLACEMENT
            foreach (Action action in player.actionsToPerform)
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
                player.agentTile.avaible = true;

                //Debug.Log("MOVE TO : "+ destination.transform.position);
                player.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f,
                                                    destination.transform.position.z),
                                                    Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    player.agentTile = destination;
                                    destination.SetOutlinesEnabled(false);
                                    destination.highlighted = false;

                                    if (destination == player.destination)
                                    {
                                        player.state = StateOfAction.Idle;
                                        player.destination = null;
                                    }
                                    if (!findNewPath)
                                        player.nextAction = true;
                                });

                if(player.capturedHostages.Count != 0)
                {
                    for (int i = player.capturedHostages.Count - 1; i >= 0; i--)
                    {
                        if (i == 0)
                        {
                            //Debug.Log("FIRST HOSTAGE");
                            player.capturedHostages[i].transform.DOMove(new Vector3(player.agentTile.transform.position.x, 0.5f,
                                                                                    player.agentTile.transform.position.z),
                                                                                    Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                                                .SetEase(Ease.Linear);
                            player.capturedHostages[i].agentTile = player.agentTile;
                        }
                        else
                        {
                            //Debug.Log("OTHER HOSTAGE");
                            player.capturedHostages[i].transform.DOMove(new Vector3(player.capturedHostages[i - 1].agentTile.transform.position.x, 0.5f,
                                                        player.capturedHostages[i - 1].agentTile.transform.position.z),
                                                        Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                                                .SetEase(Ease.Linear);
                            player.capturedHostages[i].agentTile = player.capturedHostages[i - 1].agentTile;
                        }
                    }
                }
            }
            else
            {
                findNewPath = true;
            }

            if (findNewPath)
            {
                Debug.Log("FIND ANOTHER PATH");
                player.FindAnOtherPath();
            }
        }
        else if(agent is Mb_IAHostage)
        {
            Mb_IAHostage hostage = agent as Mb_IAHostage;

            if (!destination.avaible && destination == hostage.destination)
            {
                hostage.nextAction = true;
                return;
            }
            bool findNewPath = false;

            //IF PLAYER PATH HAS CHANGE DURING DEPLACEMENT
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

            if (destination.avaible)
            {
                destination.avaible = false;
                hostage.agentTile.avaible = true;

                //Debug.Log("MOVE TO : "+ destination.transform.position);
                hostage.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f,
                                                     destination.transform.position.z),
                                                     Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                 .SetEase(Ease.Linear)
                                 .OnComplete(() =>
                                 {
                                     hostage.agentTile = destination;
                                     destination.SetOutlinesEnabled(false);
                                     destination.highlighted = false;

                                     if (destination == hostage.destination)
                                     {
                                         hostage.state = StateOfAction.Idle;
                                         hostage.destination = null;
                                     }

                                     hostage.UpdatePositionToGo();

                                     if (!findNewPath)
                                         hostage.nextAction = true;
                                 });

            }
            else if(destination.GetComponentInChildren<Mb_Door>() != null)
            {
                //Debug.Log("Set interaction");
                hostage.SetFirstInteraction(destination.GetComponentInChildren<Mb_Trial>());
            }
            else
            {
                findNewPath = true;
            }

            if (findNewPath)
            {
                Debug.Log("FIND ANOTHER PATH");
                hostage.FindAnOtherPath();
            }
        }

    }
}
