using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class VERY_POORLY_WRITTEN_CLASS : MonoBehaviour
{
    // I have no idea what this monstrosity even is but im not getting inside of this script without a sword, armour and at least ten brave warriors. This is a dragon cave, run man! Run before this code swallows you!

    [SerializeField] Transform DeploymentPanel;
    // I mean.. I warned you!

    public void Okay()
    {
        int ID = -1;
        switch (Global.instance.MatchType)
        {
            case MatchTypes.Online:
                if (PhotonNetwork.isMasterClient)
                {
                    ID = 0;
                }
                else
                {
                    ID = 1;
                }
                break;
            case MatchTypes.HotSeat:

                for (int i = 0; i < Global.instance.playerBuilders.Length; i++)
                {
                    if (Global.instance.playerBuilders[i] != null)
                    {
                        ID = Global.instance.playerBuilders[i].team.index;
                        break;
                    }
                }
                break;
            case MatchTypes.Singleplayer:
                for (int i = 0; i < Global.instance.playerBuilders.Length; i++)
                {
                    if (Global.instance.playerBuilders[i] != null)
                    {
                        ID = Global.instance.playerBuilders[i].index;
                    }
                }
                break;
            case MatchTypes.None:
                break;
            default:
                break;
        }
        PlayerBuilder currentPlayerBuilder = Global.instance.playerBuilders[ID];
        Player currentPlayer = new Player(currentPlayerBuilder);
        GameRound.instance.currentPlayer = currentPlayer;
        Global.instance.playerTeams[currentPlayerBuilder.team.index].AddNewPlayer(currentPlayer);        
        Global.instance.playerBuilders[ID] = null;
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

    
}
