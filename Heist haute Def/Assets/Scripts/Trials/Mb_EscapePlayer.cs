using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_EscapePlayer : Mb_Trial
{
    public override void DoThings()
    {
        foreach(Mb_Agent player in listOfUser)
        {
            player.state = StateOfAction.Escaped;
        }
        base.DoThings();
    }
}
