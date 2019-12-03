using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_DEPOPHOSTAGe : MonoBehaviour
{

    public Tile tileAssociated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Mb_IAAgent>())
        {
            Destroy(other);
            Ma_LevelManager.Instance.timeRemaining -= 10;
            tileAssociated.avaible = true;
        }
    }
}
