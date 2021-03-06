﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mb_FacingTheCam : MonoBehaviour
{
    public Transform transformToLookAt;
    Vector3 lookAtPose;

    public void Awake()
    {
        transformToLookAt = FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        lookAtPose = new Vector3(transform.position.x, transformToLookAt.position.y, transform.position.z);
        transform.LookAt(lookAtPose);
    }
}
