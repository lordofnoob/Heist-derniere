using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPanelEvent : MonoBehaviour
{

    public scr_UITween toTrigger;
    private bool oneTime;
    public float hangTime;
    public bool isTriggerable;
    public bool filling;

    public void OnMouseOver()
    {
        if (isTriggerable)
        {
            if (oneTime)
            {
                toTrigger.punch = true;
                toTrigger.doOnce = true;
                oneTime = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                filling = true;
                toTrigger.fill = true;
            }
        }
    }

    public void OnMouseExit()
    {
        if (!filling)
        {
            StartCoroutine("Availability");
        }
    }

    public IEnumerator Availability()
    {

        isTriggerable = false;
        toTrigger.bScale = false;
        oneTime = true;

        toTrigger.killIdle();
        yield return new WaitForSeconds(hangTime);
        isTriggerable = true;

    }
}
