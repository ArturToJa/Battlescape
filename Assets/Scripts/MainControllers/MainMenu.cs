using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool forceOnlineGameEvenThoughNoRace = false;
    // this flag allows player to say "yes i want to play even though i dont have some races"

    [SerializeField] Slider rotation;
    [SerializeField] Slider speed;
    [SerializeField] Slider animSpeed;
    [SerializeField] GameObject YRotCheckbox;
    [SerializeField] GameObject ToggleNextPhaseConfirmationCheckbox;
    bool canChangeYRotation = false;
    float panSpeed = 10;
    float rotationSpeed = 4;
    float moveSpeedAnim = 1f;
    float timeTillTooltip = 0.5f;
    //float timeTillTooltip;
    bool singlePlayer = false;
    bool isGreenAI = false;

    [SerializeField] List<AIArmy> AIArmies;
    [SerializeField] GameObject NoArmyPopupWindow;
    [SerializeField] GameObject LackingRaceSavesPopupWindow;
    [SerializeField] Text LackingRacesText;
    string gameSceneName;
    [SerializeField] List<string> GameScenes;

    public static MainMenu instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        PlayerPrefs.SetFloat("TimeTillOpenTooltip", timeTillTooltip);
        ToggleNextPhaseConfirmationCheckbox.SetActive(PlayerPrefs.HasKey("SkipNextPhaseNotification") && PlayerPrefs.GetInt("SkipNextPhaseNotification") == 1);
        if (PlayerPrefs.HasKey("panSpeed") == false || PlayerPrefs.HasKey("moveSpeedAnimation") == false || PlayerPrefs.GetFloat("moveSpeedAnimation") <= 0)
        {
            DefaultOptions();
        }

        canChangeYRotation = (PlayerPrefs.GetInt("canChangeYRotation") == 1);
        panSpeed = PlayerPrefs.GetFloat("panSpeed");
        rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed");
        moveSpeedAnim = PlayerPrefs.GetFloat("moveSpeedAnimation");
    }
    void Update()
    {
        animSpeed.value = moveSpeedAnim;
        rotation.value = rotationSpeed;
        speed.value = panSpeed;
        YRotCheckbox.SetActive(canChangeYRotation);

    }



    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangePanSpeed(float newSpeed)
    {
        panSpeed = newSpeed;
    }
    public void ChangeMSAnim(float newSpeed)
    {
        moveSpeedAnim = newSpeed;
    }
    public void ChangeRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
    public void ChangeYRotBool(bool b)
    {
        canChangeYRotation = b;
    }
    public void ChangeTimeTillTooltipsShowUp(float f)
    {
        timeTillTooltip = f;
    }
    public void OKOptions()
    {
        if (canChangeYRotation)
        {
            PlayerPrefs.SetInt("canChangeYRotation", 1);
        }
        else
        {
            PlayerPrefs.SetInt("canChangeYRotation", 0);
        }
        PlayerPrefs.SetFloat("moveSpeedAnimation", moveSpeedAnim);
        PlayerPrefs.SetFloat("panSpeed", panSpeed);
        PlayerPrefs.SetFloat("rotationSpeed", rotationSpeed);
        PlayerPrefs.SetFloat("TimeTillOpenTooltip", timeTillTooltip);
        PlayerPrefs.Save();
    }
    public void DefaultOptions()
    {
        canChangeYRotation = false;
        panSpeed = 10;
        rotationSpeed = 4;
        moveSpeedAnim = 2;
        timeTillTooltip = 0.5f;
        OKOptions();
    }
    public void ManageArmy()
    {        
        Networking.instance.SendCommandToLoadScene("_ManagementScene");
    }

    public void SwitchAISide()
    {
        isGreenAI = !isGreenAI;
    }
    public void SetSinglePlayer(bool single)
    {
        singlePlayer = single;
    }

    public void StartQuickMatch()
    {
        Global.instance.matchType = MatchTypes.Singleplayer;
        if (Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath) == null || Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath).Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }

        CreatePlayerBuilders(PlayerType.Local);
        Global.instance.playerBuilders[System.Convert.ToInt32(!isGreenAI)].type = PlayerType.AI;
        
        PlayGameScene();
    }

    private void PlayGameScene()
    {
        if (GameScenes.Contains(gameSceneName) == false)
        {
            gameSceneName = GameScenes[Random.Range(0, GameScenes.Count)];
        }
        Networking.instance.SendCommandToLoadScene(gameSceneName);
    }

    public void StartHotSeat()
    {
        Global.instance.matchType = MatchTypes.HotSeat;
        if (Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath) == null || Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath).Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }
        CreatePlayerBuilders(PlayerType.Local);
        
        if (gameSceneName == null)
        {
            gameSceneName = GameScenes[Random.Range(0, GameScenes.Count)];
        }
        Networking.instance.SendCommandToLoadScene(gameSceneName);
    }

    void CreatePlayerBuilders(PlayerType type)
    {
        for (int i = 0; i < Global.instance.playerCount; i++)
        {
            PlayerBuilder playerBuilder = new PlayerBuilder();
            playerBuilder.index = 0;
            playerBuilder.colour = (PlayerColour)i;
            playerBuilder.playerName = playerBuilder.colour.ToString() + " Player";
            playerBuilder.race = Race.Neutral;
            playerBuilder.type = type;
            playerBuilder.team = Global.instance.playerTeams[i];
            Global.instance.playerBuilders.Add(playerBuilder);
        }
    }

    public void StartOnlineMatch()
    {
        if (Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath) == null || Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath).Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }
        if (forceOnlineGameEvenThoughNoRace == false)
        {
            List<Race> racesLackingSaves = new List<Race>();
            bool showPopup = false;
            foreach (Race race in System.Enum.GetValues(typeof(Race)))
            {
                if (race == Race.Neutral)
                {
                    continue;
                }
                if (Global.instance.armySavingManager.HasArmyOfRace(race) == false)
                {
                    showPopup = true;
                    racesLackingSaves.Add(race);
                    ShowNoParticularRacePopupWindow(racesLackingSaves);
                }
            }
            if (showPopup == true)
            {
                return;
            }
        }
        MyNetworkManager.Instance.Connect();
        Global.instance.matchType = MatchTypes.Online;
        CreatePlayerBuilders(PlayerType.Network);
       
        Networking.instance.SendCommandToLoadScene("_LobbyScene");
    }

    public void ToggleNextPhaseConfirmation()
    {
        if (PlayerPrefs.HasKey("SkipNextPhaseNotification") == false || PlayerPrefs.GetInt("SkipNextPhaseNotification") == 0)
        {
            PlayerPrefs.SetInt("SkipNextPhaseNotification", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SkipNextPhaseNotification", 0);
        }

    }

    void ShowNoParticularRacePopupWindow(List<Race> races)
    {
        LackingRaceSavesPopupWindow.SetActive(true);
        LackingRacesText.text = "You do not have saves for these Races: ";
        if (races.Count > 1)
        {
            for (int i = 0; i < races.Count - 1; i++)
            {
                LackingRacesText.text += races[i] + ", ";
            }
        }
        LackingRacesText.text += races[races.Count - 1] + ". Do you still want to continue?";
    }

    public void ForcefullyStartOnlineGame()
    {
        forceOnlineGameEvenThoughNoRace = true;
        StartOnlineMatch();
    }

    public void SetMapTo(string scene)
    {
        gameSceneName = scene;
    }
}
