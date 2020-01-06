using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mb_EscapePlayer : Mb_Trial
{
    public override void DoThings()
    {
        foreach(Mb_Agent player in listOfUser)
        {
            // A REMPLCER PAR DU PROPRE POUR LE FAIRE RENTRER DANS LA VOITURE 
            player.transform.DOMove(transform.position, 1);
            player.GetAgentTile().avaible = true;
            player.GetAgentTile().agentOnTile = null;
            //

            player.animator.SetTrigger("Escape");
            player.state = StateOfAction.Escaped;
            Ma_LevelManager.instance.CheckEscape();
        }
        base.DoThings();
    }
}
