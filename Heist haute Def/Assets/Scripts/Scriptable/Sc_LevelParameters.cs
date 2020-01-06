using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevelParameters", menuName = "levelParameter") ]
public class Sc_LevelParameters : ScriptableObject
{
    public Sprite mapIcon;
    public Sprite levelScreen;
    public string LevelName;
    [TextArea] public string levelDescription;
    public Sc_PlayerSpecs[] allPlayerDescription;
    public Sc_Objective[] allObjectives;
    public Scene sceneAssociated;



    public float timeAvaibleBeforePolice;
    public float totalTimeToComplete;

  
}