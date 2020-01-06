using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_PlayerTrial : Mb_Trial
{
    public Mb_Player playerAgent;

    public override void DoThings()
    {
        
        //Debug.Log(iaAgent.onGoingInteraction);
        //Debug.Log("DO THINGS");

        if (playerAgent.onGoingInteraction != null)
        {
            playerAgent.onGoingInteraction.listOfUser.Remove(playerAgent);
            playerAgent.onGoingInteraction.ReUpduateTiming();
        }

        listOfUser[0].nextAction = true;
        playerAgent.SetNewActionState(StateOfAction.Captured);
        ResetValues();
        base.DoThings();
    }

}
