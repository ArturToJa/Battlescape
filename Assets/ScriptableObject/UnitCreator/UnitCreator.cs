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
        foreach (UnitCreator unitCreator in SaveLoadManager.Instance.allUnitCreators)
        {
            if (unitCreator.index == index)
            {
                return unitCreator.prefab;
            }
        }
        Debug.LogError("Unit not found!");
        return null;
    }
}
