using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Agent : Mb_Poolable
{
    //[HideInInspector] 
    public Pathfinder pathfinder;
    [HideInInspector] public List<Tile> VisitedTiles = new List<Tile>();

    public Tile agentTile;

    [Header("Actions")]
    public StateOfAction state;
    public List<Action> actionsToPerform = new List<Action>();
    [HideInInspector] public Mb_Trial onGoingInteraction;
    public Tile destination;
    public bool nextAction = true;

    public void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    public virtual void PerformAction() { }
    public virtual void AddDeplacement(List<Tile> path) { }
    public virtual void FindAnOtherPath() { }
    public virtual void Interact() { }
    public virtual void SetNextInteraction(Mb_Trial trialToUse) { }
    public virtual void AddItem(Sc_Items itemToAdd) { }
    public virtual void DropItem(Sc_Items itemToDrop) { }

}
