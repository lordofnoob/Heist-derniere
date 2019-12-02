using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ma_IAManager : MonoBehaviour
{
    public static Ma_IAManager Instance;

    [Header("For Debuging")]
    public bool activateHostageStress = true;

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
        if (activateHostageStress)
            foreach (Mb_IAAgent hostage in IAList)
            {
                Ma_ClockManager.Instance.tickTrigger.AddListener(hostage.IncreaseStress);
            }
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
        List<Mb_IAAgent> stockedHostages = new List<Mb_IAAgent>();
        foreach (Mb_IAAgent hostage in hostages)
        {
            foreach (Tile position in area.hostagesPos)
            {
                if (position.agentOnTile == null)
                {
                    List<Tile> pathToGo = hostage.pathfinder.SearchForShortestPath(hostage.AgentTile, new List<Tile> { position});
                    Debug.Log("Deplacement to stock pos : " + pathToGo.Count);
                    hostage.AddDeplacement(pathToGo);
                    hostage.hostageState = HostageState.Stocked;
                    position.agentOnTile = hostage;
                    hostage.target = null;
                    area.SetStockedHostageNumber(area.GetStockedHostageNumber() + 1);
                    stockedHostages.Add(hostage);
                    break;
                }
            }
        }

        area.stockedHostage.AddRange(stockedHostages);

        foreach (Mb_IAAgent stockedHostege in stockedHostages)
            hostages.Remove(stockedHostege);

    }
}
