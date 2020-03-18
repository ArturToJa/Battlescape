using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class DisallowUnownedRaces : MonoBehaviour
{

    [SerializeField] Transform RaceGrid;

    private void Start()
    {
        for (int i = 0; i < RaceGrid.childCount; i++)
        {
            if (SaveLoadManager.instance.HasRaceSaved((Race)i) == false)
            {
                RaceGrid.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
