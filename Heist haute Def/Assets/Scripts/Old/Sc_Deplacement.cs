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
        //Debug.Log("MOVE TO : "+ destination.transform.position);
        toMove.transform.DOMove(new Vector3(destination.transform.position.x, 0.5f, destination.transform.position.z), Ma_LevelManager.Instance.clock.tickInterval * timeToPerform).SetEase(Ease.Linear).OnComplete(() => {
            toMove.nextAction = true;
        });
    }
}
