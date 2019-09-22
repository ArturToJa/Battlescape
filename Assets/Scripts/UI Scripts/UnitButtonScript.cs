using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonScript : MonoBehaviour
{

    public Unit thisUnit;
    bool isSpecial;
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
            theName.text = thisUnit.Name;
            isSpecial = thisUnit.thisUnitFirstPlayer.GetComponent<UnitScript>().isSpecial;
            this.GetComponent<Button>().onClick.AddListener(() => SetPressedButton());
            if (isSpecial)
            {
                limit.color = Color.yellow;
                theName.color = Color.yellow;
                value.color = Color.yellow;
            }
            else
            {
                limit.color = Color.white;
                theName.color = Color.white;
                value.color = Color.white;
            }
        }

    }

    private void Update()
    {
        if (isVisual)
        {
            value.text = thisUnit.Cost.ToString();


            if (this.transform.parent.parent.name == "PossibleUnits")
            {
                limit.text = thisUnit.LimitFor50.ToString();
            }
            else
            {
                limit.text = amount.ToString();
            }
        }
    }

    public void SetPressedButton()
    {
        ArmyBuilder.Instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.Instance.AddOrRemoveUnit(true);
    }
    public void AddButtonToRightList()
    {
        ArmyBuilder.Instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.Instance.AddOrRemoveUnit(false);
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
