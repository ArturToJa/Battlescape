using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIconsDisplay : MonoBehaviour
{
    [SerializeField] GameObject ArrowPrefab;
    BattlescapeLogic.Unit myUnit;

    void Start()
    {
        myUnit = GetComponentInParent<BattlescapeLogic.Unit>();
        CombatController.Instance.AttackEvent += OnAttack;
    }


    public void OnAttack(BattlescapeLogic.Unit Attacker, BattlescapeLogic.Unit Defender)
    {
        if (Attacker == myUnit && TurnManager.Instance.CurrentPhase == TurnPhases.Attack && myUnit.IsInCombat() == false)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            } 
            else
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}
