using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class ArmyBuildingEndButton : MonoBehaviour
{
    [SerializeField] GameObject UnitPanel;
    //HeroChoiceScreenScript heroChoicer;    

    //[SerializeField] Renderer Pedestal;
    [SerializeField] GameObject LoadWindow;

    private void Update()
    {
        UIManager.SmoothlyTransitionActivity(this.gameObject, UnitPanel.transform.childCount == 0, 0.01f);
    }

    public void OK()
    {
        CameraController.Instance.SetCurrentViewTo(0);
        //CombatController.Instance.MakeAIWait(3f);
        this.transform.parent.parent.gameObject.SetActive(false);
        if (GameRound.instance.currentPlayer.team.index == 0 && Global.instance.matchType != MatchTypes.Online)
        {
            SkyboxChanger.Instance.SetSkyboxTo(SkyboxChanger.Instance.PregameSkyboxDefault);
            SaveLoadManager.Instance.UnitsList.Clear();
            foreach (Transform child in UnitPanel.transform)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
            if (Global.instance.playerBuilders[1,0].type == PlayerType.AI)
            {
                SaveLoadManager.Instance.LoadAIArmyToGame(Global.instance.playerBuilders[1,0], SaveLoadManager.Instance.currentSaveValue);
                Global.instance.playerBuilders[1,0].race = (Faction)SaveLoadManager.Instance.Race;
            }
            else
            {
                LoadWindow.SetActive(true);
                LoadWindow.GetComponent<CanvasGroup>().alpha = 1f;
                LoadWindow.GetComponent<CanvasGroup>().interactable = true;
                LoadWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
               
            }
            else
            {
                GameRound.instance.EndOfPhase();
            }
            foreach (Tile tile in FindObjectsOfType<Tile>())
            {
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
            CameraController.Instance.StartCoroutine(CameraController.Instance.CheckIfPositionAndRotationMatchDesired());
            // UnitPanel.transform.parent.gameObject.SetActive(false);
        }
        if (Global.instance.matchType == MatchTypes.Online)
        {
            Networking.instance.PlayerEndedPreGame();

        }
    }
}
