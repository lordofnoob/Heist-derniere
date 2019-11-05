using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Mb_PoolManager))]
public class In_PoolManagerInterface : Editor
{
    Mb_PoolManager mySelectedScript;

    private void OnEnable()
    {
        mySelectedScript = target as Mb_PoolManager;
        Undo.undoRedoPerformed += MyUndoCallback;
    }
    void MyUndoCallback()
    {
        Debug.Log("undo has been perfromed");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Undo.RecordObject(mySelectedScript, "Edited Something");
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("GeneratePools", GUILayout.MinHeight(50)))
        {
            ClearPools(mySelectedScript.prefabsInstations);
            BeginPooling();
            EditorSceneManager.MarkSceneDirty(mySelectedScript.gameObject.scene);
        }

        EditorGUI.EndChangeCheck();

    void BeginPooling()
    {
        for (int i = 0; i < mySelectedScript.prefabsInstations.Length; i++)
        {
            EnHancePooling(mySelectedScript.prefabsInstations[i]);
        }
    }
   
    void EnHancePooling(PrefabsPoolingParameters basicCharacts)
    {
        for (int j = 0; j < basicCharacts.numberToGenerate; j++)
        {
            Mb_Poolable newItem = Instantiate(basicCharacts.pooledItem);
            newItem.transform.parent = basicCharacts.associatedTransformPos.transform;
            newItem.gameObject.SetActive(false);
            basicCharacts.associatedTransformPos.listOfPoolableItem
                    .Add(newItem);

            PrefabUtility.RecordPrefabInstancePropertyModifications(mySelectedScript);
        }

        for (int p = 0; p < mySelectedScript.prefabsInstations.Length; p++)
        {
            PrefabUtility.RecordPrefabInstancePropertyModifications(mySelectedScript.prefabsInstations[p].associatedTransformPos); 
        }
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());


    }

    void ClearPools(PrefabsPoolingParameters[] basicCharacts)
    {
        for (int j = 0; j < basicCharacts.Length; j++)
        {
            basicCharacts[j].associatedTransformPos.listOfPoolableItem.Clear();

            Transform parent = basicCharacts[j].associatedTransformPos.transform;
            int safeCounter = 0;

            while (parent.childCount > 0)
            {
                DestroyImmediate(parent.GetChild(0).gameObject);
                safeCounter++;
                if (safeCounter > 10000)
                    break;
            }
        }
    }
        }
}
