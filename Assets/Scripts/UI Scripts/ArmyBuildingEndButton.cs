using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class ArmyBuildingEndButton : MonoBehaviour
{

    [SerializeField] GameObject loadWindow;

    public void OK()
    {
        CameraController.Instance.SetCurrentViewTo(0);
        //CombatController.Instance.MakeAIWait(3f);
        this.transform.parent.parent.gameObject.SetActive(false);
        if (GameRound.instance.currentPlayer.team.index == 0 && Global.instance.matchType != MatchTypes.Online)
        {
            SaveLoadManager.instance.unitsList.Clear();
            if(Global.instance.GetCurrentPlayerBuilder().type == PlayerType.AI)
            {
                SaveLoadManager.instance.LoadAIArmyToGame(Global.instance.GetCurrentPlayerBuilder(), SaveLoadManager.instance.currentSaveValue);
                Global.instance.GetCurrentPlayerBuilder().race = SaveLoadManager.instance.race;
            }
            else
            {
                loadWindow.SetActive(true);
                loadWindow.GetComponent<CanvasGroup>().alpha = 1f;
                loadWindow.GetComponent<CanvasGroup>().interactable = true;
                loadWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
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
            NetworkingApiBaseClass.Instance.PlayerEndedPreGame();

        }
    }
}
