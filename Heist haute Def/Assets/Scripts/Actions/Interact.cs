using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : Action
{
    private Mb_Trial trial;

    public Interact(float timeToPerform, Mb_Agent agent, Mb_Trial trial) : base(timeToPerform, agent) 
    {
        this.trial = trial;
    }

    public override void PerformAction()
    {
        //trial.StopMoving();
        agent.Interact();
    }

}
