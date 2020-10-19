using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

public class SaveLoadButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        FindObjectOfType<AMScreen_ArmyChoice>().chosenSaveName = this.gameObject.name;
    }
}
