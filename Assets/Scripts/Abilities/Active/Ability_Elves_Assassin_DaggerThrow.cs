using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Elves_Assassin_DaggerThrow : Ability_Basic
{
    [SerializeField] GameObject vfx;
    [SerializeField] int Damage;
    [SerializeField] GameObject Dagger;
    [SerializeField] Transform daggerSpawn;
    public float Speed = 10f;
    protected override void OnStart()
    {
        return;
    }

    protected override void OnUpdate()
    {
        return;
    }




    protected override bool IsUsableNow()
    {
        return true;        
    }

    protected override void Use()
    {
        BattlescapeGraphics.ColouringTool.UncolourAllTiles();
        ColourTiles();
    }

    protected override void CancelUse()
    {
        BattlescapeGraphics.ColouringTool.UncolourAllTiles();
    }





    public override bool ActivationRequirements()
    {
        return
            //MouseManager.Instance.mouseoveredTile != null &&
            //MouseManager.Instance.mouseoveredTile.myUnit != null &&
            //MouseManager.Instance.mouseoveredTile.myUnit.GetMyOwner() != myUnit.GetMyOwner() &&
            //Helper.AreTilesInRange(MouseManager.Instance.mouseoveredTile,myUnit.currentPosition, 2);
            true;
    }

    public override void Activate()
    {
        StartCoroutine(Throw());
    }

    IEnumerator Throw()
    {
        //myUnit.LookAtTheTarget(Target.transform.position, 60);
        ////myUnit.GetComponent<AnimController>().Cast();
        /* var temp = Instantiate(Dagger, daggerSpawn.position, Dagger.transform.rotation);
         temp.GetComponent<ProjectileScript>().Target = Target.transform.position;*/
        //LaunchDagger(Target.transform.position, speed);
        //Target.myUnit.DealDamage(Damage + myUnit.statistics.GetCurrentAttack() - Target.myUnit.statistics.GetCurrentDefence(), true, false, false);
        PassiveAbility_Buff.AddBuff(Target.myUnit.gameObject, 2, 0, -2, -2, Target.myUnit.statistics.currentMaxNumberOfRetaliations, "AssassinDebuff", vfx, 0, false, true, false);
        PopupTextController.AddParalelPopupText("-" + (Damage - Target.myUnit.statistics.GetCurrentDefence()), PopupTypes.Damage);
        Log.SpawnLog("Assassin throws a deadly dagger at " + Target.myUnit.name + ", dealing " + (Damage - Target.myUnit.statistics.GetCurrentDefence()) + " damage.");
        
        
        //Animate the shit
        // do actual logic
        yield return null;
        FinishUsing();
        yield return new WaitForSeconds(1.0f);
        CreateVFXOn(Target.transform, BasicVFX.transform.rotation);
    }





    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
    }

    public override GameObject AI_ChooseTarget()
    {
        return Target.gameObject;
    }

    public override bool AI_IsGoodToUseNow()
    {
        //for (int x = 0; x < Global.instance.map.mapWidth; x++)
        //    for (int z = 0; z < Global.instance.map.mapHeight; z++)
        //    {
        //        //if (Helper.AreTilesInRange(Global.instance.map.board[x, z],myUnit.currentPosition, 2) && Global.instance.map.board[x, z].myUnit != null && Global.instance.map.board[x, z].myUnit.GetMyOwner() != myUnit.GetMyOwner() && (Global.instance.map.board[x, z].myUnit.statistics.cost > 5 || Global.instance.map.board[x, z].myUnit is BattlescapeLogic.Hero))
        //        //{
        //        //    Target = Global.instance.map.board[x, z];
        //        //    return true;
        //        //}
        //    }
        return false;
    }



    protected override void SetTarget()
    {
        Target = //MouseManager.Instance.mouseoveredTile;
            null;
    }
    protected override void ColourTiles()
    {
        //for (int x = 0; x < Global.instance.map.mapWidth; x++)
        //    for (int z = 0; z < Global.instance.map.mapHeight; z++)
        //    {
        //        //if (Helper.AreTilesInRange(myUnit.currentPosition, Global.instance.map.board[x, z], 2) && Global.instance.map.board[x, z].myUnit != null && Global.instance.map.board[x, z].myUnit.GetMyOwner() != myUnit.GetMyOwner())
        //        //{
        //        //    BattlescapeGraphics.ColouringTool.ColourObject(Global.instance.map.board[x, z],Color.red);
        //        //}
        //    }

    }

    public void LaunchDagger(Vector3 target, float speed)
    {
        PlayAbilitySound();
        GameObject projectile = Instantiate(Dagger) as GameObject;
        projectile.transform.position = daggerSpawn.position;
        projectile.transform.LookAt(target + new Vector3(0, 0.5f, 0));
        projectile.transform.Rotate(Vector3.left, 45, Space.Self);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed;
        //shooter.lastShotProjectile = projectile.GetComponent<ProjectileScript>();
        //projectile.GetComponent<ProjectileScript>().speed = speed;
        //projectile.GetComponent<ProjectileScript>().Target = target;
        //        projectile.GetComponent<ProjectileScript>().myShooter = shooter;
    }




}
