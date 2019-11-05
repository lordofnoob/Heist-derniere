using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Tile> freeTiles = new List<Tile>();
    public Tile[,] tilemap = new Tile[20,20];

    public void Awake()
    {
        //freeTiles.AddRange(GameObject.Find("Tiles").GetComponentsInChildren<Free>());
    }

    public void BuildGridLevel(string[,] array)
    {
        Vector3 position = Vector3.zero;
        Tile newTile = null;

        for(int row = 1; row <= array.GetLength(0); row++)
        {
            for(int column = 1; column <= array.GetLength(1); column++)
            {
                //print(position +" : ["+row+","+column+"]");
                switch (array[row-1,column-1])
                {
                    case "W":
                        GameObject newWall = Instantiate(Ma_LevelManager.Instance.WallPrefab, position, new Quaternion(0f, 0f, 0f, 0f), GameObject.Find("Walls").transform);
                        newTile = newWall.GetComponent<Wall>();
                        newTile.row = row;
                        newTile.column = column;
                        break;

                    case "F":
                        GameObject newFree = Instantiate(Ma_LevelManager.Instance.FreePrefab, position, new Quaternion(0f, 0f, 0f, 0f), GameObject.Find("Tiles").transform);
                        newTile = newFree.GetComponent<Free>();
                        newTile.row = row;
                        newTile.column = column;
                        freeTiles.Add(newTile);
                        break;

                    case "P":
                        GameObject newFreePlayer = Instantiate(Ma_LevelManager.Instance.FreePrefab, position, new Quaternion(0f, 0f, 0f, 0f), GameObject.Find("Tiles").transform);
                        newTile = newFreePlayer.GetComponent<Free>();
                        newTile.row = row;
                        newTile.column = column;
                        //Spawn player
                        GameObject player = Instantiate(Ma_LevelManager.Instance.PlayerPrefab, new Vector3(position.x, 0.5f, position.z), new Quaternion(0f, 0f, 0f, 0f));
                        player.GetComponent<Mb_Player>().playerTile = newTile;
                        freeTiles.Add(newTile);
                        break;

                    case "IA":
                        GameObject newFreeIA = Instantiate(Ma_LevelManager.Instance.FreePrefab, position, new Quaternion(0f, 0f, 0f, 0f), GameObject.Find("Tiles").transform);
                        newTile = newFreeIA.GetComponent<Free>();
                        newTile.row = row;
                        newTile.column = column;
                        //Spawn player
                        GameObject IA = Instantiate(Ma_LevelManager.Instance.IAPrefab, new Vector3(position.x, 0.5f, position.z), new Quaternion(0f, 0f, 0f, 0f));
                        IA.GetComponent<Mb_IAHostage>().IATile = newTile;
                        IAManager.Instance.IAList.Add(IA.GetComponent<Mb_IAHostage>());
                        freeTiles.Add(newTile);
                        break;
                }

                SetTileInTilemap(newTile, row, column);

                if (column != 20)
                    position.z += Ma_LevelManager.Instance.FreePrefab.transform.localScale.x;
                else
                    position.z = 0;
            }
            position.x += Ma_LevelManager.Instance.FreePrefab.transform.localScale.x;
        }
    }

    public void SetTileInTilemap(Tile tile, int row, int column)
    {
        tilemap[row - 1, column - 1] = tile;
    }

    public Tile GetTile(int row, int column)
    {
        //print(row + " , " + column);
        return tilemap[row - 1, column - 1];
    }
}
