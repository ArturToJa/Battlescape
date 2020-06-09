using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[CreateAssetMenu(fileName = "New Unit Creator", menuName = "Unit Creator")]
[System.Serializable]

public class UnitCreator : ScriptableObject
{
    public GameObject prefab;    
    public int index;

    public static GameObject FindUnitPrefabByIndex(int index)
    {
        foreach (UnitCreator unitCreator in SaveLoadManager.instance.allUnitCreators)
        {
            if (unitCreator.index == index)
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
