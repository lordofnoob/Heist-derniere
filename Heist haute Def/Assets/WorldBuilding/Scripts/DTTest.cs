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

    public void Start()
    {
        DOTween.Init();
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);

    }

    public void Update()
    {
        duration = Time.deltaTime + 1;
        transform.DOShakePosition(duration, shakeStrength, vibrato, noise, isSnapping, isFadeOut);
    }

}
