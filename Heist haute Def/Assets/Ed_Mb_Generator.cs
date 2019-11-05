using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;   

public class Ed_Mb_Generator : MonoBehaviour
{
    public Texture2D textureToTraduce;
    public float distanceBetweenItems;
    public Ed_Sc_ColorCode colorCodeAssociated;
    public Transform roomParent;

    public void InstantiateRoom()
    {
        Identify();
    }
    void Identify()
    {
        Debug.Log(textureToTraduce.width);
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
                    Instantiate(colorCodeAssociated.colorCode[i].prefabAssociated, new Vector3(roomParent.position.x + x * distanceBetweenItems, roomParent.position.y, roomParent.position.z + z * distanceBetweenItems), Quaternion.identity, roomParent);
                }
            }

        }
    }
}
