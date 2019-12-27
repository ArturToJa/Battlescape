using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class ShowAbilitiesUI : MonoBehaviour
{
    public bool RealOne;
    public bool IsManagementScene;
    public BattlescapeLogic.Unit Unit { get; private set; }
    
    /*[SerializeField] Image theVSSprite;
    [SerializeField] GameObject VSObject;*/
    string Name;
    string Description;
    UnitTypes unitTypes;

    private void Start()
    {
        unitTypes = GetComponent<UnitTypes>();
    }    

    void Update()
    {
        if (IsManagementScene)
        {
            Unit = unitTypes.myUnit;
            return;
        }
        //if (RealOne == false && EnemyTooltipHandler.isOn)
        //{
        //    return;
        //}
        if (RealOne)
        {
            if (MouseManager.instance.selectedUnit == null)
            {
                return;
            }
            Unit = MouseManager.instance.selectedUnit;
        }
        else
        {
            //if (MouseManager.Instance.MouseoveredUnit == null)
            //{
            //    return;
            //}
            //Unit = MouseManager.Instance.MouseoveredUnit.GetComponent<BattlescapeLogic.Unit>();
        }
    }

    
    public GameObject AddAbilityIcon(GameObject prefab/*, Sprite sprite, string Name, string Description*/, bool real)
    {
        if (real != RealOne)
        {
            return null;
        }
        GameObject NewIcon = Instantiate(prefab, this.transform);
        /* NewIcon.GetComponent<Image>().sprite = sprite;
         NewIcon.GetComponent<MouseHoverInfoCursor>().TooltipName = Name;
         NewIcon.GetComponent<MouseHoverInfoCursor>().TooltipText = Description;*/
        return NewIcon;
    }
}
