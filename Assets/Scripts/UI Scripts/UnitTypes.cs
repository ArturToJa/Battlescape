using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UnitTypes : MonoBehaviour
{
    [SerializeField] Sprite Cannonmeat;
    [SerializeField] Sprite Shooters;
    [SerializeField] Sprite Infantry;
    [SerializeField] Sprite Cavalry;
    [SerializeField] Sprite Creatures;

    [SerializeField] GameObject SpecialUnits;
    [SerializeField] GameObject Summonned;
    [SerializeField] GameObject Construct;
    [SerializeField] GameObject Colossus;
    [SerializeField] GameObject Undead;

    [SerializeField] GameObject Poisonous;
    [SerializeField] GameObject Flying;
    [SerializeField] GameObject CannotAttack;
    [SerializeField] GameObject StopRetaliation;
    [SerializeField] GameObject NoMeleePenalty;
    [SerializeField] GameObject Assassin;
    [SerializeField] Text AssassinValue;

    [SerializeField] GameObject BasicType;
    [SerializeField] Image BasicTypeSprite;
    public BattlescapeLogic.Unit myUnit;
    BattlescapeLogic.Unit myPrevUnit;
    public bool RealOne;
    public bool ManagementScene;
    [SerializeField] UnitStatShower USS;


    void Update()
    {        
        if (RealOne)
        {            
            if (GameRound.instance.currentPlayer.selectedUnit == null)
            {
                return;
            }
            myUnit = GameRound.instance.currentPlayer.selectedUnit;
        }
        else if (ManagementScene == false)
        {
            //if (MouseManager.Instance.MouseoveredUnit == null || EnemyTooltipHandler.isOn)
            //{
            //    return;
            //}
            //myUnit = MouseManager.Instance.MouseoveredUnit;           
        }
        else
        {
            myUnit = USS.currUnit;            
        }
        if (myPrevUnit != myUnit)
        {            
            //UpdateTheBasicType();
            //CheckForNonBasicTypes();
        }

        myPrevUnit = myUnit;
    }



    //private void CheckForNonBasicTypes()
    //{
    //    foreach (GameObject icon in IconsToDelete)
    //    {
    //        Destroy(icon);
    //    }
    //    //CheckForSpecial();
    //    //CheckForConstruct();
    //    //CheckForSummonned();
    //    //CheckForColossus();
    //    //CheckForUndead();
    //    //CheckForPoisonous();
    //    //CheckForFlying();
    //    //CheckForCannotAttack();
    //    //CheckForStopRetaliation();
    //    //CheckForNoMeleePen();
    //    //CheckForAssassin();
    //    //if (ManagementScene)
    //    //{
    //    //    foreach (PassiveAbility ability in myUnit.GetComponents<PassiveAbility>())
    //    //    {
    //    //        if (ability is BasicStatModifier || ability.HasIcon == false)
    //    //        {
    //    //            continue;
    //    //        }
    //    //        GameObject icon = shower.AddAbilityIcon((GameObject)Resources.Load(ability.AbilityIconName), false);
    //    //        IconsToDelete.Add(icon);
    //    //    }
    //    //}
    //}

    //private void CheckForAssassin()
    //{
    //    AssassinStatModifier modifier = myUnit.GetComponent<AssassinStatModifier>();
    //    bool active = modifier != null;
    //    Assassin.SetActive(active);
    //    if (active)
    //    {
    //        AssassinValue.text = modifier.AttackModifierVersusUnitType.ToString();
    //    }
    //}

    //private void CheckForStopRetaliation()
    //{
    //    StopRetaliation.SetActive(myUnit.isStoppingRetaliation);
    //}

    //private void CheckForNoMeleePen()
    //{
    //    NoMeleePenalty.SetActive(false);
    //    //NoMeleePenalty.SetActive(myUnit.IsRanged() && myUnit.GetComponent<ShootingScript>().doesNotLoseAttackInCombat);        
    //}

    //private void CheckForCannotAttack()
    //{
    //    CannotAttack.SetActive(!myUnit.CanAttack);
    //}    

    //private void CheckForFlying()
    //{
    //    Flying.SetActive(myUnit.movement is FlyingMovement);
    //}

    //private void CheckForPoisonous()
    //{
    //    Poisonous.SetActive(myUnit.isPoisonous);
    //}

    //private void CheckForUndead()
    //{
    //    Undead.SetActive(myUnit.isUndead);
    //}

    //private void CheckForColossus()
    //{
    //    Colossus.SetActive(myUnit.isColossal);
    //}

    //private void CheckForSummonned()
    //{
    //    Summonned.SetActive(myUnit.isSummonned);
    //}

    //private void CheckForConstruct()
    //{
    //    Construct.SetActive(myUnit.isConstruct);
    //}

    //private void CheckForSpecial()
    //{
    //    SpecialUnits.SetActive(myUnit.isSpecial);   
    //}



    //private void UpdateTheBasicType()
    //{
    //    switch (myUnit.unitType)
    //    {
    //        case UnitType.Null:

    //            if (myUnit.IsRanged())
    //            {
    //                BasicType.SetActive(true);
    //                BasicTypeSprite.sprite = Shooters;
    //                Name = "Ranged Unit";
    //                Description = "This unit can shoot!";
    //                break;
    //            }
    //            else
    //            {
    //                BasicType.SetActive(false);
    //                break;
    //            }
    //        case UnitType.Cannonmeat:
    //            BasicType.SetActive(true);
    //            BasicTypeSprite.sprite = Cannonmeat;
    //            Name = "Cannonmeat";
    //            Description= "Weakest, but cheap unit.";
    //            break;
    //        case UnitType.Shooters:
    //            BasicType.SetActive(true);
    //            BasicTypeSprite.sprite = Shooters;
    //            Name = "Ranged Unit";
    //            Description = "This unit can shoot!";
    //            break;
    //        case UnitType.Infantry:
    //            BasicType.SetActive(true);
    //            BasicTypeSprite.sprite = Infantry;
    //            Name = "Infantry";
    //            Description = "Unit of footman.";
    //            break;
    //        case UnitType.Cavalry:
    //            BasicType.SetActive(true);
    //            BasicTypeSprite.sprite = Cavalry;
    //            Name = "Cavalry";
    //            Description = "Unit of horseman";
    //            break;
    //        case UnitType.Creatures:
    //            BasicType.SetActive(true);
    //            BasicTypeSprite.sprite = Creatures;
    //            Name = "Creatures";
    //            Description = "Wild beasts.";
    //            break;
    //        default:
    //            Debug.LogError("Why no unittype?");
    //            break;
    //    }
    //}
}
