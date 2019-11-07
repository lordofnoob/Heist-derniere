using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Deplacement : Sc_Action
{
    public Tile destination;
    public Mb_Player toMove;

    public Sc_Deplacement(float timeToPerform, Mb_Player toMove, Tile destination) : base(timeToPerform)
    {
        this.toMove = toMove;
        this.destination = destination;
    }

    public override void PerformAction()
    {
        if (destination.avaible)
        {
            destination.avaible = false;
            toMove.playerTile.avaible = true;
            toMove.playerTile = destination;

            //Debug.Log("MOVE TO : "+ destination.transform.position);
            toMove.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f, destination.transform.position.z), Ma_LevelManager.Instance.clock.tickInterval * timeToPerform).SetEase(Ease.Linear).OnComplete(() => {
                destination.SetOutlinesEnabled(false);
                destination.highlighted = false;

                if (destination == toMove.destination)
                {
                    toMove.state = Mb_Player.StateOfAction.Idle;
                    toMove.destination = null;
                }

                toMove.nextAction = true;
            });
        }

        //IF PLAYER PATH HAS CHANGE DURING DEPLACEMENT
        bool findNewPath = false;
        foreach(Sc_Action action in toMove.actionsToPerform)
        {
            if(action is Sc_Deplacement)
            {
                Sc_Deplacement deplacement = action as Sc_Deplacement;
                if (!deplacement.destination.avaible)
                {
                    findNewPath = true;
                    break;
                }
            }
        }

        if (findNewPath)
        {
            for(int i = 0; i<toMove.actionsToPerform.Count; i++)
            {
                if (toMove.actionsToPerform[i] is Sc_Deplacement)
                    toMove.actionsToPerform.RemoveAt(i);
            }

            List<Tile> newShortestPath = Ma_LevelManager.Instance.GetComponentInChildren<Pathfinder>().SearchForShortestPath(toMove.playerTile, new List<Tile> { toMove.destination }, toMove.destination);
            toMove.AddDeplacement(newShortestPath);
        }

    }
}
