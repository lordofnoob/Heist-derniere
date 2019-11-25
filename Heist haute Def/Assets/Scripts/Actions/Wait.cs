using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Function();

public class Wait : Action
{
    private float timer = 0;
    private Function method;
    public Wait(float timeToPerform, Mb_Agent agent,  Function method) : base(timeToPerform, agent)
    {
        this.method = method;
    }

    public override void PerformAction()
    {
        Debug.Log("Wait");
        agent.StartCoroutine(agent.WaitForTime(timeToPerform * Ma_ClockManager.Instance.tickInterval));
    }
}
