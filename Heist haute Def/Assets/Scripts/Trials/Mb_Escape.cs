﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Escape : Mb_Trial
{
    public override void DoThings()
    {
        foreach(Mb_IAAgent iaAgent in listOfUser)
        {
            Ma_LevelManager.instance.timeRemaining -= iaAgent.aiCharacteristics.escapeValue;
            iaAgent.AgentTile.avaible = true;
            Destroy(iaAgent);
        }

        base.DoThings();
    }
}
