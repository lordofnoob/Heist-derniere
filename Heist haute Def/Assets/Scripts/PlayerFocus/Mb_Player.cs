﻿using System.Collections;
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

    public Sc_Charaspec characterProperty;
    //[SerializeField] NavMeshAgent agent;
    public Color highlightedColor, selectedColor;

    [Header("Hostage")]
    public List<Mb_IAHostage> capturedHostages = new List<Mb_IAHostage>();


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
                    ModifyOutlines(Outlines.Mode.OutlineAll, selectedColor, 7.5f);
                    SetOutlinesEnabled(true);
                    break;
                case false:
                    isSelected = false;
                    SetOutlinesEnabled(false);
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
        ModifyOutlines(Outlines.Mode.OutlineVisible, highlightedColor, 7.5f);
        SetOutlinesEnabled(true);
    }

    void OnMouseExit()
    {
        if (IsSelected)
        {
            ModifyOutlines(Outlines.Mode.OutlineVisible, selectedColor, 7.5f);
        }
        else
        {
            SetOutlinesEnabled(false);
        }
        highlighted = false;
    }

    void ModifyOutlines(Outlines.Mode mode, Color color, float width)
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
    }

    public override void AddDeplacement(List<Tile> path)
    {
        //Debug.Log(path.Count);
        state = StateOfAction.Moving;
        destination = path[path.Count - 1];
        foreach(Tile tile in path)
        {
            actionsToPerform.Add(new Deplacement(characterProperty.speed, this, tile));
        }
        //Debug.Log(actionsToPerform.Count);

        //uniquement pour la next interaction n influe pas sur le deplacement whatsoever
        //positionToGo = endPos;
    }

    public void ChangeDeplacement(List<Tile> newPath)
    {
        //REMOVE OLDER DEPLACEMENT ACTION
        List<Deplacement> removeList = new List<Deplacement>();
        foreach (Action action in actionsToPerform)
        {
            if (action is Deplacement)
                removeList.Add(action as Deplacement);
        }

        List<Action> actionList = actionsToPerform;
        actionsToPerform.Clear();
        
        foreach(Tile tile in newPath)
        {
            actionsToPerform.Add(new Deplacement(characterProperty.speed, this, tile));
        }

        actionsToPerform.AddRange(actionList);
    }

    public override void FindAnOtherPath()
    {
        List<Tile> newShortestPath = new List<Tile>();
        if (!destination.avaible)
        {
            newShortestPath = pathfinder.SearchForShortestPath(agentTile, destination.GetFreeNeighbours());
        }
        else
        {
            newShortestPath = pathfinder.SearchForShortestPath(agentTile, new List<Tile> { destination });
        }
        Debug.Log("New path deplacement number : " + newShortestPath.Count);
        ChangeDeplacement(newShortestPath);
        nextAction = true;
    }

    public override void PerformAction()
    {
        if(actionsToPerform.Count != 0 && nextAction)
        {
            if(onGoingInteraction != null && onGoingInteraction.state == StateOfAction.Moving)
            {
                List<Tile> posToGo = new List<Tile>();
                for (int i = 0; i < onGoingInteraction.positionToGo.Length; i++)
                {
                    posToGo.Add(onGoingInteraction.positionToGo[i]);
                }
                List<Tile> newPath = pathfinder.SearchForShortestPath(agentTile, posToGo);
                ChangeDeplacement(newPath);
            }
            nextAction = false;
            actionsToPerform.First().PerformAction();
            actionsToPerform.Remove(actionsToPerform.First());
        }
    }

    public override void Interact()
    {
        //Debug.Log("INTERACT");
        state = StateOfAction.Interacting;
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
    }

    public override void ResetInteractionParameters()
    {
        state = StateOfAction.Idle;
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

    public override void SetNextInteraction(Mb_Trial trialToUse)
    {
        onGoingInteraction = trialToUse;
        actionsToPerform.Add(new Interact(trialToUse.trialParameters.timeToAccomplishTrial, this, trialToUse));
    }
}
