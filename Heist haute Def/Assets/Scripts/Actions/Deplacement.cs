﻿using DG.Tweening;
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
        agent.state = StateOfAction.Moving;

        if (agent is Mb_Player)
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
                //Debug.Log("MOVE TO : "+ destination.transform.position);
                player.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f,
                                                    destination.transform.position.z),
                                                    Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    destination.SetOutlinesEnabled(false);
                                    destination.highlighted = false;

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
                            player.capturedHostages[i].transform.DOMove(new Vector3(player.AgentTile.transform.position.x, 0.5f,
                                                                                    player.AgentTile.transform.position.z),
                                                                                    Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                                                .SetEase(Ease.Linear);
                            player.capturedHostages[i].AgentTile = player.AgentTile;
                        }
                        else
                        {
                            //Debug.Log("OTHER HOSTAGE");
                            player.capturedHostages[i].transform.DOMove(new Vector3(player.capturedHostages[i - 1].AgentTile.transform.position.x, 0.5f,
                                                        player.capturedHostages[i - 1].AgentTile.transform.position.z),
                                                        Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                                                .SetEase(Ease.Linear);
                            player.capturedHostages[i].AgentTile = player.capturedHostages[i - 1].AgentTile;
                        }
                    }
                }
                player.AgentTile = destination;
            }
            else
            {
                findNewPath = true;
            }

            if (findNewPath)
            {
                player.FindAnOtherPath();
            }
        }
        else if(agent is Mb_IAAgent)
        {
            Mb_IAAgent hostage = agent as Mb_IAAgent;

            if (!destination.avaible && destination == hostage.destination)
            {
                hostage.nextAction = true;
                return;
            }
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

            if (destination.avaible)
            {
                //Debug.Log("MOVE TO : "+ destination.transform.position);
                hostage.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f,
                                                     destination.transform.position.z),
                                                     Ma_LevelManager.Instance.clock.tickInterval * timeToPerform)
                                 .SetEase(Ease.Linear)
                                 .OnComplete(() =>
                                 {
                                     destination.SetOutlinesEnabled(false);
                                     destination.highlighted = false;

                                     if (!findNewPath)
                                         hostage.nextAction = true;
                                 });
                hostage.AgentTile = destination;
            }
            else if(destination.GetComponentInChildren<Mb_Door>() != null)
            {
                //Debug.Log("Set interaction");
                hostage.SetFirstInteraction(destination.GetComponentInChildren<Mb_Trial>());
                hostage.nextAction = true;
            }
            else
            {
                findNewPath = true;
            }

            if (findNewPath)
            {
                hostage.FindAnOtherPath();
            }
        }

    }
}
