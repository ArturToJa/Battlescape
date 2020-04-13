using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using System;

public class VERY_POORLY_WRITTEN_CLASS : MonoBehaviour
{
    // I have no idea what this monstrosity even is but im not getting inside of this script without a sword, armour and at least ten brave warriors. This is a dragon cave, run man! Run before this code swallows you!

    [SerializeField] Transform deploymentPanel;
    // I mean.. I warned you!

    public void LoadPlayerToGame()
    {
        PlayerBuilder currentPlayerBuilder = Global.instance.GetCurrentPlayerBuilder();
        Player currentPlayer = new Player(currentPlayerBuilder);
        GameRound.instance.currentPlayer = currentPlayer;
        currentPlayer.race = SaveLoadManager.instance.race;

        if (SkyboxChanger.instance.isSkyboxRandom)
        {
            SkyboxChanger.instance.SetSkyboxTo(SkyboxChanger.instance.realSkyboxNumber);
        }

        CameraController.Instance.SetCurrentViewTo(currentPlayer.team.index + 1);
        CameraController.Instance.manualCamera = false;
        CameraController.Instance.correctCamera = true;
        deploymentPanel.parent.gameObject.SetActive(true);
        //CreateAllUnits();
        PreGameAI temp = new PreGameAI();
        NetworkingApiBaseClass.Instance.SendCommandToAddPlayer(currentPlayer.team, currentPlayer);
        temp.PositionUnits();
    }
}
