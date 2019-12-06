using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Door : Mb_Trial
{
    public Animator doorAnim;
    public Tile[] tileAssociated;
    public bool close = true;

    public override void DoThings()
    { 
        close = !close;
        doorAnim.SetTrigger("DoThings");
        foreach (Tile tileToSet in tileAssociated)
            tileToSet.avaible = !tileToSet.avaible;

        ResetValues();
        base.DoThings();
    }        
}
