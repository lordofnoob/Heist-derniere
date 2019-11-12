using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ma_IAManager : MonoBehaviour
{
    public static Ma_IAManager Instance;

    public Transform HostagesContainer;
    //[HideInInspector]
    public Mb_IAHostage[] IAList;
    public List<Mb_Trial> HostageArea = new List<Mb_Trial>();

    public float repeatActionInterval = 3f;
    private float timer = 0;

    void Awake()
    {
        Instance = this;
        IAList = HostagesContainer.GetComponentsInChildren<Mb_IAHostage>();
    }

    private void Start()
    {
        foreach (Mb_IAHostage hostage in IAList)
        {
            Ma_ClockManager.Instance.tickTrigger.AddListener(hostage.IncreaseStress);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if(timer >= repeatActionInterval)
        {
            foreach(Sc_IAHostage IACharacter in IAList)
            {
                IACharacter.RandomMovement();
            }
            timer = 0f;
        }
        timer += Time.deltaTime;*/
        
    }

    public void IAHostageFollowingPlayer(Mb_Agent h, Mb_Agent p)
    {
        Mb_IAHostage hostage = h as Mb_IAHostage;
        Mb_Player player = p as Mb_Player;
        hostage.target = player;
        player.capturedHostages.Add(hostage);
        hostage.hostageState = HostageState.Captured;
    }

    public void StockHostagesInArea(Mb_HostageStockArea area, List<Mb_IAHostage> hostages)
    {
        List<Mb_IAHostage> stockedHosteges = new List<Mb_IAHostage>();
        foreach(Mb_IAHostage hostage in hostages)
        {
            foreach (Transform position in area.hostagesPos)
            {
                if (position.GetComponent<Mb_PositionCheck>().dispo)
                {
                    position.GetComponent<Mb_PositionCheck>().dispo = false;

                    hostage.hostageState = HostageState.Stocked;                    
                    hostage.target = null;
                    stockedHosteges.Add(hostage);
                    //hostage.agent.SetDestination(position.position);
                    //hostage.agent.stoppingDistance = 0f;
                    break;
                }
            }
        }
        foreach(Mb_IAHostage stockedHostege in stockedHosteges)
            hostages.Remove(stockedHostege);
        
    }
}
