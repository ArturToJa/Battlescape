using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS is the most important script in all of tiles.. Please dont ask me about all those 5 million ways of colouring stuff in my code. I dont know, it just appeared one day and i learned to live with that @_@

[RequireComponent(typeof(TileColouringTool))]
public class Tile : MonoBehaviour
{
    public bool isBeingColoredByNormalHighlighter = false;
    public bool isBeingColoredByCombatHighlighter = false;
    public static bool areDropzonesOn = true;
    public bool isFirstDropzone;
    public bool isSecondDropzone;
    public bool isWalkable = true;
    public bool hasObstacle;
    public bool isShootable = true;
    public bool hasDoodad = false;
    public bool[] isOccupiedByPlayer = new bool[2];
    //public List<ClickableTile> neighbours;
    public bool isUnderEacs = false;
    public UnitScript myUnit;
    public TileColouringTool TCTool { get; private set; }
    GameObject grid;

    public Color theColor = Color.white;

    private void Start()
    {
        grid = this.transform.GetChild(0).gameObject;
        grid.SetActive(false);
        this.name = "Tile_" + (int)this.transform.position.x + "_" + (int)this.transform.position.z;
        TCTool = this.GetComponent<TileColouringTool>();
    }

    private void Update()
    {
        if (TurnManager.Instance.TurnCount == 0 && isFirstDropzone && areDropzonesOn)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (TurnManager.Instance.TurnCount == 0 && isSecondDropzone && areDropzonesOn)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            grid.SetActive(!grid.activeSelf);
        }
        /*if (Ability_Basic.IsForcingMovementStuff == false && MouseManager.Instance.SelectedUnit != null && Input.GetMouseButtonDown(0) && MovementController.Instance.temporaryPositions.Count > 1 && this == MovementController.Instance.temporaryPositions[MovementController.Instance.temporaryPositions.Count - 1] && MouseManager.Instance.SelectedUnit.GetComponent<UnitMovement>().isMoving == false)
        {
            MovementController.Instance.AcceptMovement(MouseManager.Instance.SelectedUnit.GetComponent<UnitMovement>());
        }*/
    }

    public List<Tile> GetNeighbours()
    {
        List<Tile> returnList = new List<Tile>();
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        // then, lets say that we FIRST want to add if z=j or x=i
        // finally - we add the rest.
        List<Tile> temporaryList = new List<Tile>();
        for (int i = 0; i < Map.Board.GetLength(0); i++)
            for (int j = 0; j < Map.Board.GetLength(1); j++)
            {
                //next line means: all 8 neighbours, literally (and prevents OUR tile to be inside the scope), NOTE THAT unwalkable/obstacled tiles are neighbours.
                if (Mathf.Abs(i - x) <= 1 && Mathf.Abs(j - z) <= 1 && !(x == i && z == j))
                {
                    //what im doning here most liekly can be done by just sorting the returnlist and then two lists are redundant.
                    //we add the "priority" tiles to the return list first and store the rest in the temporary list
                    if (x == i || z == j)
                    {
                        returnList.Add(Map.Board[i, j]);
                    }
                    else
                    {
                        temporaryList.Add(Map.Board[i, j]);
                    }

                }
            }
        foreach (Tile tile in temporaryList)
        {
            returnList.Add(tile);
        }
        return returnList;

    }

    public bool IsLegalTile()
    {
        return ((hasObstacle == false) && isWalkable);
    }


    public void OnMyUnitDied()
    {    
        myUnit = null;
    }
    public void OnUnitExitTile()
    {
       /* Debug.Log(transform.position);
        Debug.Log(myUnit);*/
        myUnit.DeathEvent -= OnMyUnitDied;
        myUnit = null;
        this.isWalkable = true;
        SetBeingOccupiedByPlayers();
        foreach (Tile neighbour in GetNeighbours())
        {
            neighbour.SetBeingOccupiedByPlayers();
        }
    }

    public bool IsOccupiedByEnemy(int playerID)
    {
        if (playerID == 0)
        {
            return isOccupiedByPlayer[1];
        }
        else
        {
            return isOccupiedByPlayer[0];
        }
    }

    public void OnUnitEnterTile(UnitScript myNewUnit)
    {
        myUnit = myNewUnit;
        myNewUnit.DeathEvent += OnMyUnitDied;
        if (myUnit.myTile != null)
        {
            myUnit.myTile.OnUnitExitTile();
        }
        myUnit.myTile = this;
        isWalkable = false;
        SetBeingOccupiedByPlayers();
        foreach (Tile neighbour in GetNeighbours())
        {
            neighbour.SetBeingOccupiedByPlayers();
        }
    }

    void SetBeingOccupiedByPlayers()
    {
        isOccupiedByPlayer[0] = false;
        isOccupiedByPlayer[1] = false;
        SetBeingOccupiedBecauseOfUnit(myUnit);
        foreach (Tile neighbour in GetNeighbours())
        {
            SetBeingOccupiedBecauseOfUnit(neighbour.myUnit);
        }
    }

    void SetBeingOccupiedBecauseOfUnit(UnitScript tilesUnit)
    {
        if (tilesUnit != null)
        {
            isOccupiedByPlayer[tilesUnit.PlayerID] = true;
        }
    }



    public bool IsTileInExactRangeOf(Tile other, int range)
    {
        return
            (Mathf.Abs(transform.position.x - other.transform.position.x) == range &&
             Mathf.Abs(transform.position.z - other.transform.position.z) <= range)
             ||
             (Mathf.Abs(transform.position.z - other.transform.position.z) == range &&
             Mathf.Abs(transform.position.x - other.transform.position.x) <= range);



    }
}

