using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimIDCard : MonoBehaviour
{

    public Tween popTween;
    public Tween dePopTween;

    public RectTransform cardID;

    public Vector2 endTarget;
    public float jumpPower;
    public float speed;

    public Vector2 endTargetR;
    public float jumpPowerR;
    public float speedR;

    public bool idle;
    public bool activated;

    public KeyCode toggleKey;

    void Update()
    {

        if (Input.GetKey(toggleKey) && !idle)
        {
            if(scr_IDmanager.iDManager.isOn == false)
            {
                scr_IDmanager.iDManager.isOn = true;
            
            activated = true;
            popTween = cardID.DOJumpAnchorPos(endTarget, jumpPower, 1, speed, false).SetAutoKill(true);
            popTween.OnComplete(() =>
            {
                scr_IDmanager.iDManager.oneTriggered = true;
                idle = true;
            });
            }
        }


        if (Input.GetKey(toggleKey) && idle)
        {
            activated = false;
            dePopTween = cardID.DOJumpAnchorPos(endTargetR, jumpPowerR, 1, speedR, false).SetAutoKill(true);
            dePopTween.OnComplete(() =>
            {
                scr_IDmanager.iDManager.oneTriggered = false;
                idle = false;
                scr_IDmanager.iDManager.isOn = false;

            });
        }
    }
}
