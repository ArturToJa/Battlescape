using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextController : MonoBehaviour
{

    PopupDmgScript _popupText;
    static PopupDmgScript popupText;
    static GameObject canvas;
    static Queue<PopupInformation> popupsToShow;
    [SerializeField] float timeBetweenPopups;
    float timeFromLastPopup;

    void Start()
    {
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
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

    public void OnNewTurn()
    {
        ClearPopups();
    }
    static void Initialize(PopupTypes type)
    {
        popupText = Resources.Load<PopupDmgScript>("PopupTextParent" + (type.GetHashCode() + 1).ToString());
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    public static void AddPopupText(string text, PopupTypes type)
    {        
        popupsToShow.Enqueue(new PopupInformation(type,text));
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
