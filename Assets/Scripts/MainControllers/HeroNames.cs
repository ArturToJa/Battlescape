using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNames : MonoBehaviour
{

    [SerializeField] List<string> theHeroNames;
    public static List<string> List;
    public static string[] PlayerHeroNames;


    // Use this for initialization
    void Start()
    {
        PlayerHeroNames = new string[2];
        if (List == null)
        {
            List = new List<string>(theHeroNames);
        }
    }

    public static void SetHeroName(int ID, string heroName)
    {
        PlayerHeroNames[ID] = heroName;
    }

    public static string GetHeroName()
    {
        return PlayerHeroNames[TurnManager.Instance.PlayerToMove];
    }

    public static string GetRandomHeroName()
    {
        string name = List[Random.Range(0, List.Count)];

        while (PlayerHeroNames[0] == name || PlayerHeroNames[1] == name)
        {
            name = List[Random.Range(0, List.Count)];
        }
        return name;
    }

    public static void SetAIHeroesNameDifferent()
    {
        if (GameStateManager.Instance.MatchType != MatchTypes.Singleplayer)
        {
            // we have no AI so both players used same name, sorry, it stays lol.
            return;
        }
        GameObject heroObject = null;
        foreach (HeroScript hero in FindObjectsOfType<HeroScript>())
        {
            if (Player.Players[hero.GetComponent<UnitScript>().PlayerID].Type == PlayerType.AI)
            {
                heroObject = hero.gameObject;
            }
        }
        string newName = null;
        PlayerHeroNames[heroObject.GetComponent<UnitScript>().PlayerID] = GetRandomHeroName();
        newName = PlayerHeroNames[heroObject.GetComponent<UnitScript>().PlayerID];
        heroObject.name = heroObject.GetComponent<UnitScript>().unitUnit.Name + " " + newName;
    }
}
