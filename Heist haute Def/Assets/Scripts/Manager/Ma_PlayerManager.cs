using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

public class Ma_PlayerManager : MonoBehaviour
{
    public static Ma_PlayerManager Instance;

    public Mb_InputController InputController;

    public Mb_Player selectedPlayer = null;
    public int TickPerTileSpeed = 4;

    void Awake()
    {
        Instance = this;
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
                if (hit.transform.CompareTag("Tile") && selectedPlayer != null && selectedPlayer.state != Mb_Player.StateOfAction.Captured && selectedPlayer.state != Mb_Player.StateOfAction.Moving)
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

                    //Debug.Log(hit.transform.GetComponent<Tile>().transform.position);
                    List<Tile> ShortestPath = Ma_LevelManager.Instance.GetComponentInChildren<Pathfinder>().SearchForShortestPath(selectedPlayer.playerTile, new List<Tile> { hit.transform.GetComponent<Tile>() },hit.transform.GetComponent<Tile>());
                    selectedPlayer.AddDeplacement(ShortestPath);
                }
                else if (hit.transform.CompareTag("Trial")  && selectedPlayer !=null && selectedPlayer.state != Mb_Player.StateOfAction.Captured && selectedPlayer.state!= Mb_Player.StateOfAction.Interacting)
                {
                    Mb_Trial targetTrial = hit.transform.gameObject.GetComponent<Mb_Trial>();
                    if (selectedPlayer.onGoingInteraction != targetTrial)
                    {
                        //CHANGED 
                        List<Tile> positionToAccomplishDuty = new List<Tile>();
                        if (targetTrial.listOfUser.Count >= targetTrial.positionToGo.Length)
                        {
                            for (int i = 0; i < targetTrial.listOfUser.Count; i++)
                            {
                                if (targetTrial.listOfUser[i] == null)
                                {
                                    positionToAccomplishDuty.Add(targetTrial.positionToGo[i]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < targetTrial.positionToGo.Length; i++)
                            {
                                positionToAccomplishDuty.Add(targetTrial.positionToGo[i]);
                            }
                        }

                        if (positionToAccomplishDuty.Count == 0)
                        {
                            Debug.Log("DEPLACEMENT IMPOSSIBLE");
                            return;
                        }

                        List<Tile> ShortestPath = Ma_LevelManager.Instance.GetComponentInChildren<Pathfinder>().SearchForShortestPath(selectedPlayer.playerTile, positionToAccomplishDuty, targetTrial.trialTile);
                        selectedPlayer.AddDeplacement(ShortestPath);
                        selectedPlayer.SetNextInteraction(targetTrial);
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
