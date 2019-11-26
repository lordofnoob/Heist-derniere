using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AnimationZoneR : MonoBehaviour
{
    public Transform wZone;
    public SpriteRenderer gZone;

    public Tween idleTweenGUp;

    public Sequence idleSequence;
    public Sequence colorSequence;

    public float height;
    public float duration;

    public SpriteRenderer voile;
    private Tween voileTween;
    private Tween voileTweenR;
    public Color alertColor;
    public float colorSpeed;
    public bool alerte;
    public Color baseColor;
    public bool initiateColorS;

    void Start()
    {
        DOTween.SetTweensCapacity(1000, 50);

        idleSequence = wZone.DOJump(new Vector3(wZone.position.x, wZone.position.y, wZone.position.z), height, 1, duration).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {

        if (alerte)
        {
            voileTween = voile.DOColor(alertColor, colorSpeed)/*.SetLoops(-1, LoopType.Yoyo)*/;
            //voileTween.OnComplete(() => voileTween.Rewind());
            colorSequence.Append(voileTween);
            voileTweenR = voile.DOColor(baseColor, colorSpeed)/*.SetLoops(-1, LoopType.Yoyo)*/;
            colorSequence.Append(voileTweenR);
            initiateColorS = true;
            alerte = false;
        }

        if(initiateColorS)
        {

            BeginColorSequence();

        }

    }

    public void BeginColorSequence()
    {
      // colorSequence.Restart(true);
        colorSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
