using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mb_FacingTheCam : MonoBehaviour
{
    public Transform transformToLookAt;
    public void Awake()
    {
        transformToLookAt = FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        transform.LookAt(transformToLookAt);
    }
}
