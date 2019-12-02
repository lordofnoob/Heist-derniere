using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_LockedDoor : Mb_Door
{

    public bool locked = false;
    public KeyColor associatedColor;

    public override bool CheckCondition()
    {
        if (locked)
            foreach (Mb_Player player in listOfUser)
            {
                foreach (Sc_Key keycolor in player.itemsHold)
                {
                    if (associatedColor == keycolor.colorAssociated)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
        return false;

    }

    public override void DoThings()
    {
        locked = false;
        for (int i = 0; i < listOfUser.Count; i++)
        {
            listOfUser[i].SetNewActionState(StateOfAction.Idle);
        }
        listOfUser.Clear();
        doorAnim.SetTrigger("DoThings");
        base.DoThings();
    }
}
