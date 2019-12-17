using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mb_UiMenu : MonoBehaviour
{
    public Sc_SkillsIconsInfo skillIconRule;
    [SerializeField] TextMeshProUGUI levelTitle;
    [SerializeField] Image cardMiniature;
    [SerializeField] TextMeshProUGUI levelDescription;
    [SerializeField] Image levelIcon;
    [SerializeField] TextMeshProUGUI timeBeforePoliceArrive;
    [SerializeField] TextMeshProUGUI[] objectiveDescription;
    [SerializeField] CharacterParameters[] allCharacterPreview;
 
    [System.Serializable]
    public struct CharacterParameters
    {
        public Image characterPortrait;
        public Image[] characterSkills;
        public TextMeshProUGUI characterCapacity;
        public TextMeshProUGUI characterName;
    }

    public void SetupUi(Sc_LevelParameters parametersToDisplay)
    {
        levelDescription.text = parametersToDisplay.levelDescription;

        cardMiniature.sprite = parametersToDisplay.mapIcon;

        levelIcon.sprite = parametersToDisplay.levelScreen;

        for (int i = 0; i < objectiveDescription.Length; i++)
            objectiveDescription[i].text = parametersToDisplay.allObjectives[i].objectifDescription;

        //SETUP DES PERSOS EN PREVIEW
        for (int i = 0; i < allCharacterPreview.Length; i++)
        {
            //previewPortrait
            allCharacterPreview[i].characterPortrait.sprite = parametersToDisplay.allPlayerDescription[i].characterPortrait;

            //previewSkill
            Debug.Log(parametersToDisplay.allPlayerDescription[i].characterSkills.Length);
            for (int j = 0; j < parametersToDisplay.allPlayerDescription[i].characterSkills.Length; j++)
            {
                for (int n = 0; n < skillIconRule.allSkillsIcon.Length; n++)
                {
                    if (parametersToDisplay.allPlayerDescription[i].characterSkills[j] == skillIconRule.allSkillsIcon[n].skillAssociated)
                        allCharacterPreview[i].characterSkills[j].sprite = skillIconRule.allSkillsIcon[n].skillIcon;
                }
            }
            //Preview Capacity
            allCharacterPreview[i].characterCapacity.text = parametersToDisplay.allPlayerDescription[i].surveillanceLimit.ToString();
            allCharacterPreview[i].characterName.text = parametersToDisplay.allPlayerDescription[i].characterName;
        }

        //SETUP DES OBJECTIFS
        for (int i = 0; i < parametersToDisplay.allObjectives.Length; i++)
            objectiveDescription[i].text = parametersToDisplay.allObjectives[i].objectifDescription;

        timeBeforePoliceArrive.text = parametersToDisplay.timeAvaibleBeforePolice.ToString();
        levelTitle.text = parametersToDisplay.LevelName;
    }

}
