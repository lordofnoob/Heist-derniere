using System.Collections;
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
    public float finalTimeToSpendOn = 1;

    public bool punch;
    public Vector2 punchDir;
    public float duration;
    public int vibrato;
    public float elasticity;
    public bool snapping;

    public float scalingSpeed;
    private float scaleValue;
    public bool bScale;

    public Vector2 originPos;
    public bool doOnce;

    private float vignetCompletion = 0;
    private float currentTimeSpentOn = 0;
    float tickInterval = 0;

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
    private Tween idleTween;

    [Space(10)]
    [Header("Circle Parameter")]
    public Vector3 targetC;
    public float sizingSpeedC;

    public TriggerPanelEvent myTpe;

    void Awake()
    {
        tickInterval = Ma_ClockManager.Instance.tickInterval;
        Ma_ClockManager.Instance.tickTrigger.AddListener(Fill);
      /*  originPos.x = -0.5f;

        originPos.y = 0.5f;*/
    }

    void Update()
    {
        // /!\ bcp en update.. => à opti

        panel.localScale = new Vector3(scaleValue, scaleValue, 1);

        vignetCompletion = Mathf.Lerp(vignetCompletion, currentTimeSpentOn, tickInterval);
        ring.fillAmount = vignetCompletion / finalTimeToSpendOn;


        //Gère la jauge d'avancement du trial
        /*if (fill)
        {
            Fill();
        }
        else
        {
            currentTimeSpentOn = 0;
        }*/

        //Gère l'animation de "bond" puis d'idle lors de l'apparition de l'icone
        if (punch)
        {

            bScale = true;

            sequence.Append(panel.DOPunchAnchorPos(punchDir, duration, vibrato, elasticity, snapping));
            sequence.PrependInterval(transitionTime);
            idleTween = panel.DOShakeAnchorPos(Iduration, Istrength, Ivibrato, Irandomness, Isnapping, IfadeOut).SetAutoKill(false);
            idleTween.OnComplete(() => idleTween.Restart());
            sequence.Append(idleTween);
            alreadyFull = true;

            punch = false;
        }

        //Gère la scale de l'icone lors de l'apparition
        if (bScale)
        {
            if (scaleValue < 1)
            {
                scaleValue += scalingSpeed;
            }
        }
        else
        {
            panel.anchoredPosition3D = new Vector3(0, originPos.y, panel.anchoredPosition3D.z);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, originPos.x);
            scaleValue = 0;
        }
    }

    // Remet la position de l'icone à son start point
    public void RestatPos()
    {
        if (doOnce)
        {
            panel.anchoredPosition3D = new Vector3(panel.anchoredPosition3D.x, originPos.y, panel.anchoredPosition3D.z);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, originPos.x);

            punch = false;
            doOnce = false;
            bScale = false;
            fill = false;
            vignetCompletion = 0;
            currentTimeSpentOn = 0;
            myTpe.filling = false;
        }
    }

    //Gère la jauge d'avancement du trial
    public void Fill()
    {
        if (fill)
        {
            bScale = true;
            currentTimeSpentOn += tickInterval;
        }

        //Debug.Log("CurrentTimeSpentOn : "+currentTimeSpentOn);
        if (currentTimeSpentOn > finalTimeToSpendOn)
        {
            currentTimeSpentOn = 0;
            fill = false;
            circle.rectTransform.DOScale(targetC, sizingSpeedC).OnComplete(()=> 
            {
                //Debug.Log("DO THING : "+ myTpe.name);
                myTpe.GetComponent<Mb_Trial>().DoThings();
            });
        }
        else
        {
            circle.rectTransform.DOScale(new Vector3(0, 0, 1), 0.8f);
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            myTpe.filling = false;
            fill = false;
        }*/
    }

    // Termine l'idle state
    public void killIdle()
    {

        idleTween.Kill(true);
        sequence.Kill(true);

    }
}
