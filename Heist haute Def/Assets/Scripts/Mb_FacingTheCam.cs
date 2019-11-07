using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mb_FacingTheCam : MonoBehaviour
{
    Transform transformToLookAt;
    public void FindCam()
    {
        transformToLookAt = Editor.FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        transform.LookAt(transformToLookAt);
    }
}
