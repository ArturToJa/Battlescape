using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class NewTurnButton : MonoBehaviour
{
    Button thisButton;
    
    void Start()
    {
        thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener(GameTurn.instance.OnClick);
    }    
   

}
