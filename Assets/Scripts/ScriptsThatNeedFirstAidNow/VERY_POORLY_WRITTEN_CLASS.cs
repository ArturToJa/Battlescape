using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using System;

public class VERY_POORLY_WRITTEN_CLASS : MonoBehaviour
{
    // I have no idea what this monstrosity even is but im not getting inside of this script without a sword, armour and at least ten brave warriors. This is a dragon cave, run man! Run before this code swallows you!

    [SerializeField] Transform DeploymentPanel;
    // I mean.. I warned you!

    public void Okay()
    {
        PlayerBuilder currentPlayerBuilder = GetPlayerBuilder();        
        Player currentPlayer = new Player(currentPlayerBuilder);
        GameRound.instance.currentPlayer = currentPlayer;
        Networking.instance.SendCommandToAddPlayer(Global.instance.playerTeams[currentPlayerBuilder.team.index], currentPlayer);
        currentPlayer.race = SaveLoadManager.Instance.Race;
        if (currentPlayer.team.index == 1)
        {
            if (SkyboxChanger.Instance.isSkyboxRandom)
            {
                SkyboxChanger.Instance.SetSkyboxTo(SkyboxChanger.Instance.realSkyboxNumber);
            }
            CameraController.Instance.SetCurrentViewTo(2);
        }
        else
        {
            if (SkyboxChanger.Instance.isSkyboxRandom)
            {
                SkyboxChanger.Instance.SetSkyboxToRandom();
            }

            CameraController.Instance.SetCurrentViewTo(1);

        }
        CameraController.Instance.manualCamera = false;
        CameraController.Instance.correctCamera = true;
        DeploymentPanel.parent.gameObject.SetActive(true);
        //CreateAllUnits();
        PreGameAI temp = new PreGameAI();
        temp.PositionUnits();


    }

    //VERY temporary :D
    PlayerBuilder GetPlayerBuilder()
    {
        if (Global.instance.matchType == MatchTypes.Online)
        {
            if (PhotonNetwork.isMasterClient)
            {
                return Global.instance.playerBuilders[0, 0];
            }
            else
            {
                return Global.instance.playerBuilders[1, 0];
            }
        }
        else
        {
            for (int i = 0; i < Global.instance.playerBuilders.GetLength(0); i++)
                for (int j = 0; j < Global.instance.playerBuilders.GetLength(1); j++)
                {
                    if (Global.instance.playerBuilders[i, j] != null)
                    {
                        return Global.instance.playerBuilders[i, j];
                    }
                }
            Debug.LogError("Not found!");
            return null;
        }        
    }
}
