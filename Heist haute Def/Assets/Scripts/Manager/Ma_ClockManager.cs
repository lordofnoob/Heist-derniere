using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ma_ClockManager : MonoBehaviour
{
    public static Ma_ClockManager Instance;

    public UnityEvent tickTrigger;
    public List<Mb_Agent> agentList = new List<Mb_Agent>();

    public float tickInterval = 0.2f;
    bool isPauseActive;

    IEnumerator clock;

    void Awake()
    {
        Instance = this;

        clock = Sequencer();
        if (tickTrigger == null)
            tickTrigger = new UnityEvent();

    }

    private void Start()
    {
        agentList.AddRange(Ma_PlayerManager.Instance.playerList);
        agentList.AddRange(Ma_IAManager.Instance.IAList);

        StartCoroutine(Sequencer());
    }

    IEnumerator Sequencer()
    {
       
        yield return new WaitForSeconds(tickInterval);

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
        if (Mb_InputController.inputControler.space == true)
        {
            isPauseActive = !isPauseActive;
        }
    }
}
