using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Door : Mb_Trial
{
    public Animation door;
    public Tile tileAssociated;
    public bool close = true;
    public bool isExitDoor = false;

    public override void DoThings()
    {
        if (close)
        {
            tileAssociated.Cost = 1;
        }
        else
        {
            tileAssociated.Cost = 3;
        }

        close = !close;
        GetComponent<MeshRenderer>().enabled = close;
        tileAssociated.avaible = !tileAssociated.avaible;

        //   door.
        //door.Play();
        ResetValues();
    }        
}
