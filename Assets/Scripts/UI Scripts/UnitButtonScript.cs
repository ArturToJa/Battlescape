using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UnitButtonScript : MonoBehaviour
{

    public UnitCreator unitCreator { get; set; }
    Unit myUnit;
    Text theName;
    Text value;
    Text limit;
    int amount = 1;



    public void OnCreation(UnitCreator _unitCreator)
    {
        unitCreator = _unitCreator;
        myUnit = unitCreator.prefab.GetComponent<Unit>();
        limit = GetComponentsInChildren<Text>()[2];
        limit.fontSize = 20;
        theName = GetComponentsInChildren<Text>()[0];
        value = GetComponentsInChildren<Text>()[1];
        theName.text = myUnit.info.unitName;
        this.GetComponent<Button>().onClick.AddListener(() => SetPressedButton());
        limit.color = Color.white;
        theName.color = Color.white;
        value.color = Color.white;
        value.text = myUnit.statistics.cost.ToString();
        if (this.transform.parent.parent.name == "PossibleUnits")
        {
            limit.text = myUnit.statistics.limit.ToString();
        }
        else
        {
            limit.text = amount.ToString();
        }
    }

    public void SetPressedButton()
    {
        ArmyBuilder.instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.instance.AddOrRemoveUnit(true);
    }
    public void AddButtonToRightList()
    {
        ArmyBuilder.instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.instance.AddOrRemoveUnit(false);
    }

    public void IncrementAmount()
    { amount++; }

    public void DecrementAmount()
    { amount--; }

    public int GetAmount()
    {
        return amount;
    }


}
