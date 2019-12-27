using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_BaseClass
{

    //So this is our BASE class for AI. It will have "children" deriving from it for every type of ACTION we will need AI to do.

    //AIs will NOT know anything about what phase we are in, it is decided one level above. Here we know we are oding for example movement or attack.
    
    protected int ID;
    protected Dictionary<Tile, float> EvaluatedTiles;
    public List<BattlescapeLogic.Unit> enemyList;
    public List<BattlescapeLogic.Unit> allyList;

    List<BattlescapeLogic.Unit> LastThreeSelectedUnits;

    public AI_BaseClass(int ID)
    {
        CallTheConstructor(ID);        
    }

    protected void CallTheConstructor(int ID)
    {
        LastThreeSelectedUnits = new List<BattlescapeLogic.Unit>();
        this.ID = ID;
        allyList = Global.instance.playerTeams[ID].players[0].playerUnits;
        enemyList = Global.instance.GetNextPlayer(Global.instance.playerTeams[ID].players[0]).playerUnits;
    }

    public void DoAI()
    {
        
        //First, find who can we move! AND if nobody, then we pass turn
        Queue<BattlescapeLogic.Unit> unitsToMove = GetPossibleUnits();
        if (unitsToMove != null && unitsToMove.Count > 0)
        {
            BattlescapeLogic.Unit currentUnit = unitsToMove.Dequeue();
            if (IsInInfiniteLoop(currentUnit))
            {
                PopupTextController.AddPopupText("Bug appeared! Tell Dogo about it!", PopupTypes.Info);
                Log.SpawnLog("An AI - bug happenned. Please tell Dogo about it.");
                //GameStateManager.NextPhase();
                return;
            }
            LastThreeSelectedUnits.Add(currentUnit);
            if (LastThreeSelectedUnits.Count > 3)
            {
                LastThreeSelectedUnits.RemoveAt(0);
            }
            MouseManager.instance.unitSelector.SelectUnit(currentUnit);

            ///////////////////// ABILITIES HERE//////////////////////////////
            // Here we should check if we want to use an active ability!
            if (CheckIfGoodToUseAbility(currentUnit))
            {
                //CombatController.Instance.MakeAIWait(3f);
                return;
            }
            

            //Debug.Log("Starting evaluating");
            AI_Controller.Instance.EvaluateTiles(this, currentUnit, GetPossibleMoves(currentUnit, true));
            //Debug.Log("Finished evaluating");
            if (AI_Controller.tilesAreEvaluated)
            {
                var kvp = GetTheMove(currentUnit, EvaluatedTiles);
                if (this is AI_Base_Attack)
                {
                    CameraController.Instance.SetCamToU(currentUnit);
                }
                else if (this is AI_Base_Movement && (kvp.Key != null))
                {
                    CameraController.Instance.SetCamToU(currentUnit, currentUnit.transform.position != kvp.Key.transform.position);
                }
                PerformTheAction(currentUnit, kvp);                
            }

        }
        if (this is AI_Attack)
        {
            //CombatController.Instance.MakeAIWait(3f);
        }
        else
        {
            //CombatController.Instance.MakeAIWait(1f);
        }
    }

    bool CheckIfGoodToUseAbility(BattlescapeLogic.Unit currentUnit)
    {
        foreach (Ability_Basic ability in currentUnit.GetComponents<Ability_Basic>())
        {
            if (ability.IsUsableNowBase() && ability.AI_IsGoodToUseNow())
            {
                ability.AI_Activate(ability.AI_ChooseTarget());
                return true;
            }
        }
        return false;
    }

    bool IsInInfiniteLoop(BattlescapeLogic.Unit unit)
    {
        int count = 0;
        foreach (BattlescapeLogic.Unit u in LastThreeSelectedUnits)
        {
            if (unit = u)
            {
                count++;
            }
        }
        return (count == 3);
    }

    public virtual List<Tile> GetPossibleMoves(BattlescapeLogic.Unit currentUnit, bool isAlly)
    {
        return null;
    }

    public virtual IEnumerator EvaluatePossibleMoves(BattlescapeLogic.Unit currentUnit, List<Tile> possibleMoves)
    {
        yield return null;
    }

    protected virtual void PerformTheAction(BattlescapeLogic.Unit currentUnit, KeyValuePair<Tile, float> target)
    {
        return;
    }

    protected virtual Queue<BattlescapeLogic.Unit> GetPossibleUnits()
    {
        return null;
    }

    public KeyValuePair<Tile, float> GetTheMove(BattlescapeLogic.Unit currentUnit, Dictionary<Tile, float> allPossibleOptionsEvaluated)
    {
        List<Tile> ThingsToRandomlyPickFrom = new List<Tile>();
        float highestValue = -Mathf.Infinity;
        float lowestValue = Mathf.Infinity;
        foreach (KeyValuePair<Tile, float> pair in allPossibleOptionsEvaluated)
        {
            if (pair.Value > highestValue)
            {
                highestValue = pair.Value;
            }
            if (pair.Value < lowestValue)
            {
                lowestValue = pair.Value;
            }
        }
        foreach (KeyValuePair<Tile, float> pair in allPossibleOptionsEvaluated)
        {
            /*if (realBest)*/
            {
                if (pair.Value == highestValue)
                {
                    ThingsToRandomlyPickFrom.Add(pair.Key);
                }
            }
           /* else if (pair.Value >= (ChooseFromTopPercent / 100) * (highestValue - lowestValue) + lowestValue)
            {
                ThingsToRandomlyPickFrom.Add(pair.Key);
            }*/
        }
        if (ThingsToRandomlyPickFrom.Count > 0)
        {
            float choice = allPossibleOptionsEvaluated[ThingsToRandomlyPickFrom[Random.Range(0, ThingsToRandomlyPickFrom.Count)]];
            Debug.Log("Best move was evaluated as: " + choice);
            return new KeyValuePair<Tile, float>(ThingsToRandomlyPickFrom[Random.Range(0, ThingsToRandomlyPickFrom.Count)], choice);
        }
        else
        {
            return new KeyValuePair<Tile, float>(null, 0);
        }


    }
}
