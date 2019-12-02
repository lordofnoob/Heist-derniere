using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HostageState
{
    Free,
    Captured,
    Stocked,
    InPanic
}

public class Mb_IATrial : Mb_Trial
{
    public Mb_IAAgent iaAgent;




    public override void DoThings()
    {
        
        Debug.Log(iaAgent.onGoingInteraction);

        if (iaAgent.onGoingInteraction != null)
        {
            iaAgent.onGoingInteraction.listOfUser.Remove(iaAgent);
            iaAgent.onGoingInteraction.ReUpduateTiming();
        }

        listOfUser[0].nextAction = true;
        Ma_IAManager.Instance.IAHostageFollowingPlayer(iaAgent, listOfUser[0]);
        ResetValues();
        base.DoThings();
    }

}
