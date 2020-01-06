using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mb_Agent : Mb_Poolable
{

  

    [Header("Animator infos")]
    public Animator animator;

    //[HideInInspector] 
    public Pathfinder pathfinder;

    [HideInInspector] public List<Tile> VisitedTiles = new List<Tile>();

    [SerializeField]private Tile agentTile;

    [Header("Actions")]
    [SerializeField] public StateOfAction state;
    public List<Action> actionsToPerform = new List<Action>();
    [HideInInspector] public Mb_Trial onGoingInteraction;
    [HideInInspector] public List<Mb_Trial> trialsToGo;
    public Tile destination;
    public bool nextAction = true;

    public virtual void Awake()
    {
        if(GetComponent<Pathfinder>() != null)
            pathfinder = GetComponent<Pathfinder>();
    }

    public virtual void SetAgentTile(Tile newAgentTile, bool isSwitchingTile = false)
    {
        if (!isSwitchingTile)
        {
            if(agentTile != null)
            {
                agentTile.avaible = true;
                agentTile.agentOnTile = null;
            }
        }
        else
        {
            Debug.Log("Switch tile");
            agentTile.avaible = false;
            agentTile.agentOnTile = newAgentTile.agentOnTile;
            newAgentTile.agentOnTile.SetAgentTile(agentTile);
        }

        //Debug.Log("Set Agent Tile");
        agentTile = newAgentTile;

        newAgentTile.avaible = false;
        newAgentTile.agentOnTile = this;
    }

    public Tile GetAgentTile()
    {
        return agentTile;
    }

    public virtual void PerformAction() { }
    public void GoTo(Tile tileDestination = null)
    {
        List<Tile> newPath = new List<Tile>();
        if (!tileDestination.avaible)
        {
            //newPath = pathfinder.SearchForShortestPath(GetAgentTile(), tileDestination.GetFreeNeighbours());
            GoTo(tileDestination.GetFreeNeighbours());
            return;
        }
        else
        {
            newPath = pathfinder.SearchForShortestPath(GetAgentTile(), new List<Tile> { tileDestination });
        }

        destination = newPath[newPath.Count - 1];
        //Debug.Log("Path count : " + newPath.Count);
        ChangeDeplacement(newPath);
        nextAction = true;
    }

    public void GoTo(List<Tile> listOfTileDestination = null)
    {
        foreach(Tile tile in listOfTileDestination)
        {
            if (tile.avaible)
            {
                GoTo(tile);
                return;
            }
        }
    }

    public void GoTo(Mb_Trial nextTrialToInteract)
    {
        trialsToGo.Add(nextTrialToInteract);
        List<Tile> possibleDestination = new List<Tile>();
        if (nextTrialToInteract.listOfUser.Count >= nextTrialToInteract.positionToGo.Length)
        {
            for (int i = 0; i < nextTrialToInteract.listOfUser.Count; i++)
            {
                if (nextTrialToInteract.listOfUser[i] == null)
                {
                    possibleDestination.Add(nextTrialToInteract.positionToGo[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < nextTrialToInteract.positionToGo.Length; i++)
            {
                possibleDestination.Add(nextTrialToInteract.positionToGo[i]);
            }
        }

        if (possibleDestination.Count == 0)
        {
            Debug.Log("DEPLACEMENT IMPOSSIBLE");
            return;
        }

        List<Tile> newPath = pathfinder.SearchForShortestPath(GetAgentTile(), possibleDestination);
        destination = newPath[newPath.Count - 1];
        ChangeDeplacement(newPath);
        SetNextInteraction();
        nextAction = true;
    }
    public virtual void AddDeplacement(List<Tile> path) { }
    public virtual void ChangeDeplacement(List<Tile> newPath)
    {
        //Debug.Log("CHANGE DEPLACEMENT");
        //REMOVE OLDER DEPLACEMENT ACTION
        List<Action> actionList = new List<Action>();
        foreach (Action action in actionsToPerform)
        {
            if (!(action is Deplacement))
                actionList.Add(action);
        }

        actionsToPerform.Clear();

        AddDeplacement(newPath);

        actionsToPerform.AddRange(actionList);

        /*Debug.Log("##### ACTUAL ACTIONS TO PERFORM #####");
        foreach (Action action in actionsToPerform)
        {
            Debug.Log("First Action is : " + action);
        }
        Debug.Log("##############################");*/
    }

    public virtual void FindAnOtherPath() { }
    public virtual void Interact()
    {
        trialsToGo.Remove(trialsToGo.First());
        if(trialsToGo.Count > 0)
        {
            onGoingInteraction = trialsToGo.First();
        }
        else
        {
            onGoingInteraction = null;
        }
    }
    //Put Action at the end of actionToPerform queue
    public virtual void SetNextInteraction() { }
    //Put Action at the Starrt of actionToPerform queue
    public virtual void SetFirstActionToPerform(Action action) { }
    public virtual void ResetInteractionParameters() { }
    public virtual void AddItem(Sc_Items itemToAdd) { }
    public virtual void DropItem(Sc_Items itemToDrop) { }
    public virtual void SetNewActionState(StateOfAction agentState)
    {
        state = agentState;
    }
    public StateOfAction GetActionState()
    {
        return state;
    }

    public IEnumerator WaitForTime(float tickToWait)
    {
        yield return new WaitForSeconds(tickToWait * Ma_ClockManager.instance.tickInterval);
        GoTo(destination);
        nextAction = true;
    }
}
