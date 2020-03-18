using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatShower : MonoBehaviour
{
    [SerializeField] Text attackSpot;
    [SerializeField] Text defenceSpot;
    [SerializeField] Text healthSpot;
    [SerializeField] Text moveSpeedSpot;
    [SerializeField] Text fluffBio;

    public BattlescapeLogic.Unit currUnit;

    public void UpdateInfos()
    {
        attackSpot.text = currUnit.statistics.GetCurrentAttack().ToString();
        defenceSpot.text = currUnit.statistics.GetCurrentDefence().ToString();
        healthSpot.text = currUnit.statistics.maxHealthPoints.ToString();
        moveSpeedSpot.text = currUnit.statistics.GetCurrentMaxMovementPoints().ToString();
        fluffBio.text = currUnit.info.fluffText;
    }
}
