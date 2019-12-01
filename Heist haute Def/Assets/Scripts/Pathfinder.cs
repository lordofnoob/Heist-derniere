using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Mb_Agent agent;

    private Tile Start;
    private List<Tile> PosToGo;
    [HideInInspector]public int TileVisited = 0;
    private List<Tile> ShortestPath;
    private float ShortestPathCost;

    public void Awake()
    {
        agent = GetComponent<Mb_Agent>();
    }

    // start = start tile, posToGo = liste des tuiles ou il doit aller,  hit = case sur laquelle il va dse de^placer , la liste de postogo c est en general la liste des tuiles d u trila
    // Ai.SearchForShortestPath(AI.tile, Ma_LevelManager.levelManager.list<escapeDoor>, Ma_LevelManager.levelManager.list<escapeDoor>[2]);
    public List<Tile> SearchForShortestPath(Tile start, List<Tile> posToGo, bool useDoors = false)
    {
        //Debug.Log(hit.transform.position);

        //Debug.Log("Start : " + start.row +", "+start.column);
        //Debug.Log("Position to go : " + posToGo.Count);

        ResetVisitedTile();
        Start = start;
        TileVisited = 0;
        ShortestPathCost = -1;
        PosToGo = posToGo;

        foreach(Tile endPos in PosToGo)
        {
            foreach (Tile tile in Ma_LevelManager.Instance.allWalkableTile)
            {
                tile.StraightLineDistanceToEnd = tile.StraightLineDistanceTo(endPos);
            }

            AstarSearch(useDoors);
        }

        ShortestPath.Reverse();
        ResetTilesPrevious();
        //print(ShortestPath.Count);
        HighLightShortestPath();

        return ShortestPath;
    }

    private void AstarSearch(bool useDoors)
    {
        Start.MinCostToStart = 0;
        List<Tile> Queue = new List<Tile>();
        Queue.Add(Start);
        do
        {
            Queue.OrderBy(x => x.MinCostToStart + x.StraightLineDistanceToEnd).ToList();
            Tile currentTile = Queue.First();
            Queue.Remove(currentTile);
            TileVisited++;

            foreach(Tile neighbours in currentTile.GetFreeNeighbours(useDoors))
            {
                //Debug.Log("NEIGHBOURS : " + neighbours.transform.position);
                if (agent.VisitedTiles.Contains(neighbours))
                    continue;
                if(neighbours.MinCostToStart == 0f || currentTile.MinCostToStart + 1 < neighbours.MinCostToStart)
                {
                    neighbours.MinCostToStart = currentTile.MinCostToStart + neighbours.cost;
                    neighbours.previous = currentTile;
                    if (!Queue.Contains(neighbours))
                        Queue.Add(neighbours);
                }
            }
            agent.VisitedTiles.Add(currentTile);

            if (PosToGo.Contains(currentTile))
            {
                if (ShortestPathCost == -1 || currentTile.MinCostToStart < ShortestPathCost)
                {
                    ShortestPathCost = currentTile.MinCostToStart;
                    List<Tile> shortestPath = new List<Tile>();
                    shortestPath.Add(currentTile);
                    BuildShortestPath(shortestPath, currentTile);
                    ShortestPath = shortestPath;
                }
                return;
            }
        } while (Queue.Any());
    }

    private void BuildShortestPath(List<Tile> shortestPath, Tile tile)
    {
        //Debug.Log(shortestPath.Count);
        if(tile.previous == null)
        {
            return;
        }
        if(tile.previous != Start)
            shortestPath.Add(tile.previous);
        BuildShortestPath(shortestPath, tile.previous);
    }

    private void HighLightShortestPath()
    {
        foreach (Tile tile in ShortestPath)
        {
            //tile.ModifyOutlines(Outlines.Mode.OutlineVisible, Color.magenta, 7.5f);
            //tile.SetOutlinesEnabled(true);
            tile.highlighted = true;
        }
    }

    public void ResetVisitedTile()
    {
        foreach (Tile tile in Ma_LevelManager.Instance.allWalkableTile)
        {            
            tile.previous = null;
            tile.MinCostToStart = 0;
            tile.StraightLineDistanceToEnd = 0;
        }
        agent.VisitedTiles = new List<Tile>();
        ShortestPath = new List<Tile>();
    }

    public void ResetTilesPrevious()
    {
        foreach(Tile tile in ShortestPath)
        {
            tile.previous = null;
        }
    }
}
