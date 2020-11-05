using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UnitStatShower : MonoBehaviour
{
    public static UnitStatShower currentInstance { get; set; }
    [SerializeField] Text attackSpot;
    [SerializeField] Text defenceSpot;
    [SerializeField] Text healthSpot;
    [SerializeField] Text moveSpeedSpot;
    [SerializeField] Text fluffBio;
    [SerializeField] Text heroClass;
    [SerializeField] Text heroName;

    void UpdateInfo(Unit currUnit)
    {
        attackSpot.text = currUnit.statistics.GetCurrentAttack().ToString();
        defenceSpot.text = currUnit.statistics.GetCurrentDefence().ToString();
        healthSpot.text = currUnit.statistics.maxHealthPoints.ToString();
        moveSpeedSpot.text = currUnit.statistics.GetCurrentMaxMovementPoints().ToString();
        fluffBio.text = currUnit.info.fluffText;
        
        if (currUnit is Hero)
        {
            heroClass.text = currUnit.info.unitName;
            heroName.text = Global.instance.armySavingManager.currentSave.heroName;
        }        
    }

    public static void UpdateUnitInfo(BattlescapeLogic.Unit currUnit)
    {
        currentInstance.UpdateInfo(currUnit);
    }
}
