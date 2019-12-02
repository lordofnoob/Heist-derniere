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

        List<Tile> posToExit = new List<Tile>();
        foreach (Tile exitTile in Ma_LevelManager.Instance.allExitTile)
        {
            posToExit.Add(exitTile);
        }
        List<Tile> pathToNearestExitDoor = pathfinder.SearchForShortestPath(AgentTile, posToExit, true);
        AddDeplacement(pathToNearestExitDoor);
        //Debug.Log(actionsToPerform.Count);
    }

    public override void AddDeplacement(List<Tile> path)
    {
        if (path.Count != 0)
        {
            //Debug.Log(path.Count);
            destination = path[path.Count - 1];
            foreach (Tile tile in path)
            {
                if (hostageState == HostageState.InPanic)
                    actionsToPerform.Add(new Deplacement(panicSpeed, this, tile));
                else
                    actionsToPerform.Add(new Deplacement(normalSpeed, this, tile));
            }
            //Debug.Log(actionsToPerform.Count);
        }
        else
        {
            Debug.Log("Wait a tick");
            SetFirstActionToPerform(new Wait(1f, this, this.FindAnOtherPath));
            FindAnOtherPath();
           
        }
    }

    public override void PerformAction()
    {
        if (SomeoneWillInteractWith != null)
        {
            foreach (Tile neighbour in AgentTile.GetNeighbours())
            {
                //Debug.Log("Neighbour");
                if (neighbour.agentOnTile != null && neighbour.agentOnTile == SomeoneWillInteractWith)
                {
                    StopMoving();
                    return;
                }
            }
        }

        //Debug.Log("about to perform action. Count left = " + actionsToPerform.Count.ToString());
        if (actionsToPerform.Count != 0 && nextAction)
        {
            /*if(actionsToPerform.First() is Interact)
                Debug.Log("Perform Interaction");*/
            //Debug.Log("PERFORM 1 IAHOSTAGE ACTION");
            nextAction = false;
            actionsToPerform.First().PerformAction();

            if (destination == AgentTile)
            {
                SetNewActionState(StateOfAction.Idle);
                destination = null;
            }
            UpdatePositionToGo();

            actionsToPerform.Remove(actionsToPerform.First());

        }
    }

    public override void FindAnOtherPath()
    {
        if (GetActionState() == StateOfAction.Moving)
        {
            Debug.Log("IA Find a new path");

            List<Deplacement> removeList = new List<Deplacement>();
            foreach (Action action in actionsToPerform)
            {
                if (action is Deplacement)
                    removeList.Add(action as Deplacement);
            }

            foreach (Deplacement depla in removeList)
                actionsToPerform.Remove(depla);

            List<Tile> newShortestPath = new List<Tile>();
            if (!destination.avaible)
            {
                newShortestPath = pathfinder.SearchForShortestPath(AgentTile, destination.GetFreeNeighbours());
            }
            else
            {
                newShortestPath = pathfinder.SearchForShortestPath(AgentTile, new List<Tile> { destination });
            }
            //Debug.Log("New path deplacement number : " + newShortestPath.Count);
            ChangeDeplacement(newShortestPath);
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
    }

    public override void SetNextInteraction(Mb_Trial trialToUse)
    {
        //Debug.Log("Interact");
        onGoingInteraction = trialToUse;
        actionsToPerform.Add(new Interact(trialToUse.trialParameters.timeToAccomplishTrial, this, trialToUse));
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
        Debug.Log("STOP MOVING");
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
        IATrial.positionToGo = AgentTile.GetFreeNeighbours().ToArray();
    }

    public override void SetNewActionState(StateOfAction agentState)
    {
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
