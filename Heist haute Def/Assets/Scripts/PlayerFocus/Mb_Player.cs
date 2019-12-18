using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public enum StateOfAction
{
    Idle, Interacting, Captured, Moving
}


public class Mb_Player : Mb_Agent 
{

    //[SerializeField] NavMeshAgent agent;
    public Color highlightedColor, selectedColor;

    public Sc_PlayerSpecs charaPerks;

    [Header("Hostage")]
    public List<Mb_IAAgent> capturedHostages = new List<Mb_IAAgent>();


    [Header("Items")]
    public List<Sc_Items> itemsHold = new List<Sc_Items>();
    [HideInInspector] public bool highlighted = false;
    [HideInInspector] [SerializeField] private bool isSelected = false;

    [HideInInspector] public bool IsSelected { 
        set
        {
            switch (value)
            {
                case true:
                    isSelected = true;
                    //ModifyOutlines(Outlines.Mode.OutlineAll, selectedColor, 7.5f);
                    //SetOutlinesEnabled(true);
                    break;
                case false:
                    isSelected = false;
                    //SetOutlinesEnabled(false);
                    break;
            }
        }
        get
        {
            return isSelected;
        }
    }

    void Update () 
    {
        CheckingDistance();
    }

    void OnMouseEnter()
    {
        highlighted = true;
        //ModifyOutlines(Outlines.Mode.OutlineVisible, highlightedColor, 7.5f);
        //SetOutlinesEnabled(true);
    }

    void OnMouseExit()
    {
        if (IsSelected)
        {
            //ModifyOutlines(Outlines.Mode.OutlineVisible, selectedColor, 7.5f);
        }
        else
        {
            //SetOutlinesEnabled(false);
        }
        highlighted = false;
    }

    /*void ModifyOutlines(Outlines.Mode mode, Color color, float width)
    {
        Outlines outline = gameObject.GetComponent<Outlines>();
        outline.OutlineMode = mode;
        outline.OutlineColor = color;
        outline.OutlineWidth = width;
    }

    void SetOutlinesEnabled(bool enabled)
    {
        Outlines outline = gameObject.GetComponent<Outlines>();
        outline.enabled = enabled;
    }*/

    public override void AddDeplacement(List<Tile> path)
    {
        if (path.Count != 0)
        {
            //Debug.Log(path.Count);
            foreach (Tile tile in path)
            {
                actionsToPerform.Add(new Deplacement(charaPerks.speed, this, tile));
            }
            //Debug.Log(actionsToPerform.Count);

            //uniquement pour la next interaction n influe pas sur le deplacement whatsoever
            //positionToGo = endPos;
        }
        else
        {
            Debug.Log("Chemin Impossible");
            
            FindAnOtherPath();
        }
    }

    public override void FindAnOtherPath()
    {
        List<Tile> newShortestPath = new List<Tile>();
        if(destination == null)
        {
            if(onGoingInteraction != null)
            {
                List<Tile> posToGo = new List<Tile> ();
                posToGo.AddRange(onGoingInteraction.positionToGo);

                newShortestPath = pathfinder.SearchForShortestPath(GetAgentTile(), posToGo);
            }
            else
            {
                if (!destination.avaible)
                {
                    newShortestPath = pathfinder.SearchForShortestPath(GetAgentTile(), destination.GetFreeNeighbours());
                }
                else
                {
                    newShortestPath = pathfinder.SearchForShortestPath(GetAgentTile(), new List<Tile> { destination });
                }
            }
            Debug.Log("New path deplacement number : " + newShortestPath.Count);
            ChangeDeplacement(newShortestPath);
        }
        nextAction = true;
    }

    public override void PerformAction()
    {
        if(actionsToPerform.Count != 0 && nextAction)
        {
            /*
            Debug.Log("##### ACTUAL ACTIONS TO PERFORM #####");
            foreach (Action action in actionsToPerform)
            {
                if(action is Deplacement)
                {
                    Deplacement depla = action as Deplacement;
                    Debug.Log("Deplacement to : " + depla.destination);
                }
                else
                {
                    Debug.Log("Action : "+ action);
                }
            }
            Debug.Log("##############################");
            */

            //CHECK Si la prochaine interaction est un hotage en movement => alors recalcul du path
            if (onGoingInteraction != null && onGoingInteraction is Mb_IATrial)
            {
                Mb_IATrial IATrial = onGoingInteraction as Mb_IATrial;
                if(IATrial.iaAgent.GetActionState() == StateOfAction.Moving)
                {
                    List<Tile> posToGo = new List<Tile>();
                    for (int i = 0; i < onGoingInteraction.positionToGo.Length; i++)
                    {
                        posToGo.Add(onGoingInteraction.positionToGo[i]);
                    }
                    List<Tile> newPath = pathfinder.SearchForShortestPath(GetAgentTile(), posToGo);
                    ChangeDeplacement(newPath);
                }
            }

            nextAction = false;
            actionsToPerform.First().PerformAction();

            actionsToPerform.Remove(actionsToPerform.First());
        }
    }

    public override void Interact()
    {
        //Debug.Log("INTERACT");
        SetNewActionState(StateOfAction.Interacting);
        if (onGoingInteraction.listOfUser.Count==0)
        {
            onGoingInteraction.listOfUser.Add(this);
            onGoingInteraction.StartInteracting();
        }
        else
            for (int i =0; i<onGoingInteraction.listOfUser.Count; i++)
            {
                if (onGoingInteraction.listOfUser[i] != this)
                {
                    onGoingInteraction.listOfUser.Add(this);
                    onGoingInteraction.ReUpduateTiming();
                }
            }
        base.Interact();
    }

    public override void ResetInteractionParameters()
    {
        SetNewActionState(StateOfAction.Idle);
        onGoingInteraction = null;
        nextAction = true;
    }

    void CheckingDistance()
    {
        // /!\ TO CHANGE
        /*if (Vector3.Distance(transform.position, positionToGo) <= agent.stoppingDistance && onGoingInteraction != null && state == StateOfAction.Moving)
        {            
                Interact();
        }*/
    }

    public override void AddItem(Sc_Items itemToAdd)
    {
        itemsHold.Add(itemToAdd);
    }

    public override void DropItem(Sc_Items itemToDrop)
    {
        itemsHold.Remove(itemToDrop);
    }

    public override void SetNextInteraction()
    {
        if(trialsToGo.Count > 0)
        {
            onGoingInteraction = trialsToGo.First();
            actionsToPerform.Add(new Interact(onGoingInteraction.trialParameters.timeToAccomplishTrial, this, onGoingInteraction));
        }
    }

    public override void SetNewActionState(StateOfAction agentState)
    {
        //Debug.Log(agentState);
        base.SetNewActionState(agentState);

        if (agentState == StateOfAction.Moving)
        {
            animator.SetBool("Idle00_To_Move", true);
            animator.SetFloat("Speed", 8.5f);
        }
        else if(agentState == StateOfAction.Idle)
        {
            animator.SetBool("Idle00_To_Move", false);
            animator.SetFloat("Speed", 0);
        }
    }
}
