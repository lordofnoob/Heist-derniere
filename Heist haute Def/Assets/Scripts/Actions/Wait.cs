using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Action
{
    private float timer = 0;
    public Wait(float timeToPerform, Mb_Agent agent) : base(timeToPerform, agent) { }

    public override void PerformAction()
    {
        while(timer < timeToPerform * Ma_ClockManager.Instance.tickInterval)
        {
            timer += Time.deltaTime;
        }
        agent.nextAction = true;
    }
}
