using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Function();

public class Wait : Action
{
    public Wait(float timeToPerform, Mb_Agent agent) : base(timeToPerform, agent)
    {}

    public override void PerformAction()
    {
        //Debug.Log("Wait");
        agent.StartCoroutine(agent.WaitForTime(timeToPerform * Ma_ClockManager.instance.tickInterval));
    }
}
