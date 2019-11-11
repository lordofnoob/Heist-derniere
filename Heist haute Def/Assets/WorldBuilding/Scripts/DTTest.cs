using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DTTest : MonoBehaviour
{
    public float duration;
    public Vector3 shakeStrength;
    public int vibrato;
    public float noise;
    public bool isSnapping;
    public bool isFadeOut;

    public Vector3 originPos;
    public float resetDist;
    public float resetSpeed;
    public void Start()
    {
        DOTween.Init();
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(500, 10);

        originPos = transform.position;
    }

    public void Update()
    {
        duration = Time.deltaTime + 1;

        if (transform.position.x > originPos.x + resetDist || transform.position.x < originPos.x - resetDist || transform.position.y < originPos.y - resetDist || transform.position.y > originPos.y + resetDist)
        {
            transform.position = Vector3.Lerp(transform.position, originPos, resetSpeed);
        }
        else
        {
            transform.DOShakePosition(duration, shakeStrength, vibrato, noise, isSnapping, isFadeOut);
        }
    }

    

}
