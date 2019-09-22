using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] Sprite[] FactionSymbols;
    [SerializeField] Sprite[] HeroPortraits;
    public static SaveLoadManager Instance { get; private set; }
    [SerializeField] GameObject SavePrefab;
    [SerializeField] Transform DeploymentPanel;
    public List<Unit> UnitsList;
    public PlayerArmy playerArmy;
    public List<Unit> allPossibleUnits;
    public string currentSaveName;
    public int currentSaveValue = 25;
    public Unit hero;
    public Faction? Race;
    public string HeroName;
    public Faction[] ChosenFactions = new Faction[2];
    public bool AreBothFactionsChosen
    {
        get
        {
            return ChosenFactions[0] != Faction.Neutral && ChosenFactions[1] != Faction.Neutral;
        }
    }
    Faction LocalFaction
    {
        get
        {
            if (PhotonNetwork.isMasterClient)
            {
                return ChosenFactions[0];
            }
            else
            {
                return ChosenFactions[1];
            }
        }
    }
    //^ this is just for multiplayer purpose of setting factions in the lobby. As lobby scene has no central manager script and it is connected in a way to SLM (cause it restricts allowed saves to load for later).... It stays here.

    void Awake()
    {
        ChosenFactions[0] = Faction.Neutral;
        ChosenFactions[1] = Faction.Neutral;
        if (Instance == null)
        {
            Instance = this;
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
        UnitsList = new List<Unit>();

    }

    void Start()
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Singleplayer && Player.Players[0].Type == PlayerType.AI)
        {
            LoadAIArmyToGame(currentSaveValue);
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
        if (Race != null)
        {
            playerArmy.faction = Race;
        }
        if (HeroName != null && HeroName != string.Empty)
        {
            playerArmy.HeroName = HeroName;
        }
        else
        {
            playerArmy.HeroName = HeroNames.GetRandomHeroName();
            HeroName = playerArmy.HeroName;
        }
        if (ArmyBuilder.Instance != null && ArmyBuilder.Instance.Hero != null)
        {
            playerArmy.heroID = ArmyBuilder.Instance.Hero.myUnitID;
        }
        else
        {
            playerArmy.heroID = null;
        }

        playerArmy.unitIDs = new List<UnitID>();
        foreach (Unit u in UnitsList)
        {
            playerArmy.unitIDs.Add(u.myUnitID);
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
        FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>().Okay();
        StartCoroutine(CloseWindow(GameObject.Find("LoadWindowPanel")));
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            GameStateManager.Instance.GetComponent<PhotonView>().RPC("RPCSetHeroName", PhotonTargets.All, TurnManager.Instance.PlayerToMove, HeroName);
        }
        else
        {
            HeroNames.SetHeroName(TurnManager.Instance.PlayerToMove, HeroName);
        }
    }
    public void LoadAIArmyToGame(int points)
    {
        LoadAIArmy(points);
        RecreateUnitsList();
        FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>().Okay();
        HeroNames.SetHeroName(TurnManager.Instance.PlayerToMove, HeroName);
        Player.Players[1].Race = Race;
    }

    public void RecreateUnitsList()
    {
        UnitsList.Clear();
        foreach (UnitID ID in playerArmy.unitIDs)
        {
            foreach (Unit unit in allPossibleUnits)
            {
                if (unit.myUnitID == ID)
                {
                    UnitsList.Add(unit);
                    break;
                }
            }
        }

        foreach (Unit unit in allPossibleUnits)
        {
            if (unit.myUnitID == playerArmy.heroID)
            {
                if (ArmyBuilder.Instance != null)
                {
                    ArmyBuilder.Instance.Hero = unit;
                }
                hero = unit;
                break;
            }
        }


        HeroName = playerArmy.HeroName;
        Race = playerArmy.faction;
    }


    void UpdateArmyBuilderForLoad()
    {
        ArmyBuilder.Instance.UnitsList.Clear();
        ArmyBuilder.Instance.UnitsList = UnitsList;
    }
    public Dictionary<string,string> GetSaveNames(string location)
    {
        Dictionary<string,string> saveNames = new Dictionary<string, string>();
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

    public bool HasRaceSaved(Faction race)
    {
        var list = SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/");
        if (list != null)
        {
            foreach (var item in list)
            {                
                PlayerArmy armyInfo = GetInsidesOfASave(item.Value);
                if (armyInfo.faction == null)
                {
                    Debug.Log("deleted");
                    File.Delete(Application.persistentDataPath + "/Armies/" + SaveLoadManager.Instance.currentSaveValue.ToString() + "points/" + item.Key + ".lemur");                    
                    continue;
                }
                if (armyInfo.faction == race)
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
            HeroName = null;
            Race = null;
            playerArmy = new PlayerArmy();
            UnitsList = new List<Unit>();
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
        var list = SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/");
        if (list != null)
        {
            foreach (var item in list)
            {
                var temp = Instantiate(SavePrefab, ExiSaves);
                temp.GetComponent<SaveLoadButton>().isDeletable = true;
                temp.GetComponentInChildren<Text>().text = item.Key;
                PlayerArmy armyInfo = GetInsidesOfASave(item.Value);
                if (armyInfo.faction == null)
                {
                    Debug.Log("deleted");
                    File.Delete(Application.persistentDataPath + "/Armies/" + SaveLoadManager.Instance.currentSaveValue.ToString() + "points/" + item.Key + ".lemur");
                    Destroy(temp);
                    continue;
                }
                if (GameStateManager.Instance.MatchType == MatchTypes.Online && armyInfo.faction != LocalFaction)
                {
                    Debug.Log("Skipped");
                    Destroy(temp);
                    continue;
                }
                temp.GetComponentsInChildren<Image>()[1].sprite = GetRaceSprite((Faction)armyInfo.faction);
                //temp.GetComponentsInChildren<Image>()[2].sprite = FactionSymbols[(int)armyInfo.heroID];
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
            case Faction.Elves:
                name = "E";
                break;
            case Faction.Human:
                name = "H";
                break;
            case Faction.Neutral:
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

    public Sprite GetRaceSprite(Faction race)
    {
        return FactionSymbols[(int)race];
    }

}

[System.Serializable]
public class PlayerArmy
{
    public string HeroName;
    public List<UnitID> unitIDs;
    public UnitID? heroID;
    public Faction? faction;
}

[System.Serializable]
public enum UnitID
{
    // Rhino is Llha'ran. XD.
    Warrior, Ranger, Knight, Swordman, Archer, Pikeman, Horseman, Catapult, Knights, IG, Marksman, Wolf, Hunter, Rhino, Raven, Fencers, Riders, Assassins
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
