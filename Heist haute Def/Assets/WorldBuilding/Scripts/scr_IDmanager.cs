using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class scr_IDmanager : MonoBehaviour
{

    public List<AnimIDCard> iDList;
    public int activeCard;

    public int forValue;
    private RectTransform toMove;

    public float offset;
    public float moveDuration;

    public bool doOnce;
    public bool oneTriggered;

    public static scr_IDmanager iDManager;
    public bool isOn;

    public bool oneUp;

    private void Start()
    {
        iDManager = this;
    }

    void Update()
    {

        if (isOn)
        {
            if (!oneTriggered)
            {
                Organiser();
                SetAside();
            }
            else
            {
                //activeCard = 0;
            }
        }





        /*
        if (activeCard == 0)
        {
            //oneTriggered = false;

            if(iDList.Count<5)
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
            

            if (forValue == iDList.Count)
            {
                for (int a = 0; a < iDList.Count; a++)
                {
                    iDList[a].enabled = true;
                }

                activeCard = 0;
                forValue = 0;
            }
        }
        else if (activeCard > 1)
        {
            for (int a = 0; a < iDList.Count; a++)
            {
                iDList[a].enabled = true;
            }

            /* activeCard = 0;
             forValue = 0;
        }*/


    }





    public void Organiser()
    {
        doOnce = true;

        if (iDList.Count > 0)
        {
            iDList.Clear();
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            iDList.Add(this.transform.GetChild(i).GetComponent<AnimIDCard>());
        }

        //activeCard = 0;

        for (forValue = 0; forValue < iDList.Count; forValue++)
        {
            //iDList.RemoveAt(forValue);

            if (iDList[forValue].activated == true)
            {
                if (forValue > 0)
                {
                    for (int i = 0; i < forValue; i++)
                    {
                        iDList.RemoveAt(i);
                    }
                }

                iDList.RemoveAt(forValue);

                oneUp = true;

                //activeCard++;

                //oneTriggered = true;

                for (int e = 0; e < iDList.Count; e++)
                {

                    if (iDList[forValue] != iDList[e])
                    {
                        iDList[e].enabled = false;
                    }

                }

                return;
            }
        }
    }




    public void SetAside()
    {

        if (iDList.Count > 0 && iDList.Count <= 4)
        {
            if (doOnce && oneUp)
            {
                for (int i = 0; i < iDList.Count; i++)
                {

                    toMove = iDList[i].GetComponent<RectTransform>();
                    toMove.DOAnchorPos(new Vector3(toMove.anchoredPosition.x + offset, toMove.anchoredPosition.y), moveDuration).SetAutoKill(true);
                    doOnce = false;

                }
            }

            if (!doOnce)
            {
                for (int i = 0; i < iDList.Count; i++)
                {

                    toMove = iDList[i].GetComponent<RectTransform>();
                    toMove.DOAnchorPos(new Vector3(toMove.anchoredPosition.x - offset, toMove.anchoredPosition.y), moveDuration).SetAutoKill(true);
                    //doOnce = true;

                }
            }
        }
    }


}
