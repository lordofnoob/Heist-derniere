using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

public class Ma_PlayerManager : MonoBehaviour
{
    public static Ma_PlayerManager instance;

    public Mb_InputController InputController;
    public Transform PlayersContainer;
    public Mb_Player[] playerList;

    public Mb_Player selectedPlayer = null;
    public int TickPerTileSpeed = 4;

    void Awake()
    {
        instance = this;
        playerList = PlayersContainer.GetComponentsInChildren<Mb_Player>();
    }

    void Update()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if (InputController.LeftClick)
        {
            Ray ray = Ma_CameraManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Mb_Player p = hit.transform.GetComponent<Mb_Player>();
                    if (p != selectedPlayer)
                        SelectPlayer(p);
                }
                else
                    DeselectPlayer();
            }
            
        }
        
        if (InputController.RightClick)
        {
            Ray ray = Ma_CameraManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {

                //quand le joueur est en train d interagir
                if (hit.transform.CompareTag("Tile") && selectedPlayer != null && selectedPlayer.GetActionState() != StateOfAction.Captured && selectedPlayer.GetActionState() != StateOfAction.Moving)
                {
                    /*hit.point += new Vector3(Ma_LevelManager.Instance.FreePrefab.transform.localScale.x / 2, 0f, Ma_LevelManager.Instance.FreePrefab.transform.localScale.x / 2);

                    Vector3 gridPos = Vector3.zero;
                    gridPos.x = Mathf.Floor(hit.point.x / Ma_LevelManager.Instance.FreePrefab.transform.localScale.x) * Ma_LevelManager.Instance.FreePrefab.transform.localScale.x;
                    gridPos.z = Mathf.Floor(hit.point.z / Ma_LevelManager.Instance.FreePrefab.transform.localScale.x) * Ma_LevelManager.Instance.FreePrefab.transform.localScale.x;
                    */

                    if (selectedPlayer.onGoingInteraction != null)
                    {
                        selectedPlayer.onGoingInteraction.listOfUser.Remove(selectedPlayer);
                        selectedPlayer.onGoingInteraction.QuittingCheck();
                        selectedPlayer.onGoingInteraction = null;
                    }
                    //selectedPlayer.nextAction = true;
                    //Debug.Log(hit.transform.GetComponent<Tile>().transform.position);
                    /*List<Tile> ShortestPath = selectedPlayer.pathfinder.SearchForShortestPath(selectedPlayer.AgentTile, new List<Tile> { hit.transform.GetComponent<Tile>() });
                    selectedPlayer.ChangeDeplacement(ShortestPath);*/

                    selectedPlayer.GoTo(hit.transform.GetComponent<Tile>());
                }

                else if (hit.transform.CompareTag("Trial")  && selectedPlayer !=null && selectedPlayer.GetActionState() != StateOfAction.Captured && selectedPlayer.GetActionState() != StateOfAction.Interacting)
                {
                    Mb_Trial targetTrial = hit.transform.gameObject.GetComponent<Mb_Trial>();
                    if (selectedPlayer.onGoingInteraction != targetTrial)
                    {
                        //CHANGED 

                        if(targetTrial is Mb_IATrial)
                        {
                            Mb_IATrial iaTrial = targetTrial as Mb_IATrial;
                            iaTrial.iaAgent.SomeoneWillInteractWith = selectedPlayer;
                        }

                        /*List<Tile> ShortestPath = selectedPlayer.pathfinder.SearchForShortestPath(selectedPlayer.AgentTile, positionToAccomplishDuty);
                        selectedPlayer.ChangeDeplacement(ShortestPath);
                        selectedPlayer.SetNextInteraction(targetTrial);
                        selectedPlayer.nextAction = true;*/

                        selectedPlayer.trialsToGo.Add(targetTrial);
                        selectedPlayer.GoTo(targetTrial);
                    }                    
                }
            }            
        }

    }

    public void SelectPlayer(Mb_Player p)
    {
        //Debug.Log("SELECT PLAYER");
        if (selectedPlayer != null)
            selectedPlayer.IsSelected = false;
        selectedPlayer = p;
        p.IsSelected = true;
    }

    public void SelectListedPlayer(int playerNumber)
    {
        if (selectedPlayer != null)
            selectedPlayer.IsSelected = false;

        selectedPlayer = playerList[playerNumber];
        Ma_CameraManager.Instance.TargetLooking(selectedPlayer.transform.position);
        selectedPlayer.IsSelected = true;
    }

    public void DeselectPlayer()
    {
        if(selectedPlayer != null)
        {
            //Debug.Log("DESELECT PLAYER");
            selectedPlayer.IsSelected = false;
            selectedPlayer = null;
        }
    }


}
