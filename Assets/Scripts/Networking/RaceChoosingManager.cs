using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using Photon.Pun;

public class RaceChoosingManager : MonoBehaviour
{
    public static RaceChoosingManager instance;

    [SerializeField] Image myArmy;
    [SerializeField] Image enemyArmy;
    [SerializeField] GameObject roomObj;
    [SerializeField] GameObject raceChooserObj;
    [SerializeField] GameObject chooseRaceButton;
    public Text raceDescriptionText;
    public Text raceNameText;
    Race chosenRace;

    PhotonView photonView;
    bool didRevealEnemyRace = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        chooseRaceButton.SetActive(Global.instance.HaveAllPlayersChosenRace() == false && PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2);
        if (Global.instance.HaveAllPlayersChosenRace() && didRevealEnemyRace == false)
        {
            photonView.RPC("RPCSetEnemyRace", RpcTarget.Others, chosenRace, PlayerPrefs.GetString("MyPlayerName"));
            didRevealEnemyRace = true;
        }
    }

    public void StartRacesetting()
    {
        raceChooserObj.transform.SetAsLastSibling();
        SetMyRaceTo(Race.Neutral);
    }
    public void ChooseRace(int RaceID)
    {

        // THIS sets my Race to chosen race ON MY PC ONLY. 
        Race race = (Race)RaceID;
        chosenRace = race;
        foreach (PlayerTeam team in Global.instance.playerTeams)
        {
            foreach (Player player in team.players)
            {
                if (player.type == PlayerType.Local)
                {
                    player.playerName = PlayerPrefs.GetString("MyPlayerName");
                    player.race = race;
                }
            }
        }      
        myArmy.sprite = Global.instance.armySavingManager.GetRaceSprite(chosenRace);
    }

    public void AcceptRace()
    {
        SetMyRaceTo(chosenRace);
        roomObj.transform.SetAsLastSibling();
    }

    public void SetMyRaceTo(Race race)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RpcSetRaceAndName", RpcTarget.All, race, PlayerPrefs.GetString("MyPlayerName"), 0, 0);
        }
        else
        {
            photonView.RPC("RpcSetRaceAndName", RpcTarget.All, race, PlayerPrefs.GetString("MyPlayerName"), 1, 0);
        }
    }

    [PunRPC]
    public void RpcSetRaceAndName(Race race, string name, int teamID, int playerID)
    {
        Global.instance.playerTeams[teamID].players[playerID].playerName = PlayerPrefs.GetString("MyPlayerName");
        Global.instance.playerTeams[teamID].players[playerID].race = race;
        Global.instance.playerTeams[teamID].players[playerID].team = Global.instance.playerTeams[teamID];
    }

    [PunRPC]
    public void RPCSetEnemyRace(Race Race, string playerName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RpcSetRaceAndName", RpcTarget.All, Race, playerName, 1, 0);
        }
        else
        {
            photonView.RPC("RpcSetRaceAndName", RpcTarget.All, Race, playerName, 0, 0);
        }
        enemyArmy.sprite = Global.instance.armySavingManager.GetRaceSprite(Race);
        LogConsole.instance.LobbySpawnLog("--SYSTEM-- Player: " + playerName + " chose race: " + Race.ToString());
    }



}
