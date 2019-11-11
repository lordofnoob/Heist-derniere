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
        if (hostageState == HostageState.Captured)
            FollowTarget();
    }

    public void RandomMovement()
    {
        //agent.SetDestination(transform.position + new Vector3(Random.Range(1, trialsAreaSize), 0f, Random.Range(1, trialsAreaSize)));
    }

    public override void DoThings()
    {
        Debug.Log("TARGET CAPTURED");
        listOfUser[0].nextAction = true;
        IAManager.Instance.IAHostageFollowingPlayer(this, listOfUser[0]);
        
        ResetValues();
    }

    void FollowTarget()
    {
        //Debug.Log("FOLLOWING");

        Vector3 targetPos = target.transform.position;
        //agent.SetDestination(targetPos);
        //agent.stoppingDistance = 2f;
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
                stress -= Random.Range(minStress, maxStress);
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
        List<Tile> pathToNearestExitDoor = pathfinder.SearchForShortestPath(agentTile, posToExit);
        AddDeplacement(pathToNearestExitDoor);
        //Debug.Log(actionsToPerform.Count);
    }

    public override void PerformAction()
    {
        if (actionsToPerform.Count != 0 && nextAction)
        {
            //Debug.Log("PERFORM 1 IAHOSTAGE ACTION");
            nextAction = false;
            actionsToPerform.First().PerformAction();
            actionsToPerform.Remove(actionsToPerform.First());
        }
    }

    public override void AddDeplacement(List<Tile> path)
    {
        //Debug.Log(path.Count);
        state = StateOfAction.Moving;
        destination = path[path.Count - 1];
        foreach (Tile tile in path)
        {
            if(hostageState == HostageState.InPanic)
                actionsToPerform.Add(new Deplacement(panicSpeed, this, tile));
            else
                actionsToPerform.Add(new Deplacement(normalSpeed, this, tile));
        }
        //Debug.Log(actionsToPerform.Count);
    }

    public override void FindAnOtherPath()
    {
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
        Debug.Log("New path deplacement number : " + newShortestPath.Count);
        AddDeplacement(newShortestPath);
        nextAction = true;
    }

    public void UpdatePositionToGo()
    {
        positionToGo = agentTile.GetFreeNeighbours().ToArray();
    }
}
