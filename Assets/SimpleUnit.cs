using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class SimpleUnit : MonoBehaviour {

    public UnitCreator unitCreator;

    public Unit unit { get; private set; }

    private void Start()
    {
        unit = unitCreator.prefab.GetComponent<Unit>();
    }   
}
