using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_OutlineManagerR : MonoBehaviour
{

    public GameObject outline;
    private bool down;

    public void OnMouseDown()
    {
        outline.SetActive(true);
        down = true;
    }

    public void OnMouseUp()
    {
        down = false;
    }

    public void Update()
    {
        if(outline.activeInHierarchy == true && Input.GetMouseButtonDown(0) && !down)
        {
            outline.SetActive(false);
        }
    }
}
