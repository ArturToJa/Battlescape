using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetaliationButtonsScript : MonoBehaviour
{      
    private void Update()
    {
        UIManager.SmoothlyTransitionActivity(this.gameObject, CheckIfImNeeded(), 0.1f);
        SetBlockingRaycasts();
    }

    private void SetBlockingRaycasts()
    {
        if (GetComponent<CanvasGroup>().alpha > 0.9f)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    private bool CheckIfImNeeded()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Log.SpawnLog("GameState: " + GameStateManager.Instance.GameState.ToString());
            Log.SpawnLog("isLocal: " + GameStateManager.Instance.IsCurrentPlayerLocal());
        }
        return GameStateManager.Instance.GameState == GameStates.RetaliationState && GameStateManager.Instance.IsCurrentPlayerLocal();
    }

    public void Yes()
    {
        CombatController.Instance.SendCommandToRetaliate();
    }

    public void No()
    {
        CombatController.Instance.SendCommandToNotRetaliate();
    }
}
