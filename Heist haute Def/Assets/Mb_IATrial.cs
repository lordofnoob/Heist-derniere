using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HostageState
{
    Free,
    Captured,
    Stocked,
    InPanic
}

public class Mb_IATrial : Mb_Trial
{
    [HideInInspector] public Mb_IAAgent IAAgent;

    private void Awake()
    {
        IAAgent = GetComponent<Mb_IAAgent>();
    }

    void Update()
    {
        Counting();
    }

    public override void DoThings()
    {
        Debug.Log("TARGET CAPTURED");
        listOfUser[0].nextAction = true;
        Ma_IAManager.Instance.IAHostageFollowingPlayer(IAAgent, listOfUser[0]);

        ResetValues();
    }

}
