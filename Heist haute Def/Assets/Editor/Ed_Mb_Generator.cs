using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;   

public class Ed_Mb_Generator : MonoBehaviour
{
    public Texture2D textureToTraduce;
    public float distanceBetweenItems;
    public Ed_Sc_ColorCode colorCodeAssociated;  

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
            for (int i =0; i< colorCodeAssociated.colorCode.Count; i++)
            {
                if (colorCodeAssociated.colorCode[i].colorAssociated.Equals(currentPixelColor))
                {
                   /* Mb_PoolManager.PoolManager.CallItem(
                        colorCode.itemTospawn,
                        positionOfTheSpawn.position + new Vector3(x * distanceBetweenPixels, 0, z * distanceBetweenPixels),
                        180);*/
                }
            }

        }
    }
}
