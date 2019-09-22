using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    //float timeTillTooltip;
    bool singlePlayer = false;
    bool isGreenAI = false;
    [SerializeField] Toggle the25toggle;
    [SerializeField] Toggle the50toggle;
    [SerializeField] Toggle MAthe25toggle;
    [SerializeField] Toggle MAthe50toggle;
    [SerializeField] Toggle MultiPlayerthe25toggle;
    [SerializeField] Toggle MultiPlayerthe50toggle;
    [SerializeField] List<AIArmy> AIArmies;
    [SerializeField] GameObject NoArmyPopupWindow;
    [SerializeField] GameObject LackingRaceSavesPopupWindow;
    [SerializeField] Text LackingRacesText;
    string GameSceneName;
    [SerializeField] List<string> GameScenes;

    void Start()
    {
        PlayerPrefs.SetFloat("TimeTillOpenTooltip", /*timeTillTooltip*/ 0.1f);
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
    /*public void ChangeTimeTillTooltipsShowUp(float f)
    {
        timeTillTooltip = f;
    }*/
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
        PlayerPrefs.SetFloat("TimeTillOpenTooltip", /*timeTillTooltip*/ 0.1f);
        PlayerPrefs.Save();
    }
    public void DefaultOptions()
    {
        canChangeYRotation = false;
        panSpeed = 10;
        rotationSpeed = 4;
        moveSpeedAnim = 2;
        //timeTillTooltip = 0.5f;
        OKOptions();
    }
    public void ManageArmy()
    {
        if (MAthe25toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 25;
        }
        if (MAthe50toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 50;
        }
        FindObjectOfType<LevelLoader>().CommandLoadScene("_ManagementScene");
    }

    public void SwitchAISide()
    {
        isGreenAI = !isGreenAI;
    }
    public void SetSinglePlayer(bool single)
    {
        singlePlayer = single;
    }
    public void SetStartingPoints(int points)
    {
        GameStateManager.Instance.startingArmyPoints = points;
    }
    public void StartQuickMatch()
    {
        GameStateManager.Instance.MatchType = MatchTypes.Singleplayer;
        if (SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/") == null || SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/").Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }
        if (isGreenAI && singlePlayer)
        {
            Player.Players[0] = new Player(PlayerType.AI, PlayerColour.Green);
            Player.Players[1] = new Player(PlayerType.Local, PlayerColour.Red);
        }
        if (isGreenAI == false && singlePlayer)
        {
            Player.Players[0] = new Player(PlayerType.Local, PlayerColour.Green);
            Player.Players[1] = new Player(PlayerType.AI, PlayerColour.Red);
        }
        if (singlePlayer == false)
        {
            Debug.LogError("are you sure?");
        }
        if (the25toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 25;
            SetStartingPoints(25);
        }
        else if (the50toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 50;
            SetStartingPoints(50);
        }
        PlayGameScene();
    }

    private void PlayGameScene()
    {
        if (GameScenes.Contains(GameSceneName) == false)
        {
            GameSceneName = GameScenes[Random.Range(0, GameScenes.Count)];
        }
        FindObjectOfType<LevelLoader>().CommandLoadScene(GameSceneName);
    }

    public void StartHotSeat()
    {
        GameStateManager.Instance.MatchType = MatchTypes.HotSeat;
        if (SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/") == null || SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/").Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }
        Player.Players[0] = new Player(PlayerType.Local, PlayerColour.Green);
        Player.Players[1] = new Player(PlayerType.Local, PlayerColour.Red);
        if (MultiPlayerthe25toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 25;
            SetStartingPoints(25);
        }
        else if (MultiPlayerthe50toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 50;
            SetStartingPoints(50);
        }
        FindObjectOfType<LevelLoader>().CommandLoadScene(GameSceneName);
    }

    public void StartOnlineMatch()
    {
        if (SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/") == null || SaveLoadManager.Instance.GetSaveNames(Application.persistentDataPath + "/Armies/").Count == 0)
        {
            NoArmyPopupWindow.SetActive(true);
            return;
        }
        if (forceOnlineGameEvenThoughNoRace == false)
        {
            List<Faction> racesLackingSaves = new List<Faction>();
            bool showPopup = false;
            foreach (Faction race in System.Enum.GetValues(typeof(Faction)))
            {
                if (race == Faction.Neutral)
                {
                    continue;
                }
                if (SaveLoadManager.Instance.HasRaceSaved(race) == false)
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
        GameStateManager.Instance.MatchType = MatchTypes.Online;
        Player.Players[0] = new Player(PlayerType.Network, PlayerColour.Green);
        Player.Players[1] = new Player(PlayerType.Network, PlayerColour.Red);



        if (MultiPlayerthe25toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 25;
            SetStartingPoints(25);
        }
        else if (MultiPlayerthe50toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 50;
            SetStartingPoints(50);
        }
        FindObjectOfType<LevelLoader>().CommandLoadScene("_LobbyScene");
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

    void ShowNoParticularRacePopupWindow(List<Faction> races)
    {
        LackingRaceSavesPopupWindow.SetActive(true);
        LackingRacesText.text = "You do not have saves for these Factions: ";
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
    public void SetMapToCurrentNumber()
    {
        // NOTE that names of the buttons currently need to correspond to the names of the scenes ;D bad code i know right
        GameSceneName = EventSystem.current.currentSelectedGameObject.name;
    }
}
