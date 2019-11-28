using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class scr_IDmanager : MonoBehaviour
{
    
    public List<AnimIDCard> iDList;

    public bool doOnce;

    public static scr_IDmanager iDManager;
    public bool isOn;

    public AnimIDCard triggeredID;

    private void Start()
    {
        iDManager = this;

        if (iDList.Count < 5)
        {
            if (iDList.Count > 0)
            {
                iDList.Clear();
            }

            for (int i = 0; i < this.transform.childCount; i++)
            {
                iDList.Add(this.transform.GetChild(i).GetComponent<AnimIDCard>());
            }
        }
    }

    void Update()
    {

        if (isOn)
        {
            doOnce = true;

            if (iDList.Count == 5)
            {
                for (int a = 0; a < iDList.Count; a++)
                {
                    if (iDList[a] != triggeredID)
                    {
                        iDList[a].enabled = false;
                    }
                }
            }
        }
        else
        {
            if (doOnce)
            {
                for (int a = 0; a < iDList.Count; a++)
                {
                    if (iDList[a] != triggeredID)
                    {
                        iDList[a].enabled = true;
                        triggeredID = null;

                    }
                }

                doOnce = false;

            }
        }

    }
}
