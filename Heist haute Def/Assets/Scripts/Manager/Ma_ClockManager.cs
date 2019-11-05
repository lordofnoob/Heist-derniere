using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ma_ClockManager : MonoBehaviour
{
    public static Ma_ClockManager Instance;

    public UnityEvent tickTrigger;

    public float tickInterval = 0.5f;
    void Awake()
    {
        Instance = this;

        if (tickTrigger == null)
            tickTrigger = new UnityEvent();

        StartCoroutine(Sequencer());
    }

    IEnumerator Sequencer()
    {
        yield return new WaitForSeconds(tickInterval);

        tickTrigger.Invoke();

        StartCoroutine(Sequencer());
    }
}
