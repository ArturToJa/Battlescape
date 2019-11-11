using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

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
        return PlayerHeroNames[Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].team.index];
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
}
