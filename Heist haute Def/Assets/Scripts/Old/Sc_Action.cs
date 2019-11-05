using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Action
{
    public float timeToPerform;

    public Sc_Action(float timeToPerform)
    {
        this.timeToPerform = timeToPerform;
    }

    public virtual void PerformAction() { }
}
