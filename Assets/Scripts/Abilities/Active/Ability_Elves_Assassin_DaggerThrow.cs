﻿using System.Collections;
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
        ColouringTool.UncolourAllTiles();
        ColourTiles();
    }

    protected override void CancelUse()
    {
        ColouringTool.UncolourAllTiles();
    }





    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            MouseManager.Instance.mouseoveredTile.myUnit != null &&
            MouseManager.Instance.mouseoveredTile.myUnit.PlayerID != myUnit.PlayerID &&
            Helper.AreTilesInRange(MouseManager.Instance.mouseoveredTile,myUnit.myTile, 2);

    }

    public override void Activate()
    {
        StartCoroutine(Throw());
    }

    IEnumerator Throw()
    {
        myUnit.GetComponent<UnitMovement>().LookAtTheTarget(Target.transform.position, 60);
        GetComponent<AnimController>().Cast();
        /* var temp = Instantiate(Dagger, daggerSpawn.position, Dagger.transform.rotation);
         temp.GetComponent<ProjectileScript>().Target = Target.transform.position;*/
        //LaunchDagger(Target.transform.position, speed);
        Target.myUnit.DealDamage(Damage + myUnit.CurrentAttack - Target.myUnit.CurrentDefence, true, false, false);
        PassiveAbility_Buff.AddBuff(Target.myUnit.gameObject, 2, 0, -2, -2, -100, false, "AssassinDebuff", vfx, 0, false, true, false);
        PopupTextController.AddParalelPopupText("-" + (Damage - Target.myUnit.CurrentDefence), PopupTypes.Damage);
        Log.SpawnLog("Assassin throws a deadly dagger at " + Target.myUnit.name + ", dealing " + (Damage - Target.myUnit.CurrentDefence) + " damage.");
        
        
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
        for (int x = 0; x < Map.mapWidth; x++)
            for (int z = 0; z < Map.mapHeight; z++)
            {
                if (Helper.AreTilesInRange(Map.Board[x, z],myUnit.myTile, 2) && Map.Board[x, z].myUnit != null && Map.Board[x, z].myUnit.PlayerID != myUnit.PlayerID && (Map.Board[x, z].myUnit.Value > 5 || Map.Board[x, z].myUnit.GetComponent<HeroScript>() != null))
                {
                    Target = Map.Board[x, z];
                    return true;
                }
            }
        return false;
    }



    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }
    protected override void ColourTiles()
    {
        for (int x = 0; x < Map.mapWidth; x++)
            for (int z = 0; z < Map.mapHeight; z++)
            {
                if (Helper.AreTilesInRange(myUnit.myTile, Map.Board[x, z], 2) && Map.Board[x, z].myUnit != null && Map.Board[x, z].myUnit.PlayerID != myUnit.PlayerID)
                {
                    ColouringTool.SetColour(Map.Board[x, z],Color.red);
                }
            }

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
        projectile.GetComponent<ProjectileScript>().speed = speed;
        projectile.GetComponent<ProjectileScript>().Target = target;
        //        projectile.GetComponent<ProjectileScript>().myShooter = shooter;
    }




}
