using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ObjectiveSetup : MonoBehaviour
{
    public objectiveBundle[] objectiveSpots;
}

[System.Serializable]
public struct objectiveBundle
{
    public Image objectiveStatusIcon;
    public TextMeshProUGUI objectiveDescription;
}