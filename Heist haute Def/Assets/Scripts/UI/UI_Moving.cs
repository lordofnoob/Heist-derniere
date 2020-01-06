using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Moving : MonoBehaviour
{
    [SerializeField] RectTransform uiToMove;
    [SerializeField] Transform button;
    public Vector2 posFolded, posFinal;
    public KeyCode toggleKey;
    bool isDeployed = false;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            Check();
    }

    public void Check()
    {
        {
            if (isDeployed == false)
                Deploy();
            else
                Fold();
        }
    }


    public void Deploy()
    {
        RotateTheButton();
        uiToMove.DOAnchorPos(posFinal, .5f);
        isDeployed = true;
    }

    public void Fold()
    {
        RotateTheButton();
        uiToMove.DOAnchorPos(posFolded, .5f);
        isDeployed = false;
    }

    public void RotateTheButton()
    {
        button.DORotate(new Vector3(0, 0, button.rotation.z + 180), 0.5f, RotateMode.LocalAxisAdd);
    }
}