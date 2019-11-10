using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public static IAManager Instance;

    public List<Mb_IAHostage> IAList = new List<Mb_IAHostage>();
    public List<Mb_Trial> HostageArea = new List<Mb_Trial>();

    public float repeatActionInterval = 3f;
    private float timer = 0;

    void Awake()
    {
        Instance = this;
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
        if(IAList.Count != 0)
            UpdateHostageStress();
    }

    public void IAHostageFollowingPlayer(Mb_IAHostage hostage, Mb_Player p)
    {
        hostage.target = p;
        p.capturedHostages.Add(hostage);
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

    void UpdateHostageStress()
    {
        foreach(Mb_IAHostage hostage in IAList)
        {
            hostage.stress += Random.Range(0.01f, 0.1f) * Time.deltaTime;
        }
    }
}
