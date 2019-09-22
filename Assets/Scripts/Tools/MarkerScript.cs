using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    Renderer r;

    private void Start()
    {
        r = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        r.enabled = !GetComponentInParent<Tile>().isUnderEacs && MouseManager.Instance.mouseoveredTile != null;
    }

}
