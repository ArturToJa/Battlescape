using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class CombatController : MonoBehaviour
{
    PhotonView photonView;
    public static CombatController Instance { get; private set; }
    int waitRoutines = 0;
    public UnitScript AttackTarget;
    public UnitScript AttackingUnit;
    //public int TresholdKicker = 4;
    public int DiceFaceNumber = 12;
    public int DamageForRandomization = 5;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        ///////////////////////////////////////test
        if (Input.GetKeyDown(KeyCode.K))
        {
            var temp = new DamageCalculator();
            UnitScript Attacker = MouseManager.Instance.SelectedUnit;
            UnitScript Defender = MouseManager.Instance.MouseoveredUnit;
            for (int i = 0; i < 1000; i++)
            {
                Debug.Log(temp.GetDmgWithProbability(Attacker, Defender));

            }
        }




        CheckForInput();
        /* if (Input.GetKeyDown(KeyCode.W))
         {
             Debug.LogError(GameStateManager.Instance.GameState);
             Debug.LogError(GameStateManager.IsGreenActive());
         }*/
    }

    public void OnNewTurn()
    {
        MakeAIWait(1f);
    }

    void CheckForInput()
    {
        if (GameStateManager.Instance.IsCurrentPlayerLocal() == false)
        {
            return;
        }
        if (Helper.IsOverNonHealthBarUI())
        {
            return;
        }
        if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.EnemyList.Contains(MouseManager.Instance.MouseoveredUnit) && !MouseManager.Instance.SelectedUnit.hasAttacked && MouseManager.Instance.SelectedUnit.CanAttack)
        {
            SendCommandToAttack(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit, false, !MouseManager.Instance.SelectedUnit.isStoppingRetaliation && MouseManager.Instance.MouseoveredUnit.DoesRetaliate);
            MouseManager.Instance.SelectedUnit.hasAttacked = true;
        }
        if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.EnemyList.Contains(MouseManager.Instance.MouseoveredUnit.GetComponent<UnitScript>()) && !MouseManager.Instance.SelectedUnit.hasAttacked && MouseManager.Instance.SelectedUnit.CanAttack == false)
        {
            PopupTextController.AddPopupText("This unit cannot attack!", PopupTypes.Info);
        }
        //THIS below is just for testing and allows to shoot with Z key. XD. Its just for quick testing if a shot works ;) REAL shooting exists inside ShootingScript currently apparently
        /*if (MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>() != null && Input.GetKeyDown(KeyCode.Z) && MouseManager.Instance.MouseoveredUnit != null && Application.isEditor)
        {
            Shoot(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit, false);
        }*/
        if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.mouseoveredDestructible != null && MouseManager.Instance.SelectedUnit.CanAttackDestructible(MouseManager.Instance.mouseoveredDestructible.GetComponent<DestructibleScript>(), true))
        {
            if (GameStateManager.Instance.MatchType == MatchTypes.Online)
            {
                photonView.RPC
                    (
                    "RPCMeleeAttackObstacle",
                    PhotonTargets.All,
                    Mathf.RoundToInt(MouseManager.Instance.SelectedUnit.transform.position.x),
                    Mathf.RoundToInt(MouseManager.Instance.SelectedUnit.transform.position.z),
                    Mathf.RoundToInt(MouseManager.Instance.mouseoveredDestructible.transform.position.x),
                    Mathf.RoundToInt(MouseManager.Instance.mouseoveredDestructible.transform.position.z)
                   );

            }
            else
            {
                MeleeAttackObstacle(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredDestructible.GetComponent<DestructibleScript>());
            }

        }
    }

    void MeleeAttackObstacle(UnitScript Attacker, DestructibleScript target)
    {
        Debug.Log("Melee attacking obstacle - debugging fencer bug");
        Attacker.GetComponent<UnitMovement>().LookAtTheTarget(target.transform.position, Attacker.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        Attacker.GetComponentInChildren<AnimController>().AnimateAttack();
        target.GetDamaged(Attacker.CurrentAttack + 1);
        Attacker.hasAttacked = true;
        CheckIfLastAttacker();
        target.GetComponentInChildren<ObjectHP>().SwitchTrigger();
    }

    [PunRPC]
    void RPCMeleeAttackObstacle(int unitX, int unitZ, int targetX, int targetZ)
    {
        UnitScript Attacker = Map.Board[unitX, unitZ].myUnit;
        DestructibleScript target = Map.Board[targetX, targetZ].GetComponentInChildren<DestructibleScript>();
        MeleeAttackObstacle(Attacker, target);
    }

    public IEnumerator ShootToObstacle(ShootingScript shooter, Vector3 target)
    {

        SendCommandToLaunchProjectile(shooter, target);        

        yield return new WaitForSeconds((Vector3.Distance(target, shooter.ProjectileLauncher.position) * Mathf.PI) / (2 * Mathf.Sqrt(2)) / shooter.PROJECTILE_SPEED);
        DestroyObstacle(Map.Board[Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z)]);
        if (Application.isEditor)
        {
            DestroyImmediate(shooter.lastShotProjectile.gameObject);
        }
        else
        {
            Destroy(shooter.lastShotProjectile.gameObject);
        }

    }

    void DestroyObstacle(Tile tile)
    {
        List<GameObject> deadObjects = new List<GameObject>();
        tile.DestroyObstacle();
        for (int i = 0; i < tile.transform.childCount; i++)
        {
            if (tile.transform.GetChild(i).gameObject.tag == "Dice")
            {
                deadObjects.Add(tile.transform.GetChild(i).gameObject);
            }
        }

        while (deadObjects.Count != 0)
        {
            StartCoroutine(DestroySlowly(deadObjects[0]));
            deadObjects.Remove(deadObjects[0]);
        }
    }

    IEnumerator DestroySlowly(GameObject target)
    {
        if (target.transform.root.GetComponentInChildren<Obstacle>() != null)
        {
            if (target.transform.root.GetComponentInChildren<Obstacle>().IsNotBeingDestroyed == true)
            {
                target.transform.root.GetComponentInChildren<Obstacle>().IsNotBeingDestroyed = false;
            }
        }


        while (target.GetComponent<Renderer>().material.color.a > 0)
        {
            foreach (var item in target.GetComponentsInChildren<Renderer>())
            {
                
                Color temp = item.material.color;
                temp.a = Mathf.Lerp(temp.a, -1, 0.5f * Time.deltaTime);
                item.material.color = temp;
            }

            target.transform.position += Vector3.down * 2f * Time.deltaTime;
            yield return null;
        }
        if (Application.isEditor)
        {
            DestroyImmediate(target.gameObject);
        }
        else
        {
            Destroy(target.gameObject);
        }
    }

    public void SendCommandToAttack(UnitScript Attacker, UnitScript Defender, bool badRangeShooting, bool retaliatable)
    {
        int hits;
        int damage = CalculateDamage(Attacker, Defender, badRangeShooting, out hits);
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC(
                "RPCAttack", PhotonTargets.All,
                Mathf.RoundToInt(Attacker.transform.position.x),
                Mathf.RoundToInt(Attacker.transform.position.z),
                Mathf.RoundToInt(Defender.transform.position.x),
                Mathf.RoundToInt(Defender.transform.position.z),
                retaliatable,
                damage, hits
                );
        }
        else
        {
            Attack(Attacker, Defender, retaliatable, damage, hits);
        }

    }

    int CalculateDamage(UnitScript Attacker, UnitScript Defender, bool badRangeShooting, out int hits)
    {
        DamageCalculator dc = new DamageCalculator();
        hits = dc.GetHits(Attacker, Defender, badRangeShooting);
        return dc.CalculateAmountOfDamage(Attacker, Defender, badRangeShooting,hits);
    }

    [PunRPC]
    void RPCAttack(int AttackerX, int AttackerZ, int DefenderX, int DefenderZ, bool retaliatable, int damage, int hits)
    {
        UnitScript Attacker = Map.Board[AttackerX, AttackerZ].myUnit;
        UnitScript Defender = Map.Board[DefenderX, DefenderZ].myUnit;
        Attack(Attacker, Defender, retaliatable, damage, hits);
    }

    public event Action<UnitScript, UnitScript, int> AttackEvent;

    public void Attack(UnitScript Attacker, UnitScript Defender, bool retaliatable, int damage, int hits)
    {
        if (Attacker.GetComponent<ShootingScript>() != null)
        {
            Attacker.GetComponent<ShootingScript>().myTarget = Defender.transform.position;
            // this is mostly done HERE for melee attacks of Ravens...
        }
        if (AttackEvent != null)
        {
            AttackEvent(Attacker, Defender, damage);
        }
        AttackingUnit = Attacker;
        AttackTarget = Defender;
        DamageCalculator dc = new DamageCalculator();
        dc.DealDamage(Attacker, Defender, damage, Attacker.isPoisonous, retaliatable, hits);
        CheckIfLastAttacker();
    }

    public static bool CheckIfLastAttacker()
    {
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
        {
            foreach (UnitScript ally in Player.Players[TurnManager.Instance.PlayerHavingTurn].PlayerUnits)
            {
                if (ally.hasAttacked == false && ally.CheckIfIsInCombat())
                {
                    return false;
                }
            }
            PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            return true;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
        {
            foreach (UnitScript ally in Player.Players[TurnManager.Instance.PlayerHavingTurn].PlayerUnits)
            {
                if (ally.GetComponent<ShootingScript>() != null && ally.GetComponent<ShootingScript>().CanShoot)
                {
                    return false;
                }
            }
            PopupTextController.AddPopupText("No more units can shoot!", PopupTypes.Info);
            return true;
        }
        return false;
    }

    public void SendCommandToRetaliate()
    {
        //note this Attack command is already networked therefore it does not need to be networked a second time inside this command we gonna send here
        SendCommandToAttack(AttackTarget, AttackingUnit, false, false);
        //note also that AttackTarget is now the guy who retaliates, and AttackingUnit is getting hit.

        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCRetaliate", PhotonTargets.All);
        }
        else
        {
            Retaliate();
        }
    }

    [PunRPC]
    void RPCRetaliate()
    {
        Retaliate();
    }

    public void Retaliate()
    {
        Log.SpawnLog(AttackingUnit.name + " strikes back!");
        AttackingUnit.HasRetaliatedThisTurn = true;
        GameStateManager.Instance.FinishRetaliation();
        MakeAIWait(1f);
    }

    public void RetaliationForAI()
    {
        StartCoroutine(AIRetaliationRoutine());
    }

    IEnumerator AIRetaliationRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        SendCommandToRetaliate();
        AI_Controller.isRetaliating = false;
    }

    public void SendCommandToNotRetaliate()
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCNoRetaliation", PhotonTargets.All);
        }
        else
        {
            DoNotRetaliate();
        }
    }


    [PunRPC]
    void RPCNoRetaliation()
    {
        DoNotRetaliate();
    }

    public void DoNotRetaliate()
    {
        GameStateManager.Instance.FinishRetaliation();
        AttackTarget = null;
    }

    public void Shoot(UnitScript Attacker, UnitScript Defender, bool badRange, bool AOE)
    {

        SendCommandToAttack(Attacker, Defender, badRange, false);
        if (AOE)
        {
            foreach (Tile tile in Defender.myTile.neighbours)
            {
                if (tile.myUnit != null)
                {
                    SendCommandToAttack(Attacker, tile.myUnit, badRange, false);
                }
            }
        }
        SendCommandToLaunchProjectile(Attacker.GetComponent<ShootingScript>(), Defender.transform.position);
    }

    public void SendCommandToLaunchProjectile(ShootingScript shooter, Vector3 enemy)
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC(
                "RPCLaunchProjectile", PhotonTargets.All,
                Mathf.RoundToInt(shooter.transform.position.x),
                Mathf.RoundToInt(shooter.transform.position.z),
                Mathf.RoundToInt(enemy.x),
                Mathf.RoundToInt(enemy.z)
                );
        }
        else
        {
            LaunchProjectile(shooter, enemy);
        }
    }

    public void LaunchProjectile(ShootingScript shooter, Vector3 enemy)
    {
        shooter.GetComponentInChildren<AnimController>().Shoot();
    }
    /*public void LaunchProjectile(ShootingScript shooter, Vector3 enemy, GameObject projectile, int shots)
    {
        StartCoroutine(LaunchProjectileRoutine(shooter, enemy, projectile, shots));
    }*/

    [PunRPC]
    void RPCLaunchProjectile(int shooterX, int shooterZ, int targetX, int targetZ)
    {
        ShootingScript shooter = Map.Board[shooterX, shooterZ].myUnit.GetComponent<ShootingScript>();
        Vector3 target = new Vector3(targetX, 0, targetZ);
        LaunchProjectile(shooter, target);
    }

   /* void LaunchProjectileRoutine(ShootingScript shooter, Vector3 target)
    {
        
    }*/

    public void DoShot(ShootingScript shooter, Vector3 target, GameObject _PROJECTILE)
    {
        GameObject projectile = Instantiate(_PROJECTILE) as GameObject;
        projectile.transform.position = shooter.ProjectileLauncher.position;
        projectile.transform.LookAt(target + new Vector3(0, 0.5f, 0));
        projectile.transform.Rotate(Vector3.left, 45, Space.Self);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * shooter.PROJECTILE_SPEED;
        shooter.lastShotProjectile = projectile.GetComponent<ProjectileScript>();
        projectile.GetComponent<ProjectileScript>().speed = shooter.PROJECTILE_SPEED;
        projectile.GetComponent<ProjectileScript>().Target = target;
        projectile.GetComponent<ProjectileScript>().myShooter = shooter;
    }
   /* IEnumerator LaunchProjectileRoutine(ShootingScript shooter, Vector3 target, GameObject _projectile, int shots)
    {
        shooter.GetComponentInChildren<AnimController>().Shoot();
        yield return new WaitForSeconds(shooter.DELAY);
        for (int i = 0; i < shots; i++)
        {
            GameObject projectile = Instantiate(_projectile) as GameObject;
            projectile.transform.position = shooter.ProjectileLauncher.position;
            projectile.transform.LookAt(target + new Vector3(0, 0.5f, 0));
            projectile.transform.Rotate(Vector3.left, 45, Space.Self);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * shooter.PROJECTILE_SPEED;
            shooter.lastShotProjectile = projectile.GetComponent<ProjectileScript>();
            projectile.GetComponent<ProjectileScript>().speed = shooter.PROJECTILE_SPEED;
            projectile.GetComponent<ProjectileScript>().Target = target;
            projectile.GetComponent<ProjectileScript>().myShooter = shooter;
            yield return new WaitForSeconds(shooter.TIME_BETWEEN_SHOTS);
        }
    }*/

    public void MakeAIWait(float time)
    {
        StartCoroutine(ActionOnCooldownRoutine(time));
    }

    IEnumerator ActionOnCooldownRoutine(float time)
    {
        waitRoutines++;
        AI_Controller.actionCooldown = true;
        yield return new WaitForSeconds(time);
        waitRoutines--;
        if (waitRoutines == 0)
        {
            AI_Controller.actionCooldown = false;
        }
    }

    public void SendCommandToReduceDefence(UnitScript target, int value)
    {
        int posX = Mathf.RoundToInt(target.transform.position.x);
        int posZ = Mathf.RoundToInt(target.transform.position.z);
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC(
                "RPCReduceDefence", PhotonTargets.All,
                posX, 
                posZ, 
                value
                );
        }
        else
        {
            ReduceDefence(posX,posZ,value);
        }
    }


    [PunRPC]
    void RPCReduceDefence(int posX, int posZ, int value)
    {
        ReduceDefence(posX, posZ, value);   
    }
    void ReduceDefence(int posX, int posZ, int value)
    {
        UnitScript target = Map.Board[posX, posZ].myUnit;
        target.CurrentDefence -= value;
        target.DefenceReduction += value;
    }
}
