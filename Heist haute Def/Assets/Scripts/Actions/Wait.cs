using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Action
{
    private float timer = 0;
    public Wait(float timeToPerform, Mb_Agent agent) : base(timeToPerform, agent) { }
    private float intervalTime, waitingTime;

    void Start()
    {
        intervalTime = Ma_ClockManager.Instance.tickInterval;
        Ma_ClockManager.Instance.tickTrigger.AddListener(this.PerformAction);
    }

    public override void PerformAction()
    {
        if (timeToPerform < waitingTime)
        {
            waitingTime += intervalTime;
        }
        else
            agent.nextAction = true;
    }
}
