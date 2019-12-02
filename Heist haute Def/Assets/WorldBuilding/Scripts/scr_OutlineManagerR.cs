using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_OutlineManagerR : MonoBehaviour
{

    public GameObject outline;
    private bool selection;
    public SkinnedMeshRenderer outlineRend;
    public Material outlineMat;

    public bool over;

    [Header("   Standard Preset    ")]
    [Space(20)]
    public Color32 outSimpleColor;
    public float outSimpleThickness;
    public float outSimpleIntensity;

    [Header("   Over Preset    ")]
    [Space(20)]
    public Color32 outOverColor;
    public float outOverThickness;
    public float outOverIntensity;

    [Header("   Selection Preset    ")]
    [Space(20)]
    public Color32 outSelectionColor;
    public float outSelectionThickness;
    public float outSelectionIntensity;

    public void Start()
    {
        outlineRend = outline.GetComponent<SkinnedMeshRenderer>();
        outlineMat = outlineRend.material;

        SimplePreset();
    }

    public void OnMouseOver()
    {
        if(!selection)
        {
            if(!over)
            {
                OverPreset();
                over = true;
            }
        }
    }

    public void OnMouseExit()
    {
        over = false;
        if(!selection)
        {
            SimplePreset();
        }
    }


    public void OnMouseDown()
    {
        SelectionPreset();
        selection = true;
    }

    public void OnMouseUp()
    {
       // selection = false;
    }

    public void Update()
    {
        if(outline.activeInHierarchy == true && Input.GetMouseButtonDown(0) && !over)
        {
            selection = false;
            SimplePreset();
        }
    }

    public void OverPreset()
    {
        outlineMat.SetColor("_outColor", outOverColor);
        outlineMat.SetFloat("_outWidth", outOverThickness);
        outlineMat.SetFloat("_outIntensity", outOverIntensity);
    }

    public void SimplePreset()
    {
        outlineMat.SetColor("_outColor", outSimpleColor);
        outlineMat.SetFloat("_outWidth", outSimpleThickness);
        outlineMat.SetFloat("_outIntensity", outSimpleIntensity);
    }

    public void SelectionPreset()
    {
        outlineMat.SetColor("_outColor", outSelectionColor);
        outlineMat.SetFloat("_outWidth", outSelectionThickness);
        outlineMat.SetFloat("_outIntensity", outSelectionIntensity);
    }
}
