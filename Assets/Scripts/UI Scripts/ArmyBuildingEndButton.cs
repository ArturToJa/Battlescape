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
        CombatController.Instance.MakeAIWait(3f);
        this.transform.parent.parent.gameObject.SetActive(false);        
        if (TurnManager.Instance.PlayerToMove == 0 && GameStateManager.Instance.MatchType != MatchTypes.Online)
        {
            TurnManager.Instance.TurnCount = -1;
            //  Pedestal.enabled = true;
            //heroChoicer.NextPlayer();
            TurnManager.Instance.NewTurn(false);
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
            if (Player.Players[1].Type == PlayerType.AI)
            {
                SaveLoadManager.Instance.LoadAIArmyToGame(SaveLoadManager.Instance.currentSaveValue);
                Player.Players[1].Race = SaveLoadManager.Instance.Race;
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
            if (GameStateManager.Instance.MatchType == MatchTypes.Online)
            {
                var Text1 = FindObjectOfType<CurrentPlayerInfo>();
                Text1.GetComponent<Text>().text = Player.Players[Mathf.Abs(TurnManager.Instance.PlayerHavingTurn - 1)].Colour.ToString() + "'s turn";
                Text1.isOff = true;
                var Text3 = FindObjectOfType<TurnNumberText>();
                Text3.GetComponent<Text>().text = "Waiting for opponent...";
                Text3.isOff = true;

                foreach (Tile tile in Map.Board)
                {
                    for (int i = 0; i < tile.isDropzoneOfPlayer.Length; i++)
                    {
                        tile.isDropzoneOfPlayer[i] = false;
                    }                 
                }
            }
            else
            {
                TurnManager.Instance.NewTurn(true);
            }
            HeroNames.SetAIHeroesNameDifferent();
            foreach (Tile tile in FindObjectsOfType<Tile>())
            {
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
            CameraController.Instance.StartCoroutine(CameraController.Instance.CheckIfPositionAndRotationMatchDesired());
            // UnitPanel.transform.parent.gameObject.SetActive(false);
        }
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            TurnManager.Instance.PlayerEndedPreGame();

        }
    }
}
