using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VERY_POORLY_WRITTEN_CLASS : MonoBehaviour
{
    // I have no idea what this monstrosity even is but im not getting inside of this script without a sword, armour and at least ten brave warriors. This is a dragon cave, run man! Run before this code swallows you!

    [SerializeField] Transform DeploymentPanel;
    // I mean.. I warned you!

    public void Okay()
    {
        Player.Players[TurnManager.Instance.PlayerHavingTurn].Race = SaveLoadManager.Instance.Race;
        TurnManager.Instance.TurnCount = 0;
        if (TurnManager.Instance.PlayerHavingTurn == 1)
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
        CreateAllUnits();
    }

    public void CreateUnit(GameObject Unit, RenderTexture _sprite, Unit _me)
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

    }
}
