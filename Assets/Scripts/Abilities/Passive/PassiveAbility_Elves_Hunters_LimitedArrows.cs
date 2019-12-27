using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbility_Elves_Hunters_LimitedArrows : PassiveAbility
{
    [SerializeField] int StartingArrows;
    [HideInInspector] public int CurrentArrows;
    Text TextCurrentArrowsReal;
    Text TextCurrentArrowsUnreal;
    Ability_Neutral_SwapWeapons swapWeapons;


    protected override void ChangableStart()
    {
        CurrentArrows = StartingArrows;
        swapWeapons = GetComponent<Ability_Neutral_SwapWeapons>();
        //CombatController.Instance.AttackEvent += OnAttack;
    }

    protected override void ChangableUpdate()
    {
        if (TextCurrentArrowsReal == null && IconForReal != null)
        {
            TextCurrentArrowsReal = IconForReal.GetComponentInChildren<Text>();
        }
        if (TextCurrentArrowsUnreal == null && IconForUnreal != null)
        {
            TextCurrentArrowsUnreal = IconForUnreal.GetComponentInChildren<Text>();
        }
        if (TextCurrentArrowsReal != null)
        {
            TextCurrentArrowsReal.text = CurrentArrows.ToString();
        }
        if (TextCurrentArrowsUnreal != null)
        {
            TextCurrentArrowsUnreal.text = CurrentArrows.ToString();
        }
        if (CurrentArrows <= 0 && swapWeapons.CanBeSwapped)
        {
            StartCoroutine(SwapToMelee());

        }
    }

    public void OnAttack(BattlescapeLogic.Unit Attacker, BattlescapeLogic.Unit Defender)
    {
        if (Attacker == myUnit && TurnManager.Instance.CurrentPhase == TurnPhases.Attack && myUnit.IsInCombat() == false)
        {
            CurrentArrows--;
        }
    }

    IEnumerator SwapToMelee()
    {
        swapWeapons.CanBeSwapped = false;
        SendLogs();
        yield return new WaitForSeconds(2f);
        swapWeapons.DoSwap();
    }

    void SendLogs()
    {
        PopupTextController.AddPopupText("No more arrows!", PopupTypes.Info);
        Log.SpawnLog("Hunter has no more arrows!");

    }

    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        return 0;
    }

    public override int GetDefence(BattlescapeLogic.Unit other)
    {
        return 0;
    }
}
