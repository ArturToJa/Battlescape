using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HeroNames : MonoBehaviour
{

    [SerializeField] List<string> theHeroNames;
    public static List<string> List;


    public static HeroNames instance;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (List == null)
        {
            List = new List<string>(theHeroNames);
        }
    }

    public string GetRandomHeroName()
    {
        string name = List[Random.Range(0, List.Count)];
        return name;
    }    
}
