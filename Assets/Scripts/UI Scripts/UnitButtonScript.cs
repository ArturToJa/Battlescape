using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

public class UnitButtonScript : MonoBehaviour
{
    AMScreen_UnitsChoice screen;
    public UnitCreator unitCreator { get; set; }
    Unit myUnit;
    Text theName;
    Text value;
    Text limit;
    int _amount;
    public int amount
    {
        get
        {
            return _amount;
        }
        set
        {
            _amount = value;
            if (this.transform.parent.parent.name == "PossibleUnits")
            {
                limit.text = myUnit.statistics.limit.ToString();
            }
            else
            {
                limit.text = _amount.ToString();
            }
        }
    }



    public void OnCreation(UnitCreator _unitCreator)
    {
        screen = FindObjectOfType<AMScreen_UnitsChoice>();
        unitCreator = _unitCreator;
        myUnit = unitCreator.prefab.GetComponent<Unit>();
        limit = GetComponentsInChildren<Text>()[2];
        limit.fontSize = 20;
        theName = GetComponentsInChildren<Text>()[0];
        value = GetComponentsInChildren<Text>()[1];
        theName.text = myUnit.info.unitName;
        this.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed());
        limit.color = Color.white;
        theName.color = Color.white;
        value.color = Color.white;
        value.text = myUnit.statistics.cost.ToString();
        amount = 0;
    }

    public void OnButtonPressed()
    {
        screen.OnUnitButtonPressed(this.GetComponent<Button>());
    }
}
