using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [Header("Surlignance")]
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;
    [HideInInspector] public bool highlighted = false;

    [Header("TileParamaters")]
    public bool walkable;
    public bool avaible;
    public Mb_Agent agentOnTile = null;

    [Header("GridPamaters")]
    [SerializeField] public int row;
    [SerializeField] public int column;
    [HideInInspector] public float StraightLineDistanceToEnd, MinCostToStart;
    public float Cost;
    //[HideInInspector] 
    public Tile previous;

    public Tile(Vector3 position, float row, float column)
    {
        this.row = (int)row;
        this.column = (int)column;
    }

    void Start()
    {
      /*  rend = GetComponent<Renderer>();
        startColor = rend.material.color;*/
    }

    void OnMouseEnter()
    {
        if (!highlighted)
        {
            ModifyOutlines(Outlines.Mode.OutlineVisible, Color.black, 7.5f);
            SetOutlinesEnabled(true);

        }
    }

    void OnMouseExit()
    {
        if (!highlighted)
            SetOutlinesEnabled(false);
    }

    public float StraightLineDistanceTo(Tile end)
    {
        //print((end.transform.position - transform.position).magnitude);
        return (end.transform.position - transform.position).magnitude;
    }

    public List<Tile> GetFreeNeighbours(bool useDoors = false)
    {
        List<Tile> res = new List<Tile>();

        Tile North = Ma_LevelManager.Instance.GetTile(row - 1, column);
        Tile South = Ma_LevelManager.Instance.GetTile(row + 1, column);
        Tile East = Ma_LevelManager.Instance.GetTile(row, column - 1);
        Tile West = Ma_LevelManager.Instance.GetTile(row, column + 1);

        if(North != null && North.walkable)
        {
            if((useDoors && North.GetComponentInChildren<Mb_Door>() != null) || North.avaible)
                res.Add(North);
        }

        if(South != null && South.walkable)
        {
            if ((useDoors && South.GetComponentInChildren<Mb_Door>() != null) || South.avaible)
                res.Add(South);
        }

        if(East != null && East.walkable)
        {
            if ((useDoors && East.GetComponentInChildren<Mb_Door>() != null) || East.avaible)
                res.Add(East);
        }

        if(West != null && West.walkable )
        {
            if ((useDoors && West.GetComponentInChildren<Mb_Door>() != null) || West.avaible)
                res.Add(West);
        }
        return res;
    }

    public List<Tile> GetNeighbours()
    {
        List<Tile> res = new List<Tile>();

        Tile North = Ma_LevelManager.Instance.GetTile(row - 1, column);
        Tile South = Ma_LevelManager.Instance.GetTile(row + 1, column);
        Tile East = Ma_LevelManager.Instance.GetTile(row, column - 1);
        Tile West = Ma_LevelManager.Instance.GetTile(row, column + 1);
        Tile NW = Ma_LevelManager.Instance.GetTile(row -1, column + 1);
        Tile NE = Ma_LevelManager.Instance.GetTile(row -1, column - 1);
        Tile SW = Ma_LevelManager.Instance.GetTile(row +1, column + 1);
        Tile SE = Ma_LevelManager.Instance.GetTile(row +1, column - 1);

        res.Add(North);
        res.Add(South);
        res.Add(East);
        res.Add(West);
        res.Add(NW);
        res.Add(NE);
        res.Add(SW);
        res.Add(SE);

        return res;
    }

    public void ModifyOutlines(Outlines.Mode mode, Color color, float width)
    {
        Outlines outline = gameObject.GetComponent<Outlines>();
        outline.OutlineMode = mode;
        outline.OutlineColor = color;
        outline.OutlineWidth = width;
    }

    public void SetOutlinesEnabled(bool enabled)
    {
        Outlines outline = gameObject.GetComponent<Outlines>();
        outline.enabled = enabled;
    }

    public void SetColumnAndRow(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
    }

}