﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class scr_UITween : MonoBehaviour
{
    public Image ring;
    public Image circle;
    public RectTransform panel;
    public bool fill;
    public float fillTime;

    public bool punch;
    public Vector2 punchDir;
    public float duration;
    public int vibrato;
    public float elasticity;
    public bool snapping;

    public float scalingSpeed;
    public float scaleValue;
    public bool bScale;

    public Vector2 originPos;
    public bool doOnce;

    public float fillValue;
    public float fillSpeed = 0.1f;

    [Space(10)]
    [Header("Idle Parameter")]
    public float Iduration;
    public Vector3 Istrength;
    public int Ivibrato;
    public float Irandomness;
    public bool Isnapping;
    public bool IfadeOut;


    static Sequence sequence;
    public float transitionTime;
    public bool alreadyFull;

    void Start()
    {
        originPos.x = -0.5f;

        originPos.y = 0.5f;

    }

    void Update()
    {

        panel.localScale = new Vector3(scaleValue, scaleValue, 1);
        ring.fillAmount = fillValue;
        circle.fillAmount = fillValue;

        if (fill)
        {
            Fill();
                /*
            ring.DOFillAmount(1, fillTime * Time.deltaTime);
            circle.DOFillAmount(1, fillTime * Time.deltaTime);*/

        }
        else
        {
            fillValue = 0;
            /*
            ring.fillAmount = 0;
            circle.fillAmount = 0;*/

        }

        if (punch)
        {

            bScale = true;
            if(!alreadyFull)
            {
                sequence.Append(panel.DOPunchAnchorPos(punchDir, duration, vibrato, elasticity, snapping));
                sequence.PrependInterval(transitionTime);
                sequence.Append(panel.DOShakeAnchorPos(Iduration, Istrength, Ivibrato, Irandomness, Isnapping, IfadeOut)/*.SetLoops(-1, LoopType.Restart)*/);
                alreadyFull = true;
            }
            else
            {
                sequence.Restart();
            }

            // doOnce = true;
            // Idle();
            punch = false;
        }
        else
        {
           // RestatPos();
        }

        if (bScale)
        {
            if (scaleValue < 1)
            {
                scaleValue += scalingSpeed;
            }
        }
        else
        {
            scaleValue = 0;
        }
    }


    public void RestatPos()
    {
        if(doOnce)
        {
            panel.anchoredPosition3D = new Vector3(panel.anchoredPosition3D.x, originPos.y, panel.anchoredPosition3D.z);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, originPos.x);

            punch = false;
            doOnce = false;
        }

    }

    public void Fill()
    {
        if(fillValue < 1 )
        {
            fillValue += fillSpeed;
        }

        if(Input.GetMouseButtonDown(1))
        {
            fill = false;
        }
    }

    public void killIdle()
    {
        sequence.Kill();
        sequence.Pause();
    }
}
