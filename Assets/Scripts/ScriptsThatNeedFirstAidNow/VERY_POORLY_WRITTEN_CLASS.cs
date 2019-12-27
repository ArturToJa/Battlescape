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
        Global.instance.playerTeams[currentPlayerBuilder.team.index].AddNewPlayer(new Player(currentPlayerBuilder));
        Global.instance.playerBuilders[ID] = null;
        Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].race = (Faction)SaveLoadManager.Instance.Race;
        TurnManager.Instance.TurnCount = 0;
        if (Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index == 1)
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

    /*public void CreateUnit(GameObject Unit, RenderTexture _sprite, Unit _me)
    {
        var temp = Instantiate(Unit, DeploymentPanel);
        temp.GetComponentInChildren<RawImage>().texture = _sprite;
        temp.GetComponent<DragableUnitIcon>().me = _me;
    }
    public void CreateHero(GameObject Unit, Sprite _sprite, Unit _me)
    {
        var temp = Instantiate(Unit, DeploymentPanel);
        temp.GetComponentInChildren<RawImage>().gameObject.SetActive(false);
        temp.GetComponentsInChildren<Image>(true)[1].gameObject.SetActive(true);
        temp.GetComponentsInChildren<Image>()[1].sprite = _sprite;
        temp.GetComponent<DragableUnitIcon>().me = _me;
    }

    void CreateAllUnits()
    {
        int safeCheck = 0;
        foreach (Unit theUnit in SaveLoadManager.Instance.UnitsList)
        {
            if (safeCheck > 30)
            {
                break;
            }
            CreateUnit(theUnit.thisBox, theUnit.thisSprite, theUnit);
            safeCheck++;
        }
        CreateHero(SaveLoadManager.Instance.hero.thisBox, SaveLoadManager.Instance.hero.ThisRealSprite, SaveLoadManager.Instance.hero);

    }*/
}
