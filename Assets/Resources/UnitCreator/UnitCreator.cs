using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[CreateAssetMenu(fileName = "New Unit Creator", menuName = "Unit Creator")]
[System.Serializable]

public class UnitCreator : ScriptableObject
{
    public GameObject prefab;

    public static GameObject FindUnitPrefabByName(string _name)
    {
        foreach (UnitCreator unitCreator in Resources.LoadAll<UnitCreator>("UnitCreator"))
        {
            
            if (unitCreator.name == _name)
            {
                return unitCreator.prefab;
            }
        }
        Debug.LogError("Unit not found!");
        return null;
    }

    //This should be for compatibility with an army, or hero - idk yet ;D parametres might need to change ;D
    public bool IsCompatible(Race race)
    {
        return prefab.GetComponent<Unit>().race == race;        
    }

    public bool IsHero()
    {
        return prefab.GetComponent<Hero>() != null;
    }

    public int GetCost()
    {
        return prefab.GetComponent<Unit>().statistics.cost;
    }
    public int GetLimit()
    {
        return prefab.GetComponent<Unit>().statistics.limit;
    }
}
