using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSelector
{

    // This is a NEW script made for refactoring Mouse Manager! It still does 'nothing" cause old Mouse Manager still is not reworked compeletely!

    GameObject UnitSelectionIndicator;

    
  
    public void SelectUnit(BattlescapeLogic.Unit unit)
    {
        Starting();
        UnitSelectionIndicator.transform.position = unit.transform.position;
        UnitSelectionIndicator.transform.SetParent(unit.transform);
    }

    public void DeselectUnit()
    {
        Starting();
        UnitSelectionIndicator.transform.position = new Vector3(100, 100, 100);
        UnitSelectionIndicator.transform.parent = null;
    }




    void Starting()
    {
        UnitSelectionIndicator = GameObject.FindGameObjectWithTag("SelInd");
    }

}
