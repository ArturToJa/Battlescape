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

    public UnitScript currUnit;

    public void UpdateInfos()
    {
        attackSpot.text = currUnit.GetBaseAttack().ToString();
        defenceSpot.text = currUnit.GetBaseDefence().ToString();
        healthSpot.text = currUnit.MaxHP.ToString();
        moveSpeedSpot.text = currUnit.GetBaseMS().ToString();
        fluffBio.text = currUnit.unitUnit.fluffBio;
    }
}
