using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class FlyScript : MonoBehaviour {

    Collider boxbox;
    UnitFlight thisUnit;

    List<Tile> possibleDestinations;
	
    // Use this for initialization
	void Start () {
        boxbox = this.gameObject.GetComponent<Collider>();
        possibleDestinations = new List<Tile>();
        thisUnit = this.transform.root.GetComponent<UnitFlight>();
    }
	
	// Update is called once per frame
	void Update () {
        (boxbox as BoxCollider).size = new Vector3(2 * (thisUnit.GetCurrentMoveSpeed(true) + 0.1f), 2*(thisUnit.GetCurrentMoveSpeed(true) + 0.1f), 2);
	}
    private void OnTriggerStay(Collider other)
    {
        Tile otherTile = other.gameObject.GetComponent<Tile>();
        if (otherTile != null)
        {
            thisUnit.possibleToFlyTo.Add(other.transform.position);
            if (!possibleDestinations.Contains(otherTile))
            {
                possibleDestinations.Add(otherTile);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Tile otherTile = other.gameObject.GetComponent<Tile>();
        if (otherTile != null)
        {
            if (possibleDestinations.Contains(otherTile))
            {
                possibleDestinations.Remove(otherTile);
            }
        }
    }

    public List<Tile> GetPossibleDestinations()
    {
        return possibleDestinations;
    }
}
