﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Ed_Mb_Generator : MonoBehaviour
{
    public Texture2D textureToTraduce;
    public float distanceBetweenItems;
    public Ed_Sc_ColorCode colorCodeAssociated;
    public Transform roomParent;
    [HideInInspector] public Sc_Charaspec charactSpectToInstantiate;
    [HideInInspector] public Mb_Player playerPrefab;

    public void InstantiateRoom()
    {
        Identify();
    }
    void Identify()
    {
        for (int x = 0; x < textureToTraduce.width; x++)
        {
            for (int z = 0; z < textureToTraduce.height; z++)
            {
                ReadTile(x, z, distanceBetweenItems);
            }
        }
    }
    void ReadTile(int x, int z, float distanceBetweenPixels)
    {
        Color currentPixelColor = textureToTraduce.GetPixel(x, z);

        if (currentPixelColor.a == 0)
            return;
        else
        {
            for (int i =0; i< colorCodeAssociated.colorCode.Length; i++)
            {
                if (colorCodeAssociated.colorCode[i].colorAssociated.Equals(currentPixelColor))
                {
                    Object newObject=  PrefabUtility.InstantiatePrefab(colorCodeAssociated.colorCode[i].prefabAssociated);
                    if(newObject is GameObject)
                    {
                        GameObject newGameObject = (newObject as GameObject);
                        newGameObject.transform.position = new Vector3(roomParent.position.x + x * distanceBetweenItems, roomParent.position.y, roomParent.position.z + z * distanceBetweenItems);
                        newGameObject.transform.SetParent(roomParent);
                        /*if (newGameObject.GetComponentInChildren<Mb_FacingTheCam>()==true)
                            newGameObject.GetComponentInChildren<Mb_FacingTheCam>().FindCam();*/
                    }
                       // colorCodeAssociated.colorCode[i].prefabAssociated, , Quaternion.identity, roomParent);
                }
            }

        }
    }
}
