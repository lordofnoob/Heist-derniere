using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mb_Trial : Mb_Poolable
{
    [Header("Parameters")]
    public Sc_TrialDefinition trialParameters;
    public Tile[] positionToGo;

    [Header("Interface")]
    public Image timeVignet;
    //A rechanger en private
    public List<Mb_Player> listOfUser= new List<Mb_Player>();

   
    private float currentTimeSpentOn=0;
    private float finalTimeToSpendOn=1;
    private bool counting;
    private List<float> reductionList;
    private float definitiveModifier=1;

    void Awake()
    {

    }

     public void StartInteracting()
    {

        if (CheckCondition())
        {
            foreach (Mb_Player player in listOfUser)
            {
                int length = player.characterProperty.characterSkills.Length;
                for (int i = 0; i < player.characterProperty.characterSkills.Length; i++)
                    for (int y = 0; y < trialParameters.skillToUse.Length; y++)
                        if (player.characterProperty.characterSkills[i] == trialParameters.skillToUse[y].associatedSkill)
                        {
                            if (definitiveModifier > (1 - trialParameters.skillToUse[y].associatedReduction))
                            {
                                definitiveModifier = (1 - trialParameters.skillToUse[y].associatedReduction);
                            }
                            else if (definitiveModifier <= (definitiveModifier - trialParameters.skillToUse[y].associatedReduction) && definitiveModifier >= 1)
                            {
                                definitiveModifier = (1 - trialParameters.skillToUse[y].associatedReduction);
                            }

                        }
            }
        }

        finalTimeToSpendOn = trialParameters.timeToAccomplishTrial* definitiveModifier;
        
        currentTimeSpentOn = 0;
        counting = true;
    }

    public void Counting()
    {

        if (counting==true)
            currentTimeSpentOn += Time.deltaTime * listOfUser.Count;

        if (currentTimeSpentOn > finalTimeToSpendOn)
        {
            DoThings();
        }

        timeVignet.fillAmount = currentTimeSpentOn / finalTimeToSpendOn;
    }

    public void ReUpduateTiming()
    {
        foreach (Mb_Player player in listOfUser)
        {
            Debug.Log(player.characterProperty);
            int length = player.characterProperty.characterSkills.Length;
            Debug.Log(length);
            for (int i = 0; i < player.characterProperty.characterSkills.Length; i++)
                for (int y = 0; y < trialParameters.skillToUse.Length; y++)
                    if (player.characterProperty.characterSkills[i] == trialParameters.skillToUse[y].associatedSkill)
                    {
                        if (definitiveModifier > (1 - trialParameters.skillToUse[y].associatedReduction))
                        {
                            definitiveModifier = (1 - trialParameters.skillToUse[y].associatedReduction);
                        }
                        else if (definitiveModifier <= (definitiveModifier - trialParameters.skillToUse[y].associatedReduction) && definitiveModifier >= 1)
                        {
                            definitiveModifier = (1 - trialParameters.skillToUse[y].associatedReduction);
                        }

                    }
        }
        finalTimeToSpendOn = trialParameters.timeToAccomplishTrial * definitiveModifier;
        counting = true;
    }

    private void Update()
    {
        Counting();
    }

    public virtual void DoThings()
    {}

    public void ResetValues()
    {
        counting = false;
        currentTimeSpentOn = 0;
        definitiveModifier = 1;
        foreach (Mb_Player player in listOfUser)
        {
            player.state = Mb_Player.StateOfAction.Idle;
            player.ResetInteractionParameters();
        }
        listOfUser.Clear();

        finalTimeToSpendOn = trialParameters.timeToAccomplishTrial;
    }

    public void QuittingCheck()
    {
        if (listOfUser.Count != 0)
        {
            ReUpduateTiming();
        }
        else
            ResetValues();
    }

    public virtual bool CheckCondition()
    {
        return true;
    }
}
