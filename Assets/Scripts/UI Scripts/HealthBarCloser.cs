using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCloser : MonoBehaviour
{

    void Start()
    {
        foreach (Image child in GetComponentsInChildren<Image>())
        {
            child.raycastTarget = false;
        }
    }

    void Update()
    {
        foreach (Transform go in transform)
        {
            go.gameObject.SetActive(TurnManager.Instance.TurnCount != 0 && !VictoryLossChecker.IsGameOver);         
        }


    }
}
