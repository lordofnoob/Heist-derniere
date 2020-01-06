using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ma_ClockManager : MonoBehaviour
{
    public static Ma_ClockManager instance;

    public UnityEvent tickTrigger;
    public List<Mb_Agent> agentList = new List<Mb_Agent>();

    public float tickInterval = 0.2f;
    bool isPauseActive;
    float timePassed = 0; 

    IEnumerator clock;

    void Awake()
    {
        instance = this;

        clock = Sequencer();
        if (tickTrigger == null)
            tickTrigger = new UnityEvent();

    }

    private void Start()
    {
        agentList.AddRange(Ma_PlayerManager.instance.playerList);
        agentList.AddRange(Ma_IAManager.Instance.IAList);

        StartCoroutine(Sequencer());
    }

    IEnumerator Sequencer()
    {
       
        yield return new WaitForSeconds(tickInterval);
        timePassed += tickInterval;
        Ma_LevelManager.instance.CheckTimeObjective(timePassed);
        if (isPauseActive == false)
        {
            tickTrigger.Invoke();
            PerformAgentActions();
        }

        StartCoroutine(Sequencer());
        
    }

    public void PerformAgentActions()
    {
        foreach(Mb_Agent agent in agentList)
        {
            agent.PerformAction();
        }
    }

    public void Update()
    {
        if (Mb_InputController.inputControler.space == true && Ma_LevelManager.instance.levelFinished == false)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPauseActive = !isPauseActive;
    }
}
