using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Army", menuName = "AI Army")]
[System.Serializable]

public class AIArmy : ScriptableObject
{
    public string Name;
    public int Points;
    public Unit Hero;
    public List<Unit> Units;
    public BattlescapeLogic.Faction faction;
}
