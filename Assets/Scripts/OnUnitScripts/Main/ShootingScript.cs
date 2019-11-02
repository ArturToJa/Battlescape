using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[DisallowMultipleComponent]
public class ShootingScript : MonoBehaviour
{
    UnitScript theUnit;

    [HideInInspector]
    public int currMinimalRange;
    public int baseMinimalRange;
    public bool shortDistance;
    public bool AOEAttack;

    public bool CanShoot
    {
        get
        {
            return
                theUnit.CanStillAttack() == true &&
                theUnit.IsFrozen == false;
        }
    }
    public GameObject Projectile;
    [HideInInspector] public GameObject CurrentProjectile;
    public float PROJECTILE_SPEED;
    public Transform ProjectileLauncher;
    public ProjectileScript lastShotProjectile;
    public bool doesNotLoseAttackInCombat;

    public Sound[] ShootSounds;
    AudioSource shootSource;

    public Sound[] HitShotSounds;
    AudioSource hitShotSource;
    [HideInInspector] public Vector3 myTarget;

    public void Shoot()
    {
        PlayRandomShot();
        CombatController.Instance.DoShot(this, myTarget, CurrentProjectile);
        CurrentProjectile = Projectile;
    }       
    public void PlayRandomShot()
    {
        Sound s = ShootSounds[Random.Range(0, ShootSounds.Length)];
        s.oldSource = shootSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    public void PlayRandomHitShot()
    {
        Sound s = HitShotSounds[Random.Range(0, HitShotSounds.Length)];
        s.oldSource = hitShotSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    private void Start()
    {
        CurrentProjectile = Projectile;
        currMinimalRange = baseMinimalRange;
        theUnit = this.GetComponent<UnitScript>();
        hitShotSource = gameObject.AddComponent<AudioSource>();
        shootSource = gameObject.AddComponent<AudioSource>();
    }
    private void Update()
    {
        if (PossibleToShootAt(MouseManager.Instance.MouseoveredUnit, false) && WeDontUseAbilityRightNow())
        {
            CheckForInput();
        }
    }


    private bool WeDontUseAbilityRightNow()
    {
        return (Ability_Basic.currentlyUsedAbility == null || (Ability_Basic.currentlyUsedAbility != null && Ability_Basic.currentlyUsedAbility.gameObject != this.gameObject));
    }

    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CombatController.Instance.Shoot(theUnit, MouseManager.Instance.MouseoveredUnit, CheckBadRange(MouseManager.Instance.MouseoveredUnit.gameObject), AOEAttack);
            theUnit.statistics.numberOfAttacks = 0;
        }
    }

    public bool CheckBadRange(GameObject target)
    {
        if (!shortDistance)
        {
            return false;
        }
        else
        {
            Bounds Range = new Bounds(this.transform.position, new Vector3(theUnit.statistics.GetCurrentAttackRange() + 0.25f, 5, theUnit.statistics.GetCurrentAttackRange() + 0.25f));
            return !(Range.Contains(target.transform.position));
        }
    }

    public bool PossibleToShootAt(UnitScript target, bool isCursor)
    {
        if (Application.isEditor && Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log(!theUnit.CheckIfIsInCombat() + "," + target != null + "," + IsTargetInRange(target.transform.position) + "," + CanShoot + "," + (GameStateManager.Instance.GameState == GameStates.ShootingState));
        }
        return (
                  MouseManager.Instance.SelectedUnit != null
                  && MouseManager.Instance.SelectedUnit == theUnit
                  && !theUnit.CheckIfIsInCombat() && target != null
                  && target != null && target.PlayerID != MouseManager.Instance.SelectedUnit.PlayerID
                  && IsTargetInRange(target.transform.position)
                  && CanShoot
                  && GameStateManager.Instance.GameState == GameStates.ShootingState
                  && theUnit.PlayerID == Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index
                  && IsInLineOfSight(transform.position, target.transform.position, isCursor)
               );
    }

    public bool IsAbleToShoot()
    {
        return (!theUnit.CheckIfIsInCombat() && CanShoot);
    }

    public static bool IsInLineOfSight(Vector3 start, Vector3 target, bool showNoPopup)
    {
        Vector3 startPos = start  /*+Vector3.up * 0.01f*/;
        Vector3 endPos = target  /*+Vector3.up * 0.01f*/;
        Vector3 direction = endPos - startPos;
        foreach (RaycastHit hit in Physics.RaycastAll(startPos, direction, Vector3.Distance(startPos, endPos) + 0.01f))
        {
            if (hit.collider.transform.gameObject.tag == "Tile")
            {
                //MIND YOU - this HAS TO BE recreated in a new way (I have an idea, how) in new code if we want to keep this mechanic!
                //if (/*hit.collider.transform.gameObject.GetComponent<Tile>().isShootable == false*/ false)
                //{
                //    if (Input.GetMouseButtonDown(0) && showNoPopup == false)
                //    {
                //        PopupTextController.AddPopupText("Can not shoot through this obstacle!", PopupTypes.Info);
                //    }
                //    return false;
                //}
                
            }
        }
        return true;
    }

    public bool IsTargetInRange(Vector3 target)
    {
        Bounds maxiRange = new Bounds(this.transform.position, new Vector3(2 * theUnit.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * theUnit.statistics.GetCurrentAttackRange() + 0.25f));
        if (currMinimalRange > 0)
        {
            Bounds miniRange = new Bounds(this.transform.position, new Vector3(2 * currMinimalRange + 0.25f, 5, 2 * currMinimalRange + 0.25f));
            return (maxiRange.Contains(target) && miniRange.Contains(target) == false);
        }
        else
        {
            return (maxiRange.Contains(target));
        }

    }

    public static KeyValuePair<bool, bool> WouldItBePossibleToShoot(UnitScript shooter, Vector3 start, Vector3 target)
    {

        Bounds FullRange = new Bounds(start, new Vector3(2 * shooter.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * shooter.statistics.GetCurrentAttackRange() + 0.25f));
        //Bounds BadRange = new Bounds(start, new Vector3(shooter.statistics.GetCurrentAttackRange() + 0.25f, 5, shooter.statistics.GetCurrentAttackRange() + 0.25f));
        bool key = false;
        if (shooter.statistics.minimalAttackRange > 0)
        {
            Bounds miniRange = new Bounds(start, new Vector3(2 * shooter.statistics.minimalAttackRange + 0.25f, 5, 2 * shooter.statistics.minimalAttackRange + 0.25f));
            key = miniRange.Contains(target) == false && FullRange.Contains(target) && IsInLineOfSight(start, target, true);
        }
        else
        {
            key = FullRange.Contains(target) && IsInLineOfSight(start, target, true);
        }
        bool value = false;
        //if (shooter.shortDistance)
        //{
        //    value = (BadRange.Contains(target) == false);
        //}
        //THIS above should somehow become a thing!
        //else
        //{
        //    value = false;
        //}
        return new KeyValuePair<bool, bool>(key, value);
    }

    public static List<UnitScript> PossibleTargets(UnitScript shooter, Vector3 start)
    {
        Bounds Range = new Bounds(start, new Vector3(2 * shooter.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * shooter.statistics.GetCurrentAttackRange() + 0.25f));
        Bounds miniRange = new Bounds(start, Vector3.zero);
        if (shooter.statistics.minimalAttackRange > 0)
        {
            miniRange = new Bounds(start, new Vector3(2 * shooter.statistics.minimalAttackRange + 0.25f, 5, 2 * shooter.statistics.minimalAttackRange + 0.25f));
        }
        List<UnitScript> targets = new List<UnitScript>();
        foreach (UnitScript enemy in VictoryLossChecker.GetEnemyUnitList())
        {
            if (Range.Contains(enemy.transform.position) == true && miniRange.Contains(enemy.transform.position) == false)
            {
                targets.Add(enemy);
            }
        }
        return targets;
    }
}
