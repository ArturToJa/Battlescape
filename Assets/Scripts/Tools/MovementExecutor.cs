using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementExecutor
{
    public MovementExecutor(UnitMovement _v, List<Vector3> _p)
    {
        voyager = _v;
        path = new List<Vector3>(_p);
    }
    List<Vector3> path;
    UnitMovement voyager;


    public IEnumerator Travel()
    {
        UnitScript voyagerUnitScript = voyager.GetComponent<UnitScript>();
        //sanity check
        if (voyagerUnitScript == null)
        {
            Debug.LogError("Why the fuck is there no UnitScript on this Voyager??");
            Log.SpawnLog("ERROR! MoveExe line22");
            yield return null;
        }
        else if (voyager is UnitFlight)
        {
            Debug.LogError("Travel used instead of fly! Why?");
            Log.SpawnLog("ERROR! MoveExe line30");
            yield return null;
        }

        else
        //we generally HAVE TO end up here
        {
            SetTileAndUnitPair(voyager.GetComponent<UnitScript>(), path[path.Count - 1]); //basically tells FINAL tile of the path and unit 'you are now married'.
            voyager.CanMove = false; // player cannot move it now
            voyager.isMoving = true; //mostly so that you cannot use an ability on this unit untill it stops moving

            for (int i = 0; i < path.Count; i++)
            {
                voyager.SetDestination(path[i]);
                PathCreator.Instance.ClearPath();
                yield return new WaitUntil(voyager.HasStoppedMoving);
            }

            FinishMovement(voyager);
        }


    }

    public static void SetTileAndUnitPair(UnitScript unit, Vector3 tile)
    {
        int tileX = (int)tile.x;
        int tileZ = (int)tile.z;
        Tile myTile = Map.Board[tileX, tileZ];
        myTile.OnUnitEnterTile(unit);
    }

    public IEnumerator Fly(UnitFlight flier, Vector3 destination)
    {
        if (flier.GetComponent<UnitScript>() != null)
        {
            SetTileAndUnitPair(flier.GetComponent<UnitScript>(), destination);
        }
        flier.CanMove = false;
        flier.isMoving = true; 
        flier.GoUp();
        while (Mathf.Abs(flier.transform.position.y - flier.FLIGHT_HEIGHT) > flier.GetSmoothDistance())
        {
            yield return null;

        }
        flier.FlyTowards(destination);
        while (Vector3.Distance(flier.transform.position, flier.GetDestination()) > flier.GetSmoothDistance())
        {
            yield return null;
        }

        flier.GoDown(destination);
        while (flier.transform.position.y > flier.GetSmoothDistance())
        {
            yield return null;
        }
        PathCreator.Instance.Path.Clear();
        FinishMovement(flier);

    }


    void FinishMovement(UnitMovement unitMovement)
    {
        PathCreator.Instance.Path.Clear();

        GameStateManager.Instance.EndAnimation();

        unitMovement.transform.position = Helper.RoundPosition(unitMovement.gameObject);

        SetMovementRelatedData(unitMovement); // So if it IS a Unit, whioch currently it always is, it gets set to not moving, etc.

        DeselectIfAI();
    }

    void DeselectIfAI()
    {
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            MouseManager.Instance.Deselect();
        }
    }

    void SetMovementRelatedData(UnitMovement unitMovement)
    {
        unitMovement.isMoving = false;

        UnitScript unitScript = unitMovement.GetComponent<UnitScript>();
        if (unitScript.CheckIfIsInCombat())
        {
            unitScript.hasJustArrivedToCombat = true;
        }
        if (unitScript.isQuittingCombat)
        {
            unitScript.isQuittingCombat = false;
        }
    }
}
