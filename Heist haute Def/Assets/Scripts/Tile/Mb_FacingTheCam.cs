using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mb_FacingTheCam : MonoBehaviour
{
    public Transform transformToLookAt;
    public void Awake()
    {
        transformToLookAt = Editor.FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        transform.LookAt(transformToLookAt);
    }
}
