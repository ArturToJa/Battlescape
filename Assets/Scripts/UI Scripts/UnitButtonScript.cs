using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UnitButtonScript : MonoBehaviour
{

    public UnitCreator unitCreator;
    Unit myUnit;
    Text theName;
    Text value;
    Text limit;
    int amount = 1;
    [SerializeField] bool isVisual;

    private void Start()
    {
        if (isVisual)
        {
            limit = GetComponentsInChildren<Text>()[2];
            limit.fontSize = 20;
            theName = GetComponentsInChildren<Text>()[0];
            value = GetComponentsInChildren<Text>()[1];
            myUnit = unitCreator.prefab.GetComponent<Unit>();
            theName.text = myUnit.info.unitName;
            this.GetComponent<Button>().onClick.AddListener(() => SetPressedButton());
            limit.color = Color.white;
            theName.color = Color.white;
            value.color = Color.white;
        }

    }

    private void Update()
    {
        if (isVisual)
        {
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
