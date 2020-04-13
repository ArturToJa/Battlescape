using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BattlescapeLogic;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] Sprite[] raceSymbols;
    public static SaveLoadManager instance { get; private set; }
    [SerializeField] GameObject savePrefab;
    public List<UnitCreator> unitsList { get; set; }
    public PlayerArmy playerArmy { get; set; }
    public string currentSaveName { get; set; }
    public int currentSaveValue { get; set; }
    public UnitCreator hero { get; set; }
    public Race race { get; set; }
    public string heroName { get; set; }
    //public Race[] ChosenRaces = new Race[2];
    public List<UnitCreator> allUnitCreators;
    public bool haveAllPlayersChosenRace
    {
        get
        {
            foreach (PlayerBuilder playerBuilder in Global.instance.playerBuilders)
            {
                if (playerBuilder.race == Race.Neutral)
                {
                    return false;
                }
            }
            return true;
        }
    }
    Race localRace
    {
        get
        {
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                foreach (Player player in team.players)
                {
                    if (player.type == PlayerType.Local)
                    {
                        return player.race;
                    }
                }
            }
            Debug.LogError("No local player found!");
            return Race.Neutral;
        }
    }
    //^ this is just for multiplayer purpose of setting Races in the lobby. As lobby scene has no central manager script and it is connected in a way to SLM (cause it restricts allowed saves to load for later).... It stays here.

    void Awake()
    {
        currentSaveValue = 25;
        race = Race.Neutral;
        //ChosenRaces[0] = Race.Neutral;
        //ChosenRaces[1] = Race.Neutral;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (Application.isEditor)
            {
                DestroyImmediate(this);
            }
            else
            {
                Destroy(this);
            }
        }


        playerArmy = new PlayerArmy();
        unitsList = new List<UnitCreator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (Global.instance.matchType == MatchTypes.Singleplayer && Global.instance.playerTeams[0].players[0].type == PlayerType.AI)
        {
            LoadAIArmyToGame(Global.instance.playerBuilders[0], currentSaveValue);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("_GameScene_"))
        {
            Global.instance.SetMap(scene.name);
        }
    }


    public void GetInput(string input)
    {
        currentSaveName = input;
    }

    public void Save()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Armies") == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Armies");
        }
        if (Directory.Exists(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points") == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points");
        }
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur"))
        {
            File.Delete(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur");
        }
        FileStream file = File.Create(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points/" + currentSaveName /*+ GetRaceHeroPair()*/ + ".lemur");
        CopyDataToSave();
        bf.Serialize(file, playerArmy);
        file.Close();
    }

    public void CopyDataToSave()
    {
        playerArmy = new PlayerArmy();
        if (race != Race.Neutral)
        {
            playerArmy.Race = race;
        }
        if (heroName != null && heroName != string.Empty)
        {
            playerArmy.HeroName = heroName;
        }
        else
        {
            playerArmy.HeroName = HeroNames.GetRandomHeroName();
            heroName = playerArmy.HeroName;
        }
        if (ArmyBuilder.instance != null && ArmyBuilder.instance.heroCreator != null)
        {
            playerArmy.heroIndex = ArmyBuilder.instance.heroCreator.index;
        }
        else
        {
            playerArmy.heroIndex = -1;
        }

        playerArmy.unitIndecies = new List<int>();
        foreach (UnitCreator unitCreator in unitsList)
        {
            playerArmy.unitIndecies.Add(unitCreator.index);
        }
    }
    public void LoadPlayerArmy()
    {
        if (File.Exists(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Armies/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur", FileMode.Open);
            playerArmy = bf.Deserialize(file) as PlayerArmy;
            file.Close();
        }
        /* else if (File.Exists(Application.persistentDataPath + "/Armies/AI/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur"))
         {
             BinaryFormatter bf = new BinaryFormatter();
             FileStream file = File.Open(Application.persistentDataPath + "/Armies/AI/" + currentSaveValue.ToString() + "points/" + currentSaveName + ".lemur", FileMode.Open);
             playerArmy = bf.Deserialize(file) as PlayerArmy;
             file.Close();
         }*/
        else
        {
            Debug.LogError("No such thing to load!");
        }
    }
    public void LoadAIArmy(int points)
    {
        if (Directory.Exists(Application.persistentDataPath + "/Armies/AI/" + currentSaveValue.ToString() + "points"))
        {
            var files = Directory.GetFiles(Application.persistentDataPath + "/Armies/AI/" + currentSaveValue.ToString() + "points", "*.lemur");
            string fileName = files[Random.Range(0, files.Length)];
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            playerArmy = bf.Deserialize(file) as PlayerArmy;
            file.Close();
        }
        else
        {
            Application.Quit();
            throw new System.Exception("My mum just boom");
        }
    }
    public void LoadArmyToGame()
    {
        LoadPlayerArmy();
        RecreateUnitsList();
        FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>().LoadPlayerToGame();
        StartCoroutine(CloseWindow(GameObject.Find("LoadWindowPanel")));
        if (Global.instance.matchType == MatchTypes.Online)
        {
            NetworkMessageSender.Instance.SetHeroName(GameRound.instance.currentPlayer.team.index, heroName);
        }
        else
        {
            HeroNames.SetHeroName(GameRound.instance.currentPlayer.team.index, heroName);
        }
    }
    public void LoadAIArmyToGame(PlayerBuilder currentPlayerBuilder, int points)
    {
        LoadAIArmy(points);
        RecreateUnitsList();
        FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>().LoadPlayerToGame();
        HeroNames.SetHeroName(GameRound.instance.currentPlayer.team.index, heroName);
        currentPlayerBuilder.race = (Race)race;
    }

    public void RecreateUnitsList()
    {
        unitsList.Clear();
        foreach (int index in playerArmy.unitIndecies)
        {
            unitsList.Add(GetUnitCreatorFromIndex(index));
        }
        if (playerArmy.heroIndex == -1)
        {
            Debug.LogError("EmptyArmy!");
            return;
        }
        hero = GetUnitCreatorFromIndex(playerArmy.heroIndex);
        if (ArmyBuilder.instance != null)
        {
            ArmyBuilder.instance.heroCreator = hero;
        }
        heroName = playerArmy.HeroName;
        race = playerArmy.Race;
    }

    public UnitCreator GetUnitCreatorFromIndex(int index)
    {
        foreach (UnitCreator unitCreator in allUnitCreators)
        {
            if (unitCreator.index == index)
            {
                return unitCreator;
            }
        }
        Debug.LogError("No unitcreators of set index in the all-list!");
        return null;
    }

    public Dictionary<string, string> GetSaveNames(string location)
    {
        Dictionary<string, string> saveNames = new Dictionary<string, string>();
        DirectoryInfo dir = new DirectoryInfo(location + currentSaveValue.ToString() + "points");
        if (dir.Exists == false)
        {
            return null;
        }
        FileInfo[] info = dir.GetFiles("*.lemur");
        foreach (FileInfo f in info)
        {
            saveNames.Add(Path.GetFileNameWithoutExtension(f.FullName), Path.GetFullPath(f.FullName));
        }
        return saveNames;
    }

    public bool HasRaceSaved(Race race)
    {
        var list = SaveLoadManager.instance.GetSaveNames(Application.persistentDataPath + "/Armies/");
        if (list != null)
        {
            foreach (var item in list)
            {
                PlayerArmy armyInfo = GetInsidesOfASave(item.Value);
                if (armyInfo.Race == Race.Neutral)
                {
                    Debug.Log("deleted");
                    File.Delete(Application.persistentDataPath + "/Armies/" + SaveLoadManager.instance.currentSaveValue.ToString() + "points/" + item.Key + ".lemur");
                    continue;
                }
                if (armyInfo.Race == race)
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator CloseWindow(GameObject Window)
    {
        while (Window.GetComponent<CanvasGroup>().alpha > 0.1f)
        {
            UIManager.SmoothlyTransitionActivity(Window, false, 0.1f);
        }
        yield return null;
        Window.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void SaveArmyButton()
    {
        if (currentSaveName == null)
        {
            Debug.LogError("this shouldnt have happenned. We HAVE TO already have a name");
        }
        else
        {
            Save();
            currentSaveName = null;
            hero = null;
            heroName = null;
            race = Race.Neutral;
            playerArmy = new PlayerArmy();
            unitsList = new List<UnitCreator>();
            FindObjectOfType<WindowSetter>().GoBack();
        }
    }

    /// <summary>
    /// Creates VISUAL buttons representing saves
    /// </summary>
    public void ReCreateSaves(Transform ExiSaves)
    {

        while (ExiSaves.childCount > 0)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(ExiSaves.GetChild(0).gameObject);
            }
            else
            {
                Destroy(ExiSaves.GetChild(0).gameObject);
            }
        }
        var list = SaveLoadManager.instance.GetSaveNames(Application.persistentDataPath + "/Armies/");
        if (list != null)
        {
            foreach (var item in list)
            {
                var temp = Instantiate(savePrefab, ExiSaves);
                temp.GetComponent<SaveLoadButton>().isDeletable = true;
                temp.GetComponentInChildren<Text>().text = item.Key;
                PlayerArmy armyInfo = GetInsidesOfASave(item.Value);
                if (armyInfo.Race == Race.Neutral)
                {
                    Debug.Log("deleted");
                    File.Delete(Application.persistentDataPath + "/Armies/" + SaveLoadManager.instance.currentSaveValue.ToString() + "points/" + item.Key + ".lemur");
                    Destroy(temp);
                    continue;
                }
                if (Global.instance.matchType == MatchTypes.Online && armyInfo.Race != localRace)
                {
                    Debug.Log("Skipped");
                    Destroy(temp);
                    continue;
                }
                temp.GetComponentsInChildren<Image>()[1].sprite = GetRaceSprite((Race)armyInfo.Race);
                //temp.GetComponentsInChildren<Image>()[2].sprite = RaceSymbols[(int)armyInfo.heroID];
                temp.name = item.Key;
            }
        }
    }

    PlayerArmy GetInsidesOfASave(string saveName)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(saveName, FileMode.Open);
        PlayerArmy save = bf.Deserialize(file) as PlayerArmy;
        file.Close();
        return save;
    }

    /* string GetRaceHeroPair()
     {
         string name= "";
         switch (Race)
         {
             case Race.Elves:
                 name = "E";
                 break;
             case Race.Human:
                 name = "H";
                 break;
             case Race.Neutral:
                 name = "N";
                 Debug.LogWarning("This is neutral army!");
                 break;
             default:
                 Debug.LogError("NEW RACE EXISTS, FIX IT NOW!");
                 break;
         }
         switch (hero.myUnitID)
         {
             case UnitID.Warrior:
                 name += "W";
                 break;
             case UnitID.Ranger:
                 name += "R";
                 break;
             case UnitID.Knight:
                 name += "K";
                 break;
             default:
                 Debug.LogError("NEW HERO CLASS EXISTS, FIX IT NOW!");
                 break;
         }
         return name;
     }*/

    public Sprite GetRaceSprite(Race race)
    {
        return raceSymbols[(int)race];
    }

}

[System.Serializable]
public class PlayerArmy
{
    public string HeroName;
    public List<int> unitIndecies;
    public int heroIndex;
    public Race Race;
}

[System.Serializable]
public struct LemurVector2
{
    public int x;
    public int z;
    public LemurVector2(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}
