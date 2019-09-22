using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{
    [SerializeField] int dropValue;
    UnitScript unit;


    public void Drop()
    {
      /*  return;
        unit = MouseManager.Instance.SelectedUnit;
        if (unit.GetComponent<HeroScript>() != null)
        {
            if (unit.isGreen)
            {
                VictoryLossChecker.GreenScore += dropValue * 2;
            }
            else
            {
                VictoryLossChecker.RedScore += dropValue * 2;
            }
            PopupTextController.CreatePopupText("+ " + dropValue + " Victory Points", unit.transform, PopupTypes.Info);
            return;
        }
        else
        {
            switch (dropValue)
            {
                case 1:
                    if (unit.isRanged)
                    {
                        unit.GetComponent<ShootingScript>().currShootingRange += 1;
                        PopupTextController.CreatePopupText("+1 Range", unit.transform, PopupTypes.Info);
                        break;
                    }
                    else
                    {
                        unit.GetComponent<UnitMovement>().currMoveSpeed += 1;
                        PopupTextController.CreatePopupText("+1 Movement", unit.transform, PopupTypes.Info);
                        break;
                    }
                case 2:
                    if (unit.isRanged && unit.GetComponent<ShootingScript>().shortDistance)
                    {
                        unit.GetComponent<ShootingScript>().shortDistance = false;
                        unit.GetComponent<ShootingScript>().currShootingRange += 1;
                        PopupTextController.CreatePopupText("Long Range!", unit.transform, PopupTypes.Info);
                        break;
                    }
                    if (unit.currentHP < unit.maxHP && unit.currentHP < 3)
                    {
                        unit.DealDamage(-1, false, false, false);
                        PopupTextController.CreatePopupText("+1 HP", unit.transform, PopupTypes.Info);
                    }
                    else if (unit.CurrAttack > unit.CurrDefence && unit.unitType != UnitType.Cannonmeat)
                    {
                        unit.CurrAttack += 2;
                        PopupTextController.CreatePopupText("+2 Attack", unit.transform, PopupTypes.Info);
                    }
                    else if (unit.CurrAttack < unit.CurrDefence && unit.unitType != UnitType.Cannonmeat)
                    {
                        unit.CurrDefence += 2;
                        PopupTextController.CreatePopupText("+2 Defence", unit.transform, PopupTypes.Info);
                    }
                    else
                    {
                        unit.CurrAttack += 1;
                        unit.CurrDefence += 1;
                        PopupTextController.CreatePopupText("+1 Attack and Defence", unit.transform, PopupTypes.Info);
                    }
                    break;
                case 3:
                    unit.GetComponent<UnitMovement>().currMoveSpeed += 1;
                    unit.CurrAttack += 1;
                    unit.CurrDefence += 1;
                    if (unit.currentHP < unit.maxHP)
                    {
                        unit.DealDamage(-1, false, false, false);
                    }
                    if (unit.isRanged)
                    {
                        unit.GetComponent<ShootingScript>().currShootingRange += 1;
                        if (unit.GetComponent<ShootingScript>().shortDistance)
                        {
                            unit.GetComponent<ShootingScript>().shortDistance = false;
                        }
                    }
                    PopupTextController.CreatePopupText("Complete Upgrade", unit.transform, PopupTypes.Info);
                    break;
                default:
                    break;
            }
        }
    */
    }
        

}
