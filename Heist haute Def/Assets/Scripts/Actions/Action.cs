using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float timeToPerform;
    public Mb_Agent agent;

    public Action(float timeToPerform, Mb_Agent agent)
    {
        this.timeToPerform = timeToPerform;
        this.agent = agent;
    }

    public virtual void PerformAction() { }
}
