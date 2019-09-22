using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            SaveLoadManager.Instance.ChosenFactions[0] = Race;
        }
        else
        {
            SaveLoadManager.Instance.ChosenFactions[1] = Race;
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
            photonView.RPC("RPCSetRace", PhotonTargets.All, race, 0);
        }
        else
        {
            photonView.RPC("RPCSetRace", PhotonTargets.All, race, 1);
        }
    }

    [PunRPC]
    public void RPCSetRace(Faction race, int ID)
    {
        SaveLoadManager.Instance.ChosenFactions[ID] = race;
        Player.Players[ID].Race = race;
    }

    [PunRPC]
    public void RPCSetEnemyRace(Faction Race, string playerName)
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("RPCSetRace", PhotonTargets.All, Race, 1);
        }
        else
        {
            photonView.RPC("RPCSetRace", PhotonTargets.All, Race, 0);
        }        
        EnemyArmy.sprite = SaveLoadManager.Instance.GetRaceSprite(Race);
        Log.LobbySpawnLog("--SYSTEM-- Player: " + playerName + " chose race: " + Race.ToString(), "SystemSound");
    }



}
