using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIconsDisplay : MonoBehaviour
{
    [SerializeField] GameObject ArrowPrefab;
    UnitScript myUnit;

    void Start()
    {
        myUnit = GetComponentInParent<UnitScript>();
        CombatController.Instance.AttackEvent += OnAttack;
    }


    public void OnAttack(UnitScript Attacker, UnitScript Defender, int damage)
    {
        if (Attacker == myUnit && TurnManager.Instance.CurrentPhase == TurnPhases.Shooting && myUnit.EnemyList.Count == 0)
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
