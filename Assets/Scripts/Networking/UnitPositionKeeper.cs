/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPositionKeeper : MonoBehaviour
{
    public static UnitPositionKeeper Instance;
    [HideInInspector]
    public PhotonView photonView;
    public Dictionary<int, BattlescapeLogic.Unit> UnitsInGame = new Dictionary<int, BattlescapeLogic.Unit>();
    int maxID = 0;
    //these are units with their unique integerIDs, they will be checked by multiuplayer if they are positioned where they should!

    int[,] UnitPositions = new int[Global.instance.map.mapWidth, Global.instance.map.mapHeight];
    //these are the simplified version of Global.instance.map.board - just so that i can sedn it over the net... UnitPosition[i,j] = 0 means empty slot, other number = ID from UnitsInGame queue.


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        photonView = GetComponent<PhotonView>();
        ClearMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Application.isEditor)
        {
            photonView.RPC("RPCRepositionAll", PhotonTargets.All);
        }    
    }

    void ClearMap()
    {
        for (int i = 0; i < UnitPositions.GetLength(0); i++)
            for (int j = 0; j < UnitPositions.GetLength(1); j++)
            {
                UnitPositions[i, j] = 0;
            }
        UnitsInGame.Clear();
    }

    [PunRPC]
    public void RPCAddUnit(int x, int z)
    {
        BattlescapeLogic.Unit unit = Global.instance.map.board[x, z].myUnit;
        AddUnit(unit, x, z);
        Debug.Log(UnitsInGame.Count);
    }

    void AddUnit(BattlescapeLogic.Unit unit, int x, int z)
    {
        maxID++;
        UnitsInGame.Add(maxID, unit);
        UnitPositions[x,z] = maxID;
    }

    BattlescapeLogic.Unit FindUnit(int x, int z)
    {
        int unitID = UnitPositions[x, z];
        BattlescapeLogic.Unit unit = UnitsInGame[unitID];
        return unit;
    }

    [PunRPC]
    public void RPCDeleteUnit(int x, int z)
    {
        BattlescapeLogic.Unit unit = Global.instance.map.board[x, z].myUnit;
        Debug.Log(unit);
        DeleteUnit(unit);
        
    }

    [PunRPC]
    public void RPCDeleteAll(int ID)
    {
        if (UnitsInGame.Count == 0)
        {
            return;
        }
        for (int i = 1; i < UnitsInGame.Count+1; i++)
        {
            if (UnitsInGame[i] != null && UnitsInGame[i].PlayerID == ID)
            {
                DeleteUnit(UnitsInGame[i]);
            }
        }
    }

    void DeleteUnit(BattlescapeLogic.Unit unit)
    {
        int x = Mathf.RoundToInt(unit.transform.position.x);
        int z = Mathf.RoundToInt(unit.transform.position.z);
        UnitsInGame.Remove(UnitPositions[x, z]);
        UnitPositions[x, z] = 0;
    }

    [PunRPC]
    void RPCMoveUnit(int startX, int startZ, int endX, int endZ)
    {
        BattlescapeLogic.Unit unit = Global.instance.map.board[startX, startZ].myUnit;
        Log.SpawnLog(startX + ", " + startZ);
        MoveUnit(unit, endX, endZ);
    }

    public void MoveUnit(BattlescapeLogic.Unit unit, int x, int z)
    {
        DeleteUnit(unit);
        AddUnit(unit, x, z);
    }


    //this function wants FACTUAL IN-GAMEdata to be fed and it checks them with THIS CLASS as "true" data.
    bool IsCorrectlyPositioned(BattlescapeLogic.Unit unit)
    {
        int x = Mathf.RoundToInt(unit.transform.position.x);
        int z = Mathf.RoundToInt(unit.transform.position.z);
        return FindUnit(x, z) == unit;
    }

    void CheckCorrectness()
    {
        foreach (var item in UnitsInGame)
        {
            if (IsCorrectlyPositioned(item.Value) == false)
            {
                photonView.RPC("RPCRepositionAll", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    public void RPCRepositionAll()
    {
        for (int i = 0; i < UnitPositions.GetLength(0); i++)
            for (int j = 0; j < UnitPositions.GetLength(1); j++)
            {
                if (UnitPositions[i,j] != 0)
                {
                    BattlescapeLogic.Unit unit = UnitsInGame[UnitPositions[i, j]];
                    if (unit.currentPosition != null)
                    {
                        unit.currentPosition.myUnit = null;
                    }
                    Tile newTile = Global.instance.map.board[i, j];
                    unit.currentPosition = newTile;
                    newTile.myUnit = unit;
                    unit.GetComponent<BattlescapeLogic.Unit>().SetDestination(newTile.transform.position);
                    unit.transform.position = newTile.transform.position;
                }
                else
                {
                    Tile tile = Global.instance.map.board[i, j];
                    if (tile.myUnit != null && (UnitsInGame.ContainsValue(tile.myUnit) == false))
                    {
                        Debug.Log(tile.myUnit);
                        //SO - Unitpositions[i,j] is ZERO (tile shall be empty) but tile HAS a unit, BUT this unit is not on the list - therefore it should be destroyed (it died, but nobody cared).
                        // It can be seen, that if UPos[i,j] == 0, then either unit is dead (this case), OR unit is on a wrong square (and then the first case will finally find it)
                        BattlescapeLogic.Unit unit = tile.myUnit;
                        tile.myUnit = null;
                        Destroy(unit.gameObject);
                    }
                }
            }
    }

}*/
