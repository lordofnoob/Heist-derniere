using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum HostageState
{
    Free,
    Captured,
    Stocked,
    InPanic
}

public class Mb_IAHostage : Mb_Trial
{
    [Header("Hostage Infos")]
    public float normalSpeed = 2;
    public float panicSpeed = 1;
    [Header("Stress Infos")]
    public float stress = 0f; //Percentage
    [Tooltip("Les bornes de la valeur aléatoire d'augmentation de stress/tick (A CHANGER)")]
    public float minStress, maxStress;
    public Image stressBar;
    [HideInInspector] public int panicCounter = 0;

    public float trialsAreaSize = 5f;

    [Header("Hostage State")]
    public HostageState hostageState = HostageState.Free;

    [Header("Hostage target")]
    public Mb_Player target;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePositionToGo();
    }

    // Update is called once per frame
    void Update()
    {
        Counting();
    }

    public void RandomMovement()
    {
        //agent.SetDestination(transform.position + new Vector3(Random.Range(1, trialsAreaSize), 0f, Random.Range(1, trialsAreaSize)));
    }

    public override void DoThings()
    {
        Debug.Log("TARGET CAPTURED");
        listOfUser[0].nextAction = true;
        Ma_IAManager.Instance.IAHostageFollowingPlayer(this, listOfUser[0]);
        
        ResetValues();
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
                stress += Random.Range(minStress, maxStress)/2;
                stress = Mathf.Clamp(stress, 40, 100);
                break;
        }
        //Debug.Log("Stress : "+stress);

        if (stress == 100 && hostageState != HostageState.InPanic)
        {
            Panic();
        }
    }

    public void Panic()
    {
        Debug.Log("PANIC!");
        hostageState = HostageState.InPanic;
        panicCounter++;

        List<Tile> posToExit = new List<Tile>();
        foreach(Mb_Door door in Ma_LevelManager.Instance.allExitDoors)
        {
            posToExit.AddRange(door.positionToGo);
        }
        List<Tile> pathToNearestExitDoor = pathfinder.SearchForShortestPath(agentTile, posToExit, true);
        AddDeplacement(pathToNearestExitDoor);
        //Debug.Log(actionsToPerform.Count);
    }

    public override void PerformAction()
    {
        Debug.Log("about to perform action. Count left = "+actionsToPerform.Count.ToString());
        if (actionsToPerform.Count != 0 && nextAction)
        {
            /*if(actionsToPerform.First() is Interact)
                Debug.Log("Perform Interaction");*/
            //Debug.Log("PERFORM 1 IAHOSTAGE ACTION");
            nextAction = false;
            actionsToPerform.First().PerformAction();
            actionsToPerform.Remove(actionsToPerform.First());

        }
    }

    public override void AddDeplacement(List<Tile> path)
    {
        if(path.Count != 0)
        {
            Debug.Log(path.Count);
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
            Debug.Log("COUCOU");
        }
    }

    public override void FindAnOtherPath()
    {
        bool playerWillInteractWith = false;
        foreach (Tile neighbour in agentTile.GetNeighbours())
        {
            if(neighbour.agentOnTile != null && neighbour.agentOnTile.onGoingInteraction == this)
            {
                playerWillInteractWith = true;
                break;
            }
        }

        if (!playerWillInteractWith)
        {
            if (state == StateOfAction.Moving)
            {
                Debug.Log("FIND ANOTHER PATH");

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
                    newShortestPath = pathfinder.SearchForShortestPath(agentTile, destination.GetFreeNeighbours());
                }
                else
                {
                    newShortestPath = pathfinder.SearchForShortestPath(agentTile, new List<Tile> { destination });
                }
                //Debug.Log("New path deplacement number : " + newShortestPath.Count);
                AddDeplacement(newShortestPath);
            }
        }
        else
        {
            Debug.Log("WILL INTERACT");
            StopMoving();
        }

        nextAction = true;
    }

    public void UpdatePositionToGo()
    {
        positionToGo = agentTile.GetFreeNeighbours().ToArray();
    }

    public override void SetNextInteraction(Mb_Trial trialToUse)
    {
        //Debug.Log("Interact");
        onGoingInteraction = trialToUse;
        actionsToPerform.Add(new Interact(trialToUse.trialParameters.timeToAccomplishTrial, this, trialToUse));
    }

    public override void SetFirstInteraction(Mb_Trial trialToUse)
    {
        onGoingInteraction = trialToUse;
        List<Action> temp = actionsToPerform;
        actionsToPerform.Clear();
        actionsToPerform.Add(new Interact(trialToUse.trialParameters.timeToAccomplishTrial, this, trialToUse));
        actionsToPerform.AddRange(temp);
        //Debug.Log(actionsToPerform[0]);
    }

    public override void Interact()
    {
        //Debug.Log("INTERACTING");
        state = StateOfAction.Interacting;

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

    public override void ResetInteractionParameters()
    {
        state = StateOfAction.Idle;
        onGoingInteraction = null;
        nextAction = true;
        //Debug.Log(actionsToPerform.Count);
    }

}
