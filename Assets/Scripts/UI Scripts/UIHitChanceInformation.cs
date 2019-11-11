using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UIHitChanceInformation : MonoBehaviour
{

    int defendersDefenceBonus;
    Text theText;
    Unit neededTarget;
    Unit currentTarget;
    Unit Attacker;

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
    }

    void SetTargetAndAttacker()
    {
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState)
        {
            Attacker = CombatController.Instance.attackTarget;
            neededTarget = CombatController.Instance.attackingUnit;
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
            if ((neededTarget != null) && (neededTarget.owner != Attacker.owner))
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

    public void ShowNewInformation()
    {
        currentTarget = neededTarget;
        SetUnitStats();
        float hitChance = DamageCalculator.HitChance(Attacker, currentTarget);
        theText.text = "Chances for:";
        theText.text += "\n" + "Miss (reducing Defence): " + ((1-hitChance) * 100).ToString() + "%";
        theText.text += "\n" + "Hit (dealing Damage): " + (hitChance * 100).ToString() + "%";
        int avgDmg = Statistics.baseDamage + DamageCalculator.GetStatisticsDifference(Attacker, currentTarget);
        int dmgRange = avgDmg / 5;
        theText.text += "\n" + "\n" + "Damage if hit: " + (avgDmg - dmgRange).ToString() + " - " + (avgDmg + dmgRange).ToString();
        UndoUnitStats();

        int shelter = CombatController.Instance.HowMuchShelteredFrom(MouseManager.Instance.MouseoveredUnit, MouseManager.Instance.SelectedUnit.transform.position);
        if (shelter > 0)
        {
            theText.text += "\n" + "Target's shelter increases his defence by: " + shelter.ToString();
        }
    }

    private void CheckIfIAmNeeded()
    {
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState && GameStateManager.Instance.IsCurrentPlayerLocal())
        {
            //we, the local player, are the active player during retaliation. Therefore we NEED to have the window open.
            UIManager.SmoothlyTransitionActivity(transform.parent.gameObject, true, 0.1f);
        }
        else if (MouseManager.Instance.SelectedUnit != null && (GameStateManager.Instance.GameState == GameStates.AttackState && MouseManager.Instance.SelectedUnit.CanStillAttack() || GameStateManager.Instance.GameState == GameStates.AttackState && MouseManager.Instance.SelectedUnit.IsRanged() && MouseManager.Instance.SelectedUnit.CanStillAttack() && MouseManager.Instance.SelectedUnit.IsInCombat() == false) && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.owner != MouseManager.Instance.MouseoveredUnit.owner)
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
        currentTarget.statistics.bonusDefence += defendersDefenceBonus;
    }

    void UndoUnitStats()
    {
        currentTarget.statistics.bonusDefence -= defendersDefenceBonus;
    }
}
