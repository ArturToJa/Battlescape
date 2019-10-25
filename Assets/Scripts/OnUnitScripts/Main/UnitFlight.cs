using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class UnitFlight : UnitMovement
{

    [HideInInspector] public List<Vector3> possibleToFlyTo;
    [SerializeField]
    private int _Flight_Height;
    FlyScript flyScript;

    public int FLIGHT_HEIGHT
    {
        get { return _Flight_Height; }
        private set { _Flight_Height = value; }
    }


    protected override void Start()
    {
        base.Start();
        possibleToFlyTo = new List<Vector3>();
        if (FLIGHT_HEIGHT == 0)
        {
            Debug.LogError("FLIGHT_HEIGHT not set!");
        }
        flyScript = GetComponentInChildren<FlyScript>();
    }


    protected override void Update()
    {
        base.Update();
    }

    public void GoUp()
    {
        SetDestination(new Vector3(transform.position.x, FLIGHT_HEIGHT, transform.position.z));
    }

    public void FlyTowards(Vector3 landingPoint)
    {
        SetDestination(new Vector3(landingPoint.x, transform.position.y, landingPoint.z));
    }

    public void GoDown(Vector3 landingPoint)
    {
        SetDestination(landingPoint);
    }

    public List<Tile> GetPossibleDestinations()
    {
        return flyScript.GetPossibleDestinations();
    }

    public void ColourPossibleDestinations()
    {
        if (GameStateManager.Instance.IsCurrentPlayerLocal() == false)
        {
            Debug.LogError("why");
        }
        List<Tile> tiles = GetPossibleDestinations();
        foreach (Tile tile in tiles)
        {
            if (tile.IsProtectedByEnemyOf(GetComponent<UnitScript>()))
            {
                ColouringTool.SetColour(tile, Color.red);
            }
            else
            {
                ColouringTool.SetColour(tile, Color.green);
            }
        }
    }
}
