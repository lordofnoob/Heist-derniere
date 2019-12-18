using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Mb_IAAgent : Mb_Agent
{
    [HideInInspector] public Mb_IATrial IATrial;

    [Header("Hostage Infos")]
    public float normalSpeed;
    public float panicSpeed;
    public Sc_AiSpecs aiCharacteristics;

    [Header("Stress Infos")]
    public float stress = 0f; //Percentage
    [Tooltip("Les bornes de la valeur aléatoire d'augmentation de stress/tick (A CHANGER)")]
    public float minStress, maxStress;
    public Image stressBar;
    [HideInInspector] public int panicCounter = 0;
    private int idleType = 0;

    [Header("Hostage target")]
    public Mb_Player target;

    [Header("Hostage State")]
    public HostageState hostageState = HostageState.Free;
    //[HideInInspector]
    public Mb_Agent SomeoneWillInteractWith = null;

    public override void Awake()
    {
        base.Awake();
        IATrial = GetComponent<Mb_IATrial>();
    }

    void Start()
    {
        UpdatePositionToGo();
    }

    public void IncreaseStress()
    {
        switch (hostageState)
        {
            case HostageState.Free:
                stress += Random.Range(minStress, maxStress);
                stress = Mathf.Clamp(stress, 0, 100);
                break;
            case HostageState.Captured:
                stress += Random.Range(minStress, maxStress) / 2;
                stress = Mathf.Clamp(stress, 0, 40);
                break;
            case HostageState.Stocked:
                stress += Random.Range(minStress, maxStress) / 2;
                stress = Mathf.Clamp(stress, 0, 100);
                break;
        }
        //Debug.Log("Stress : "+stress);

        if (stress == 100 && hostageState != HostageState.InPanic && hostageState != HostageState.Captured)
        {
            Panic();
        }
        else
        {
            if (stress < 25)
            {
                idleType = 0;
                animator.SetFloat("IdleValue", 0);
            }
            else if (stress >= 25 && stress < 75)
            {
                idleType = 1;
                animator.SetFloat("IdleValue", 5);
            }
            else if(stress > 75)
            {
                idleType = 2;
                animator.SetFloat("IdleValue", 10);
            }
        }
    }
    public void Panic()
    {
        hostageState = HostageState.InPanic;
        panicCounter++;

        //OLD
        /*List<Tile> posToExit = new List<Tile>();
        foreach (Tile exitTile in Ma_LevelManager.Instance.allExitTile)
        {
            posToExit.Add(exitTile);
        }
        List<Tile> pathToNearestExitDoor = pathfinder.SearchForShortestPath(AgentTile, posToExit, true);
        AddDeplacement(pathToNearestExitDoor);*/
        //Debug.Log(actionsToPerform.Count);

        //NEW
        //GoTo(warpHostageTrial);
    }

    public override void AddDeplacement(List<Tile> path)
    {
        if (path.Count != 0)
        {
            //Debug.Log(path.Count);
            foreach (Tile tile in path)
            {
                if (hostageState == HostageState.InPanic)
                    actionsToPerform.Add(new Deplacement(panicSpeed, this, tile));
                else
                    actionsToPerform.Add(new Deplacement(normalSpeed, this, tile));
            }
            //Debug.Log(actionsToPerform.Count);
        }
    }

    public override void PerformAction()
    {
        if (SomeoneWillInteractWith != null)
        {
            foreach (Tile neighbour in GetAgentTile().GetNeighbours())
            {
                //Debug.Log("Neighbour");
                if (neighbour.agentOnTile != null && neighbour.agentOnTile == SomeoneWillInteractWith)
                {
                    StopMoving();
                    return;
                }
            }
        }else if(target != null && target.GetActionState() == StateOfAction.Moving)
        {
            Debug.Log("Target.GetAgentTile : "+target.GetAgentTile());
            GoTo(target.GetAgentTile());
        }

        //Debug.Log("about to perform action. Count left = " + actionsToPerform.Count.ToString());
        if (actionsToPerform.Count != 0 && nextAction)
        {
            if(actionsToPerform.First() is Deplacement)
            {
                Deplacement depla = actionsToPerform.First() as Deplacement;
                if(depla.destination.cost > Ma_ClockManager.instance.tickInterval)
                {
                    Debug.Log("WAIT");
                    List<Action> temp = actionsToPerform;
                    actionsToPerform.Clear();
                    actionsToPerform.Add(new Wait(1, this, FindAnOtherPath));
                    actionsToPerform.AddRange(temp);
                }
            }

            
            Debug.Log("##### ACTUAL ACTIONS TO PERFORM #####");
            foreach (Action action in actionsToPerform)
            {
                if(action is Deplacement)
                {
                    Deplacement deplacement = action as Deplacement;
                    Debug.Log("First Action is : " + deplacement + "(" + deplacement.destination.name + ")");
                }
                else
                {
                    Debug.Log("First Action is : " + action);
                }
            }
            Debug.Log("##############################");
            

            nextAction = false;
            actionsToPerform.First().PerformAction();

            actionsToPerform.Remove(actionsToPerform.First());
        }
    }

    public override void FindAnOtherPath()
    {        
        //Debug.Log("IA Find a new path");

        if (onGoingInteraction)
        {
            GoTo(onGoingInteraction);
        }
        else
        {
            GoTo(destination);
        }        

        nextAction = true;
    }

    public override void Interact()
    {
        //Debug.Log("INTERACTING");
        SetNewActionState(StateOfAction.Interacting);

        if (onGoingInteraction.listOfUser.Count == 0)
        {
            onGoingInteraction.listOfUser.Add(this);
            onGoingInteraction.StartInteracting();
        }
        else
            for (int i = 0; i < onGoingInteraction.listOfUser.Count; i++)
            {
                if (onGoingInteraction.listOfUser[i] != this)
                {
                    onGoingInteraction.listOfUser.Add(this);
                    onGoingInteraction.ReUpduateTiming();
                }
            }
        base.Interact();
    }

    public override void SetNextInteraction()
    {
        if (trialsToGo.Count > 0)
        {
            onGoingInteraction = trialsToGo.First();
            actionsToPerform.Add(new Interact(onGoingInteraction.trialParameters.timeToAccomplishTrial, this, onGoingInteraction));
        }
    }

    public override void SetFirstActionToPerform(Action action)
    {
        List<Action> temp = actionsToPerform;
        actionsToPerform.Clear();
        actionsToPerform.Add(action);
        actionsToPerform.AddRange(temp);
        //Debug.Log(actionsToPerform[0]);
    }

    public override void ResetInteractionParameters()
    {
        SetNewActionState(StateOfAction.Idle);
        SomeoneWillInteractWith = null;
        onGoingInteraction = null;
        nextAction = true;
        //Debug.Log(actionsToPerform.Count);
    }

    public void StopMoving()
    {
        //Debug.Log("STOP MOVING");
        SetNewActionState(StateOfAction.Idle);
        actionsToPerform.Clear();
        actionsToPerform.TrimExcess();
        SomeoneWillInteractWith = null;
        onGoingInteraction = null;
        destination = null;
        //Debug.Log("actions have been flushed. Count left = " + actionsToPerform.Count.ToString());
    }

    public void UpdatePositionToGo()
    {
        IATrial.positionToGo = GetAgentTile().GetFreeNeighbours().ToArray();
    }

    public override void SetNewActionState(StateOfAction agentState)
    {
        //Debug.Log(agentState);
        base.SetNewActionState(agentState);
        switch (idleType)
        {
            case 0:
                if(agentState == StateOfAction.Moving)
                {
                    animator.SetBool("Idle00_To_Move", true);
                    animator.SetFloat("Speed", 10); //LERP
                }
                else if(agentState == StateOfAction.Idle)
                {
                    animator.SetFloat("Speed", 0);//LERP
                    animator.SetBool("Idle00_To_Move", false);
                }
                break;
            case 1:
                if(agentState == StateOfAction.Moving)
                {
                    animator.SetBool("Idle01_To_Move", true);
                    animator.SetFloat("Speed", 10);//LERP
                }
                else if(agentState == StateOfAction.Idle)
                {
                    animator.SetFloat("Speed", 0);//LERP
                    animator.SetBool("Idle01_To_Move", false);
                }
                break;
            case 2:
                if(agentState == StateOfAction.Moving)
                {
                    animator.SetBool("Idle02_To_Move", true);
                    animator.SetFloat("Speed", 10);//LERP
                }
                else if(agentState == StateOfAction.Idle)
                {
                    animator.SetFloat("Speed", 0);//LERP
                    animator.SetBool("Idle02_To_Move", false);
                }
                break;
        }
    }

    public void SetupTheMovementValues()
    {
        normalSpeed = aiCharacteristics.normalSpeed;
        panicSpeed = aiCharacteristics.fleeingSpeed;
    }
    //IEnumerator 
}
