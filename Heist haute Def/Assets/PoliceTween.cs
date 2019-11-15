using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoliceTween : MonoBehaviour
{
    public float duration;
    public Vector3 shakeStrength;
    public int vibrato;
    public float noise;
    public bool isFadeOut;

    public Vector3 originScale;
    public float resetDist;
    public float resetSpeed;


    public void Start()
    {
        originScale = transform.localScale;
    }

    public void Update()
    {
        duration = Time.deltaTime + 1;

        if (transform.localScale.x > originScale.x + resetDist || transform.localScale.x < originScale.x - resetDist || transform.localScale.y < originScale.y - resetDist || transform.localScale.y > originScale.y + resetDist || transform.localScale.z < originScale.z - resetDist || transform.localScale.z > originScale.z + resetDist)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originScale, resetSpeed);
        }
        else
        {
            transform.DOShakeScale(duration, shakeStrength, vibrato, noise, isFadeOut);
        }
    }



}
