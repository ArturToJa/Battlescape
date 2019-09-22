using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            if (VictoryLossChecker.HasGameEnded() && VictoryLossChecker.IsGameDrawn() == false)
            {
                if (Player.Players[0].HasWon)
                {
                    flag.sprite = Flags[(int)Player.Players[0].Colour];
                    emblem.sprite = Emblems[(int)Player.Players[0].Race];
                }
                else
                {
                    flag.sprite = Flags[(int)Player.Players[1].Colour];
                    emblem.sprite = Emblems[(int)Player.Players[1].Race];
                }
            }
            UIManager.SmoothlyTransitionActivity(gameObject, VictoryLossChecker.HasGameEnded() && VictoryLossChecker.IsGameDrawn() == false, 0.01f);
        }
        else
        {
            if (Player.Players.ContainsKey(TurnManager.Instance.PlayerToMove))
            {                
                flag.sprite = Flags[(int)Player.Players[TurnManager.Instance.PlayerToMove].Colour];
                emblem.sprite = Emblems[(int)Player.Players[TurnManager.Instance.PlayerToMove].Race];
            }
            
        }
    }
}
