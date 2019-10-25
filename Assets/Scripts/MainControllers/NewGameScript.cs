﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class NewGameScript : MonoBehaviour
{

    public static Dictionary<Vector3, Unit> PlayerOneArmy = new Dictionary<Vector3, Unit>();
    public static Dictionary<Vector3, Unit> PlayerTwoArmy = new Dictionary<Vector3, Unit>();
    SkyboxChanger skybox;
    [SerializeField] GameObject WinScreen;

    void Start()
    {
        skybox = FindObjectOfType<SkyboxChanger>();
        PlayerOneArmy = new Dictionary<Vector3, Unit>();
        PlayerTwoArmy = new Dictionary<Vector3, Unit>();
}

    public void ReplayTheSameGame()
    {
        // THIS AINT WORKING! Have to do it MUCH better!
        Debug.LogError("This aint no gonna work lol");
        FindObjectOfType<TurnNumberText>().ResetColour();
        AudioManager.isPlayingGameOverMusic = false;
        UIManager.SmoothlyTransitionActivity(WinScreen, false, 0.001f);
        TurnManager.Instance.TurnCount = 1;
        TurnManager.Instance.PlayerHavingTurn = 0;
        TurnManager.Instance.isEndgameTrue = false;
        TurnManager.Instance.MovementPhase();
        VictoryLossChecker.Clear();
        skybox.SetSkyboxToRandom();
        foreach (UnitScript unit in FindObjectsOfType<UnitScript>())
        {
            if (Application.isEditor)
            {
                DestroyImmediate(unit.gameObject);
            }
            else
            { Destroy(unit.gameObject); }
        }

        foreach (Tile tile in Map.Board)
        {
  
            
            tile.SetMyUnitTo(null);
        }

        foreach (KeyValuePair<Vector3, Unit> pair in PlayerOneArmy)
        {
            Instantiate(pair.Value.thisUnitFirstPlayer, pair.Key, pair.Value.thisUnitFirstPlayer.transform.rotation);
        }
        foreach (KeyValuePair<Vector3, Unit> pair in PlayerTwoArmy)
        {
            Instantiate(pair.Value.thisUnitSecondPlayer, pair.Key, pair.Value.thisUnitFirstPlayer.transform.rotation);
        }
    }
}
