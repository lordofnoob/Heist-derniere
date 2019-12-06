using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mb_Trial : Mb_Poolable
{
    [Header("Parameters")]
    public Sc_TrialDefinition trialParameters;
    public Tile[] positionToGo;
    public Tile trialTile;

    [Header("Interface")]
    public Image timeVignet;
    public scr_UITween UiToTrigger;
    //A rechanger en private
    public List<Mb_Agent> listOfUser= new List<Mb_Agent>();


    public float currentTimeSpentOn=0;
    private float finalTimeToSpendOn=1;
    private bool counting;
    private List<float> reductionList;
    private float definitiveModifier=1;
    private float vignetCompletion = 0;
    float tickInterval;

    private void OnEnable()
    {
        currentTimeSpentOn = 0;
        finalTimeToSpendOn = trialParameters.timeToAccomplishTrial;
    }

   public virtual void Awake()
    { 
        Ma_ClockManager.instance.tickTrigger.AddListener(this.Counting);
        tickInterval = Ma_ClockManager.instance.tickInterval;
    }

     public void StartInteracting()
     {

        if (CheckCondition())
        {
            foreach (Mb_Agent agent in listOfUser)
            {
                if(agent is Mb_Player)
                {
                    Mb_Player player = agent as Mb_Player;
                    int length = player.charaPerks.characterSkills.Length;
                    for (int i = 0; i < player.charaPerks.characterSkills.Length; i++)
                        for (int y = 0; y < trialParameters.skillToUse.Length; y++)
                            if (player.charaPerks.characterSkills[i] == trialParameters.skillToUse[y].associatedSkill)
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
        }

        UiToTrigger.finalTimeToSpendOn = trialParameters.timeToAccomplishTrial* definitiveModifier;

        UiToTrigger.fill = true;
        transform.GetComponent<TriggerPanelEvent>().filling = true;
        /*currentTimeSpentOn = 0;
        counting = true;*/
    }

    public void Counting()
    {
        if (counting == true)
        {
            currentTimeSpentOn += tickInterval;
        }

        if (currentTimeSpentOn > finalTimeToSpendOn)
        {
            DoThings();
            currentTimeSpentOn = 0;
        }
    }

    public void ReUpduateTiming()
    {
        definitiveModifier = 1;
            foreach (Mb_Player player in listOfUser)
            {
                Debug.Log(player.charaPerks);
                int length = player.charaPerks.characterSkills.Length;
                Debug.Log(length);
                for (int i = 0; i < player.charaPerks.characterSkills.Length; i++)
                    for (int y = 0; y < trialParameters.skillToUse.Length; y++)
                        if (player.charaPerks.characterSkills[i] == trialParameters.skillToUse[y].associatedSkill)
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
            }/*
            foreach (Mb_IAAgent agent in listOfUser)
            {
                int length = agent.aiCharacteristics.characterSkills.Length;
                for (int i = 0; i < agent.aiCharacteristics.characterSkills.Length; i++)
                    for (int y = 0; y < trialParameters.skillToUse.Length; y++)
                        if (agent.aiCharacteristics.characterSkills[i] == trialParameters.skillToUse[y].associatedSkill)
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
            }*/
        UiToTrigger.finalTimeToSpendOn = trialParameters.timeToAccomplishTrial * definitiveModifier;
        transform.GetComponent<TriggerPanelEvent>().filling = true;
    }


    public virtual void DoThings()
    {
        listOfUser.Clear();
    }

    public void ResetValues()
    {
        //Debug.Log("RESET VALUE");
        UiToTrigger.RestatPos();
        vignetCompletion = 0;
        currentTimeSpentOn = 0;
        definitiveModifier = 1;
        //Debug.Log(listOfUser.Count);
        foreach (Mb_Agent agent in listOfUser)
        {
            agent.ResetInteractionParameters();
        }
        listOfUser.Clear();

        UiToTrigger.finalTimeToSpendOn = trialParameters.timeToAccomplishTrial;
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
