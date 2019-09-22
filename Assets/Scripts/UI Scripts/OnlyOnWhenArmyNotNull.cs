using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOnWhenArmyNotNull : MonoBehaviour
{
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SaveLoadManager.Instance.SaveArmyButton);
    }
    private void Update()
    {
        UIManager.SmoothlyTransitionActivity(this.gameObject, ArmyBuilder.Instance.RightUnits.childCount > 0, 0.1f);
    }

}
