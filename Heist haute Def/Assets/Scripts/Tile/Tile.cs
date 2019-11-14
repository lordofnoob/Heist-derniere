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
    [HideInInspector] public TileNeighbours neighbours;
    public float Cost;
    //[HideInInspector] 
    public Tile previous;
    public TileType tileType;


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

        Tile North = Ma_LevelManager.Instance.GetWalkableTile(row - 1, column);
        Tile South = Ma_LevelManager.Instance.GetWalkableTile(row + 1, column);
        Tile East = Ma_LevelManager.Instance.GetWalkableTile(row, column - 1);
        Tile West = Ma_LevelManager.Instance.GetWalkableTile(row, column + 1);

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

    public void GetNeighbours()
    {
        /* neighbours.Add(Ma_LevelManager.Instance.GetTile(row - 1, column));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row + 1, column));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row, column + 1));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row, column - 1));
         //TANT QU IL N Y A PAS DE DETECTION INTERIEUR / EXTERIEUR C EST INUTILE A FAIRE PLUS TARD DU COUP
         /* neighbours.Add(Ma_LevelManager.Instance.GetTile(row - 1, column - 1));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row - 1, column + 1));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row + 1, column - 1));
         neighbours.Add(Ma_LevelManager.Instance.GetTile(row + 1, column + 1));*/
         Ma_LevelManager manager = GameObject.FindObjectOfType<Ma_LevelManager>();

           neighbours.North = manager.GetTile(row - 1, column);;
           neighbours.South = manager.GetTile(row + 1, column);
           neighbours.East =manager.GetTile(row, column - 1);
           neighbours.West =manager.GetTile(row, column + 1);
           neighbours.NW =  manager.GetTile(row -1, column + 1);
           neighbours.NE =  manager.GetTile(row -1, column - 1);
           neighbours.SW =  manager.GetTile(row +1, column + 1);
           neighbours.SE =  manager.GetTile(row +1, column - 1);
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
    
    public struct TileNeighbours
    {
        public Tile North;
        public Tile South;
        public Tile East;
        public Tile West;
        public Tile NW;
        public Tile NE;
        public Tile SW;
        public Tile SE;
    }


   

    public enum TileType
    {
        Other, Wall
    }

    [System.Flags]
    public enum CombinableWallType2
    {
        None = (0 << 0),
        Left = (1 << 0),
        Right = (1 << 1),
        Up = (1 << 2),
        Down = (1 << 3),

        LeftUp = (1 << 4),
        RightUp = (1 << 5),
        LeftDown = (1 << 6),
        RightDown = (1 << 7),

        LeftDownRight = (1 << 8),
        DownRightUp = (1 << 9),
        RightUpLeft = (1 << 10),
        UpLeftDown = (1 << 11),

        All = Left & Down & Left & Right
    }

}