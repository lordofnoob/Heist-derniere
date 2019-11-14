using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPanelEvent : MonoBehaviour
{

    public scr_UITween toTrigger;
    private bool oneTime;
    public void OnMouseOver()
    {
        if (oneTime)
        {
            toTrigger.punch = true;
            toTrigger.doOnce = true;
            oneTime = false;
        }
        if(Input.GetMouseButtonDown(0))
        {
            toTrigger.fill = true;
        }
    }

    public void OnMouseExit()
    {
        toTrigger.RestatPos();
        toTrigger.bScale = false;
        oneTime = true;

    }
}
