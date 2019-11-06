using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class Mb_Player : Mb_Poolable {

    public Sc_Charaspec characterProperty;
    //[SerializeField] NavMeshAgent agent;
    public Color highlightedColor, selectedColor;

    [Header("Actions")]
    public List<Sc_Action> actionsToPerform = new List<Sc_Action>();
    [HideInInspector] public bool nextAction = true;
    [HideInInspector] public Tile destination = null;

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


    [HideInInspector] public Mb_Trial onGoingInteraction;


    public Tile playerTile;
    public StateOfAction state;
    // nextInteractionToussa
    private Vector3 positionToGo;
    float distanceRemaining;

    private void Start()
    {
        Ma_ClockManager.Instance.tickTrigger.AddListener(PerformAction);
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

    public void AddDeplacement(List<Tile> path)
    {
        state = StateOfAction.Moving;
        destination = path.Last();
        foreach(Tile tile in path)
        {
            actionsToPerform.Add(new Sc_Deplacement(characterProperty.speed, this, tile));
        }
        
        //uniquement pour la next interaction n influe pas sur le deplacement whatsoever
        //positionToGo = endPos;
    }

    public void PerformAction()
    {
        if(actionsToPerform.Count != 0 && nextAction)
        {
            actionsToPerform[0].PerformAction();
            nextAction = false;
            actionsToPerform.RemoveAt(0);
        }
    }

    public void Interact()
    {
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

    public void ResetInteractionParameters()
    {
        onGoingInteraction = null;
        distanceRemaining = 0;
        positionToGo = transform.position;
    }

    void CheckingDistance()
    {
        // /!\ TO CHANGE
        /*if (Vector3.Distance(transform.position, positionToGo) <= agent.stoppingDistance && onGoingInteraction != null && state == StateOfAction.Moving)
        {            
                Interact();
        }*/
    }

    public void AddItem(Sc_Items itemToAdd)
    {
        itemsHold.Add(itemToAdd);
    }

    public void DropItem(Sc_Items itemToDrop)
    {
        itemsHold.Remove(itemToDrop);
    }

    public void SetNextInteraction(Mb_Trial trialToUse)
    {
        onGoingInteraction = trialToUse;
    }

    public enum StateOfAction
    {
        Moving, Interacting, Captured, Idle
    }
}
