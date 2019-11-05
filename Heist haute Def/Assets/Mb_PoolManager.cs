using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mb_PoolManager : MonoBehaviour
{
    public Mb_PoolManagement_Stock stockPrefab;
    public PrefabsPoolingParameters[] prefabsInstations;
    public static Mb_PoolManager poolManager;

    /*
    public ParticleSystem CallParticle(Vector3 position, float duration)
    {
        
        return particleSystem;
    }*/


    private void Awake()
    {
        poolManager = this;
    }

}

[System.Serializable]
public struct PrefabsPoolingParameters
{
    public Mb_Poolable pooledItem;
    [Range(10, 300)] public int numberToGenerate;
    public Mb_PoolManagement_Stock associatedTransformPos;
}