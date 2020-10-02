using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PopupTextController : TurnChangeMonoBehaviour
{

    PopupDmgScript _popupText;
    static PopupDmgScript popupText;
    static GameObject canvas;
    static Queue<PopupInformation> popupsToShow;
    [SerializeField] float timeBetweenPopups;
    float timeFromLastPopup;

    protected override void Start()
    {
        base.Start();
        popupsToShow = new Queue<PopupInformation>();
    }

    void Update()
    {

        if (popupsToShow.Count > 0 && timeFromLastPopup >= timeBetweenPopups)
        {
            CreatePopupText(popupsToShow.Dequeue());
            timeFromLastPopup = 0;
        }
        else
        {
            timeFromLastPopup += Time.deltaTime;
        }
    }   
    static void Initialize(PopupTypes type)
    {
        popupText = Resources.Load<PopupDmgScript>("PopupTextParent" + (type.GetHashCode() + 1).ToString());
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    public static void AddPopupText(string text, PopupTypes type)
    {
        popupsToShow.Enqueue(new PopupInformation(type, text));
    }

    public static void AddParalelPopupText(string text, PopupTypes type)
    {
        PopupInformation popup = new PopupInformation(type, text);
        CreatePopupText(popup);
    }

    static PopupDmgScript CreatePopupText(PopupInformation popupInfo)
    {
        Initialize(popupInfo.type);
        PopupDmgScript popup = Instantiate(popupText);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f, 0.5f), location.position.y + 3 + Random.Range(-0.5f, 0.5f)));
        Vector2 screenPosition = new Vector2(Screen.width / 2 + Random.Range(-100f, 100f), Screen.height / 2 + Random.Range(-100f, 100f));
        popup.transform.SetParent(canvas.transform, false);
        popup.transform.position = screenPosition;
        popup.SetText(popupInfo.text);
        popup.Execute();
        return popup;
    }

    public static void ClearPopups()
    {
        popupsToShow.Clear();
    }

    public override void OnNewRound()
    {
        ClearPopups();
        if (GameRound.instance.gameRoundCount == 1)
        {
            AddPopupText("Press Escape to see Victory Conditions!", PopupTypes.Info);
            LogConsole.instance.SpawnLog("Prepare for the Battle! Press Escape to see Victory Conditions!");
        }
        else if (GameRound.instance.gameRoundCount < GameRound.instance.maximumRounds - GameRound.instance.countdown)
        {
            AddPopupText("New Round!", PopupTypes.Info);
            LogConsole.instance.SpawnLog("New round");
        }
        else if (GameRound.instance.gameRoundCount < GameRound.instance.maximumRounds)
        {
            AddPopupText("Remaining rounds: " + (GameRound.instance.maximumRounds - GameRound.instance.gameRoundCount).ToString() + "!", PopupTypes.Damage);
            LogConsole.instance.SpawnLog("New round. Remaining rounds: " + (GameRound.instance.maximumRounds - GameRound.instance.gameRoundCount).ToString() + ".");
        }
        else if (GameRound.instance.gameRoundCount == GameRound.instance.maximumRounds)
        {
            AddPopupText("Final Turn!", PopupTypes.Damage);
            LogConsole.instance.SpawnLog("The last turn of the game has begun!");
        }
        else
        {
            AddPopupText("Time is up!", PopupTypes.Stats);
        }
    }

    public override void OnNewTurn()
    {
        AddPopupText("New Turn!", PopupTypes.Info);
        LogConsole.instance.SpawnLog("New turn of player: " + GameRound.instance.currentPlayer.playerName + ".");
    }

    public override void OnNewPhase()
    {
        AddPopupText("Next Phase!", PopupTypes.Info);
        LogConsole.instance.SpawnLog(GameRound.instance.currentPhase.ToString() + " begins.");
    }
}
public enum PopupTypes
{
    Damage, Info, Stats
}

[System.Serializable]
public struct PopupInformation
{
    public PopupTypes type;
    public string text;

    public PopupInformation(PopupTypes _type, string _text)
    {
        type = _type;
        text = _text;
    }
}
