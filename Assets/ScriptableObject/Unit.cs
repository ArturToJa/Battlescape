using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
[System.Serializable]
public class Unit : ScriptableObject {

    public string Name;
    public int Cost;
    public int LimitFor50;
    public GameObject thisUnitFirstPlayer;
    public GameObject thisUnitSecondPlayer;
    public RenderTexture thisSprite;
    [SerializeField] Sprite HumanHeroSprite;
    [SerializeField] Sprite ElvesHeroSprite;
    public Sprite ThisRealSprite
    {
       get
        {
            switch (SaveLoadManager.Instance.Race)
            {
                case Faction.Elves:
                    return ElvesHeroSprite;
                case Faction.Human:
                    return HumanHeroSprite;
                case Faction.Neutral:
                    Debug.LogError("WTF why the race is neutral");
                    return null;
                default:
                    return null;
            }            
        }
    }
    public GameObject thisBox;
    public UnitID myUnitID;
    public string fluffBio;
    public Faction faction;

    public static Unit FindUnitByID(UnitID? ID)
    {
        foreach (Unit unit in SaveLoadManager.Instance.allPossibleUnits)
        {
            if (unit.myUnitID == ID)
            {
                return unit;
            }
        }
        Debug.LogError("why no unit was returned? its a bug");
        return null;
    }

    public static GameObject FindUnitsObjectByIntID(int UnitID, int PlayerID)
    {
        foreach (Unit unit in SaveLoadManager.Instance.allPossibleUnits)
        {
            if ((int)unit.myUnitID == UnitID)
            {
                if (PlayerID == 0)
                {
                    return unit.thisUnitFirstPlayer;
                }
                else
                {
                    return unit.thisUnitSecondPlayer;
                }
            }
        }
        Debug.LogError("why no unit was returned? its a bug");
        return null;
    }
}
