using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Tile Start, End;
    [HideInInspector]public int TileVisited = 0;
    private List<Tile> ShortestPath;

    public List<Tile> SearchForShortestPath(Tile start, Tile end)
    {
        Start = start;
        End = end;
        TileVisited = 0;

        foreach (Tile tile in Ma_LevelManager.Instance.Grid.freeTiles)
        {
            tile.straightLineDistanceToEnd = tile.StraightLineDistanceTo(end);
        }

        AstarSearch();
        List<Tile> ShortestPath = new List<Tile>();
        ShortestPath.Add(end);
        BuildShortestPath(ShortestPath, end);
        //ResetVisitedTile();
        ShortestPath.Reverse();
        print(ShortestPath.Count);
        HighLightShortestPath();

        return ShortestPath;
    }

    private void AstarSearch()
    {
        Start.minCostToStart = 0;
        List<Tile> Queue = new List<Tile>();
        Queue.Add(Start);
        do
        {
            Queue.OrderBy(x => x.minCostToStart + x.straightLineDistanceToEnd).ToList();
            Tile currentTile = Queue.First();
            Queue.Remove(currentTile);
            TileVisited++;

            foreach(Tile neighbours in currentTile.GetFreeNeighbours())
            {
                if (neighbours.visited)
                    continue;
                if(neighbours.minCostToStart == 0f || currentTile.minCostToStart + 1 < neighbours.minCostToStart)
                {
                    neighbours.minCostToStart = currentTile.minCostToStart + 1;
                    neighbours.previous = currentTile;
                    if (!Queue.Contains(neighbours))
                        Queue.Add(neighbours);
                }
            }
            currentTile.visited = true;
            if (currentTile == End)
                return;
        } while (Queue.Any());
    }

    private void BuildShortestPath(List<Tile> shortestPath, Tile tile)
    {
        if(tile.previous == null)
        {
            ShortestPath = shortestPath;
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
            tile.ModifyOutlines(Outlines.Mode.OutlineVisible, Color.magenta, 7.5f);
            tile.SetOutlinesEnabled(true);
            tile.outlined = true;
        }
    }

    public void ResetVisitedTile()
    {
        foreach (Tile tile in Ma_LevelManager.Instance.Grid.freeTiles)
        {
            if (tile.visited)
                tile.visited = false;
        }
    }

}
