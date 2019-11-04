using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_Elves_Raven_EmpoweredShot : Ability_Basic
{
    [SerializeField] protected int Damage;
    protected ShootingScript myShooter;
    protected bool MadeEmpoweredShotThisTurn = false;
    //this bool is so that all versions of EmpoweredShot count as using this ability.
    [SerializeField] GameObject Projectile;


    protected override void OnStart()
    {
        myShooter = GetComponent<ShootingScript>();
    }

    protected override void OnUpdate()
    {
        return;
    }

    public override void OnNewTurn()
    {
        base.OnNewTurn();
        MadeEmpoweredShotThisTurn = false;
    }


    protected override bool IsUsableNow()
    {

        return
            MadeEmpoweredShotThisTurn == false;
    }

    

    protected override void Use()
    {
        return;
    }

    protected override void CancelUse()
    {
        return;
    }



    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            MouseManager.Instance.mouseoveredTile.myUnit != null &&
            MouseManager.Instance.mouseoveredTile.myUnit.PlayerID != myUnit.PlayerID &&
            ShootingScript.WouldItBePossibleToShoot(myUnit, this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key;
    }

    public override void Activate()
    {
        MadeEmpoweredShotThisTurn = true;
        foreach (Ability_Elves_Raven_EmpoweredShot variant in GetComponents<Ability_Elves_Raven_EmpoweredShot>())
        {
            if (variant != this)
            {
                variant.UsesLeft--;
            }
        }
        //myUnit.LookAtTheTarget(Target.transform.position, myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        myShooter.CurrentProjectile = Projectile;
        myUnit.GetComponent<AnimController>().Cast();
        Target.myUnit.DealDamage(Damage + myUnit.statistics.GetCurrentAttack() - Target.myUnit.statistics.GetCurrentDefence(), true, false, true);
        myUnit.statistics.numberOfAttacks = 0;
        
        myShooter.myTarget = Target.myUnit.transform.position;
        PopupTextController.AddParalelPopupText("-" + (Damage + myUnit.statistics.GetCurrentAttack() - Target.myUnit.statistics.GetCurrentDefence()), PopupTypes.Damage);        
     }    




    public override void AI_Activate(GameObject Target)
    {
        return;
    }

    public override GameObject AI_ChooseTarget()
    {
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
    }         

    

    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

    
}
