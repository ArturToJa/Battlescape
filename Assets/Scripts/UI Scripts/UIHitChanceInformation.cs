using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHitChanceInformation : MonoBehaviour
{

    int defendersDefenceBonus;
    Text theText;
    HitChancer hc;
    UnitScript neededTarget;
    UnitScript currentTarget;
    UnitScript Attacker;

    void Awake()
    {
        theText = this.gameObject.GetComponent<Text>();
        theText.text = "";
    }

    void OnEnable()
    {
        neededTarget = null;
    }

    void Update()
    {
        if (GameStateManager.Instance.IsCurrentPlayerLocal() == false)
        {
            UIManager.SmoothlyTransitionActivity(transform.parent.gameObject, false, 0.1f);
            //i might be checking for it sometimes two times redundantly - but it looks more obvious to me that way :D and i dont need to nervously check if i really check for taht somewhere else
            return;
        }
        SetTargetAndAttacker();
        CheckIfIAmNeeded();
        CreateAHitChancer();
    }

    void SetTargetAndAttacker()
    {
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState)
        {
            Attacker = CombatController.Instance.AttackTarget;
            neededTarget = CombatController.Instance.AttackingUnit;
        }
        else
        {
            Attacker = MouseManager.Instance.SelectedUnit;
            neededTarget = MouseManager.Instance.MouseoveredUnit;
        }
    }

    void LateUpdate()
    {
        if (GameStateManager.Instance.IsCurrentPlayerLocal() == false)
        {
            return;
        }
        if (CheckIfNeedToUpdate())
        {
            ShowNewInformation();
        }
    }

    public bool CheckIfNeedToUpdate()
    {
        if (!IsGameStateViable())
        {
            return false;
        }
        if (transform.parent.GetComponent<CanvasGroup>().alpha < 0.1f)
        {
            return false;
        }
        if (neededTarget != currentTarget)
        {
            if ((neededTarget != null) && (neededTarget.PlayerID != Attacker.PlayerID))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        return false;
    }

    public bool IsNewTargetToCalculate()
    {
        if (MouseManager.Instance.MouseoveredUnit == null)
        {
            return false;
        }
        else
        {
            return neededTarget != currentTarget;
        }
    }
    // Next finction means: if AI is not actively playing at the second,. we are not in movepment phase (or we are but ability is being cast), we also are not in animation or in idle, return true. Otherwise, returns false.
    private bool IsGameStateViable()
    {
        return (GameStateManager.Instance.IsCurrentPlayerAI() == false && GameStateManager.Instance.GameState != GameStates.IdleState && GameStateManager.Instance.GameState != GameStates.AnimatingState && !(TurnManager.Instance.CurrentPhase == TurnPhases.Movement && GameStateManager.Instance.GameState != GameStates.TargettingState));
    }

    private void CreateAHitChancer()
    {
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState)
        {
            hc = new HitChancer(CombatController.Instance.AttackTarget, CombatController.Instance.AttackingUnit, 5000);
        }
        else
        {
            hc = new HitChancer(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit, 5000);
        }
    }

    public void ShowNewInformation()
    {
        bool badRange;
        if (GameStateManager.Instance.GameState == GameStates.ShootingState)
        {
            badRange = (MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>() != null && MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>().CheckBadRange(MouseManager.Instance.MouseoveredUnit.gameObject));
        }
        else
        {
            badRange = false;
        }
        SetUnitStats();
        float missChance = hc.MissChance(badRange);
        float hitChance = hc.HitNoDamageChance(badRange);
        //float avgDmg = hc.AverageDamage(badRange);
        theText.text = "Chances for:";
        theText.text += "\n" + "Miss (reducing Defence): " + (missChance + hitChance).ToString() + "%";
        //theText.text += "\n" + "Hit (no Damage): " + hitChance.ToString() + "%";
        theText.text += "\n" + "Hit (dealing Damage): " + (100 - missChance - hitChance).ToString() + "%";
        int[] MinMax = hc.GetMinMaxDmg();
        theText.text += "\n" + "\n" + "Damage if hit: " + (MinMax[0]).ToString() + " - " + (MinMax[1]).ToString();
        UndoUnitStats();
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
        {
            int shelter = MouseManager.Instance.MouseoveredUnit.HowMuchShelteredFrom(MouseManager.Instance.SelectedUnit.transform.position);
            if (shelter > 0)
            {
                theText.text += "\n" + "Target's shelter increases his defence by: " + shelter.ToString();
            }
        }
        currentTarget = neededTarget;
    }

    private void CheckIfIAmNeeded()
    {
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState && GameStateManager.Instance.IsCurrentPlayerLocal())
        {
            //we, the local player, are the active player during retaliation. Therefore we NEED to have the window open.
            UIManager.SmoothlyTransitionActivity(transform.parent.gameObject, true, 0.1f);
        }
        else if (MouseManager.Instance.SelectedUnit != null && (GameStateManager.Instance.GameState == GameStates.AttackState && !MouseManager.Instance.SelectedUnit.hasAttacked && MouseManager.Instance.SelectedUnit.CanAttack || GameStateManager.Instance.GameState == GameStates.ShootingState && MouseManager.Instance.SelectedUnit.isRanged && MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>().CanShoot) && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.PlayerID != MouseManager.Instance.MouseoveredUnit.PlayerID)
        {
            // we have a selected unit, it can attack/shoot NOW and we are over a mouseovered enemy. Therefore we imply we are a local active player ;).
            UIManager.SmoothlyTransitionActivity(transform.parent.gameObject, true, 0.1f);
        }
        else
        {
            UIManager.SmoothlyTransitionActivity(transform.parent.gameObject, false, 0.1f);
        }
    }

    public void ChangeUnitsStatsBy(int DD)
    {
        defendersDefenceBonus = DD;
    }

    void SetUnitStats()
    {
        hc.Defender.CurrentDefence += defendersDefenceBonus;
    }

    void UndoUnitStats()
    {
        hc.Defender.CurrentDefence -= defendersDefenceBonus;
    }
}
