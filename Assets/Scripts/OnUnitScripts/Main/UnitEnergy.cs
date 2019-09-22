using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnergy : MonoBehaviour
{
    UnitScript me;
    public int MaxEnergy { get; private set; }
    int StartingEnergy;
    int EnergyPerTurn = 15;
    [HideInInspector]public int CurrentEnergy;

    private void Start()
    {
        MaxEnergy = 100;
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        me = GetComponent<UnitScript>();
        StartingEnergy = (int)(0.5f * MaxEnergy+0.5f);
        CurrentEnergy = StartingEnergy;
    }

    void OnNewTurn()
    {
        if (TurnManager.Instance.TurnCount > 2 && TurnManager.Instance.PlayerHavingTurn == me.PlayerID)
        {
            CurrentEnergy += EnergyPerTurn;
            if (me.CheckIfIsInCombat() == false)
            {
                CurrentEnergy += EnergyPerTurn;
            }
            if (CurrentEnergy >= MaxEnergy)
            {
                CurrentEnergy = MaxEnergy;
            }
        }

    }

    public bool IsEnoughEnergyFor(Ability_Basic Ability)
    {
        return Ability.EnergyCost <= CurrentEnergy;
    }
}
