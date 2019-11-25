using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ma_IAManager : MonoBehaviour
{
    public static Ma_IAManager Instance;

    public Transform HostagesContainer;
    //[HideInInspector]
    public Mb_IAAgent[] IAList;
    public List<Mb_Trial> HostageArea = new List<Mb_Trial>();

    public float repeatActionInterval = 3f;
    private float timer = 0;

    void Awake()
    {
        Instance = this;
        IAList = HostagesContainer.GetComponentsInChildren<Mb_IAAgent>();
    }

    private void Start()
    {
        foreach (Mb_IAAgent hostage in IAList)
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
        Mb_IAAgent hostage = h as Mb_IAAgent;
        Mb_Player player = p as Mb_Player;
        hostage.target = player;
        player.capturedHostages.Add(hostage);
        hostage.hostageState = HostageState.Captured;
    }

    public void StockHostagesInArea(Mb_HostageStockArea area, List<Mb_IAAgent> hostages)
    {
        List<Mb_IAAgent> stockedHosteges = new List<Mb_IAAgent>();
        foreach (Mb_IAAgent hostage in hostages)
        {
            foreach (Tile position in area.hostagesPos)
            {
                if (position.agentOnTile == null)
                {
                    List<Tile> pathToGo = hostage.pathfinder.SearchForShortestPath(hostage.AgentTile, new List<Tile> { position});
                    hostage.AddDeplacement(pathToGo);
                    hostage.hostageState = HostageState.Stocked;
                    position.agentOnTile = hostage;
                    hostage.target = null;
                    stockedHosteges.Add(hostage);
                    break;
                }
            }
        }
        foreach (Mb_IAAgent stockedHostege in stockedHosteges)
            hostages.Remove(stockedHostege);

    }
}
