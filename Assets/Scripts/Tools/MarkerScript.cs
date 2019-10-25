using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class MarkerScript : MonoBehaviour
{
    Renderer r;

    private void Start()
    {
        r = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {        
        r.enabled = !GetComponentInParent<Tile>().isUnderMovementMarker && MouseManager.Instance.mouseoveredTile != null;
    }

}
