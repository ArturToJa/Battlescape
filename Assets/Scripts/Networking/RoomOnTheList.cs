using BattlescapeLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomOnTheList : MonoBehaviour
{
    [SerializeField]
    Text _roomNameText;

    public Text RoomNameText
    {
        get
        {
            return _roomNameText;
        }
    }
    public string RoomName { get; private set; }

    public bool IsUpdated { get; set; }

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => NetworkingBaseClass.Instance.JoinRoom(RoomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string t)
    {
        RoomName = t;
        RoomNameText.text = RoomName;
    }

    public void OnClick()
    {

    }


}
