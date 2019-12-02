using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Agent : Mb_Poolable
{

  

    [Header("Animator infos")]
    public Animator animator;

    [Header("Chara perks")]

    //[HideInInspector] 
    public Pathfinder pathfinder;

    [HideInInspector] public List<Tile> VisitedTiles = new List<Tile>();

    [SerializeField]private Tile agentTile;
    [SerializeField]public Tile AgentTile {
        get { return agentTile; }
        set
        {
            if(agentTile != null)
            {
                agentTile.avaible = true;
                agentTile.agentOnTile = null;
            }

            //Debug.Log("Set Agent Tile");
            agentTile = value;

            value.avaible = false;
            value.agentOnTile = this;
        }
    }

    [Header("Actions")]
    [SerializeField]private StateOfAction state;
    public List<Action> actionsToPerform = new List<Action>();
    [HideInInspector] public Mb_Trial onGoingInteraction;
    public Tile destination;
    public bool nextAction = true;

    public virtual void Awake()
    {
        if(GetComponent<Pathfinder>() != null)
            pathfinder = GetComponent<Pathfinder>();
    }

    public virtual void PerformAction() { }
    public virtual void AddDeplacement(List<Tile> path) { }
    public virtual void ChangeDeplacement(List<Tile> newPath)
    {
        Debug.Log("CHANGE DEPLACEMENT");
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
    public virtual void Interact() { }
    //Put Action at the end of actionToPerform queue
    public virtual void SetNextInteraction(Mb_Trial trialToUse) { }
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

    public IEnumerator WaitForTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        nextAction = true;
    }
}
