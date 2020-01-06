using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUGDisplayTileCoord : MonoBehaviour
{
    public Text c, r;

    public void SetColumnAndRowDisplay(int column, int row)
    {
        c.text = column.ToString();
        r.text = row.ToString();
    }
}
