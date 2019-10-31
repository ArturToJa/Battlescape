using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class RaceChoosingManager : MonoBehaviour
{
    public static RaceChoosingManager Instance;

    [SerializeField] Image MyArmy;
    [SerializeField] Image EnemyArmy;
    [SerializeField] GameObject RoomObj;
    [SerializeField] GameObject RaceChooserObj;
    [SerializeField] GameObject ChooseRaceButton;
    public Text FactionDescriptionText;
    public Text FactionNameText;
    Faction chosenFaction;

    PhotonView photonView;
    bool didRevealEnemyRace = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        ChooseRaceButton.SetActive(SaveLoadManager.Instance.AreBothFactionsChosen == false && PhotonNetwork.connected && PhotonNetwork.room != null && PhotonNetwork.room.PlayerCount == 2);
        if (SaveLoadManager.Instance.AreBothFactionsChosen && didRevealEnemyRace == false)
        {
            photonView.RPC("RPCSetEnemyRace", PhotonTargets.Others, chosenFaction, PlayerPrefs.GetString("MyPlayerName"));
            didRevealEnemyRace = true;
        }
    }

    public void StartRacesetting()
    {
        RaceChooserObj.transform.SetAsLastSibling();
        SetMyRaceTo(Faction.Neutral);
    }
    public void ChooseRace(int factionID)
    {

        // THIS sets my faction to chosen race ON MY PC ONLY. 
        Faction Race = (Faction)factionID;
        chosenFaction = Race;
        if (PhotonNetwork.isMasterClient)
        {
            Global.instance.playerBuilders[0].playerName = PlayerPrefs.GetString("MyPlayerName");
            Global.instance.playerBuilders[0].race = Race;
        }
        else
        {
            Global.instance.playerBuilders[1].playerName = PlayerPrefs.GetString("MyPlayerName");
            Global.instance.playerBuilders[1].race = Race;
        }
        MyArmy.sprite = SaveLoadManager.Instance.GetRaceSprite(chosenFaction);
    }

    public void AcceptRace()
    {
        SetMyRaceTo(chosenFaction);
        RoomObj.transform.SetAsLastSibling();
    }

    public void SetMyRaceTo(Faction race)
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("RpcSetRaceAndName", PhotonTargets.All, race, PlayerPrefs.GetString("MyPlayerName"), 0, 0);
        }
        else
        {
            photonView.RPC("RpcSetRaceAndName", PhotonTargets.All, race, PlayerPrefs.GetString("MyPlayerName"), 1, 1);
        }
    }

    [PunRPC]
    public void RpcSetRaceAndName(Faction race, string name, int teamID, int playerID)
    {
        Global.instance.playerBuilders[playerID].playerName = PlayerPrefs.GetString("MyPlayerName");
        Global.instance.playerBuilders[playerID].race = race;
        Global.instance.playerBuilders[playerID].team = Global.instance.playerTeams[teamID];
    }

    [PunRPC]
    public void RPCSetEnemyRace(Faction Race, string playerName)
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("RpcSetRaceAndName", PhotonTargets.All, Race, playerName, 1, 1);
        }
        else
        {
            photonView.RPC("RpcSetRaceAndName", PhotonTargets.All, Race, playerName, 0, 0);
        }
        EnemyArmy.sprite = SaveLoadManager.Instance.GetRaceSprite(Race);
        Log.LobbySpawnLog("--SYSTEM-- Player: " + playerName + " chose race: " + Race.ToString(), "SystemSound");
    }



}
