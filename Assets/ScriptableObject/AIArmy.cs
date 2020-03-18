using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[CreateAssetMenu(fileName = "New AI Army", menuName = "AI Army")]
[System.Serializable]

public class AIArmy : ScriptableObject
{
    public string Name;
    public int Points;
    public UnitCreator Hero;
    public List<UnitCreator> Units;
    public Race Race;
}
