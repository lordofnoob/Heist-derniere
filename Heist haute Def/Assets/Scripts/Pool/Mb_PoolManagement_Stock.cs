using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_PoolManagement_Stock : MonoBehaviour
{
    public Queue<Mb_Poolable> queuOfPoolableItem;
    public List<Mb_Poolable> listOfPoolableItem;

    private void Awake()
    {/*
        for (int i = 0; i < listOfPoolableItem.Count; i++)
            queuOfPoolableItem.Enqueue(listOfPoolableItem[i]);
    */}
}
