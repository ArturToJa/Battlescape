using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class FlagScript : MonoBehaviour
{
    Image flag;
    Image emblem;
    [SerializeField] List<Sprite> Flags;
    [SerializeField] bool isEndgameFlag;
    [SerializeField] List<Sprite> Emblems;

    void Start()
    {
        flag = GetComponent<Image>();
        emblem = GetComponentsInChildren<Image>()[1];
    }
    void Update()
    {
        if (isEndgameFlag)
        {
            
                if (VictoryLossChecker.gameResult == GameResult.GreenWon)
                {
                    flag.sprite = Flags[(int)Global.instance.playerTeams[0].players[0].colour];
                    emblem.sprite = Emblems[(int)Global.instance.playerTeams[0].players[0].race];
                }
                if(VictoryLossChecker.gameResult == GameResult.RedWon)
                {
                    flag.sprite = Flags[(int)Global.instance.playerTeams[1].players[0].colour];
                    emblem.sprite = Emblems[(int)Global.instance.playerTeams[1].players[0].race];
                }
            
            UIManager.SmoothlyTransitionActivity(gameObject, VictoryLossChecker.IsGameOver && VictoryLossChecker.gameResult != GameResult.Draw, 0.01f);
        }
        else
        {
            if (Global.instance.playerTeams[TurnManager.Instance.PlayerToMove] != null && Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players.Count > 0 && Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0] != null)
            {                
                flag.sprite = Flags[(int)Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].colour];
                emblem.sprite = Emblems[(int)Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].race];
            }
            
        }
    }
}
