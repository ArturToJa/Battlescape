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
    public BattlescapeLogic.Unit attackingUnit;
    public BattlescapeLogic.Unit attackTarget;

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
        CheckForInput();
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
        if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.currentPosition.neighbours.Contains(MouseManager.Instance.MouseoveredUnit.currentPosition) && MouseManager.Instance.SelectedUnit.CanStillAttack() && MouseManager.Instance.SelectedUnit.attack != null)
        {
            SendCommandToAttack(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit);
            MouseManager.Instance.SelectedUnit.statistics.numberOfAttacks-- ;
        }
        if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit.currentPosition.neighbours.Contains(MouseManager.Instance.MouseoveredUnit.currentPosition) && MouseManager.Instance.SelectedUnit.CanStillAttack() && MouseManager.Instance.SelectedUnit.attack != null)
        {
            PopupTextController.AddPopupText("This unit cannot attack!", PopupTypes.Info);
        }
        if (Input.GetMouseButtonDown(0) && PossibleToShootAt(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit, false) && WeDontUseAbilityRightNow())
        {
            SendCommandToAttack(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit);
            MouseManager.Instance.SelectedUnit.statistics.numberOfAttacks = 0;
        }
        //if (GameStateManager.Instance.GameState == GameStates.AttackState && Input.GetMouseButtonDown(0) && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.mouseoveredDestructible != null && MouseManager.Instance.SelectedUnit.CanAttackDestructible(MouseManager.Instance.mouseoveredDestructible.GetComponent<DestructibleScript>(), true))
        //{
        //    if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        //    {
        //        photonView.RPC
        //            (
        //            "RPCMeleeAttackObstacle",
        //            PhotonTargets.All,
        //            Mathf.RoundToInt(MouseManager.Instance.SelectedUnit.transform.position.x),
        //            Mathf.RoundToInt(MouseManager.Instance.SelectedUnit.transform.position.z),
        //            Mathf.RoundToInt(MouseManager.Instance.mouseoveredDestructible.transform.position.x),
        //            Mathf.RoundToInt(MouseManager.Instance.mouseoveredDestructible.transform.position.z)
        //           );

        //    }
        //    else
        //    {
        //        MeleeAttackObstacle(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredDestructible.GetComponent<DestructibleScript>());
        //    }

        //}
    }

    bool WeDontUseAbilityRightNow()
    {
        return (Ability_Basic.currentlyUsedAbility == null || (Ability_Basic.currentlyUsedAbility != null && Ability_Basic.currentlyUsedAbility.gameObject != this.gameObject));
    }

    public bool PossibleToShootAt(Unit shooter, Unit target, bool isCursor)
    {
        if (Application.isEditor && Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log(!shooter.IsInCombat() + "," + target != null + "," + WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit, MouseManager.Instance.SelectedUnit.transform.position, target.transform.position) + "," + shooter.CanStillAttack());
        }
        return (
                  MouseManager.Instance.SelectedUnit != null
                  && MouseManager.Instance.SelectedUnit == shooter
                  && !shooter.IsInCombat()
                  && target != null
                  && target.owner != MouseManager.Instance.SelectedUnit.owner
                  && WouldItBePossibleToShoot(shooter, shooter.transform.position, target.transform.position)
                  && shooter.CanStillAttack()
                  //&& shooter.attack is ShootingAttack
                  && GameStateManager.Instance.GameState == GameStates.AttackState
                  && shooter.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0]
                  && IsInLineOfSight(transform.position, target.transform.position, isCursor)
               );
    }

    public bool IsInLineOfSight(Vector3 start, Vector3 target, bool showNoPopup)
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

    public bool WouldItBePossibleToShoot(BattlescapeLogic.Unit shooter, Vector3 start, Vector3 target)
    {

        Bounds FullRange = new Bounds(start, new Vector3(2 * shooter.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * shooter.statistics.GetCurrentAttackRange() + 0.25f));
        if (shooter.statistics.minimalAttackRange > 0)
        {
            Bounds miniRange = new Bounds(start, new Vector3(2 * shooter.statistics.minimalAttackRange + 0.25f, 5, 2 * shooter.statistics.minimalAttackRange + 0.25f));
            return miniRange.Contains(target) == false && FullRange.Contains(target) && IsInLineOfSight(start, target, true);
        }
        else
        {
            return FullRange.Contains(target) && IsInLineOfSight(start, target, true);
        }

    }


    void MeleeAttackObstacle(BattlescapeLogic.Unit Attacker, DestructibleScript target)
    {
        Debug.Log("Melee attacking obstacle - debugging fencer bug");
        //Attacker.LookAtTheTarget(target.transform.position, Attacker.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        Attacker.GetComponentInChildren<AnimController>().AnimateAttack();
        target.GetDamaged(Attacker.statistics.GetCurrentAttack() + 1);
        Attacker.statistics.numberOfAttacks = 0;
        CheckIfLastAttacker();
        target.GetComponentInChildren<ObjectHP>().SwitchTrigger();
    }

    [PunRPC]
    void RPCMeleeAttackObstacle(int unitX, int unitZ, int targetX, int targetZ)
    {
        BattlescapeLogic.Unit Attacker = Map.Board[unitX, unitZ].myUnit;
        DestructibleScript target = Map.Board[targetX, targetZ].GetComponentInChildren<DestructibleScript>();
        MeleeAttackObstacle(Attacker, target);
    }

    //public IEnumerator ShootToObstacle(BattlescapeLogic.Unit shooter, Vector3 target)
    //{

    //    SendCommandToLaunchProjectile(shooter, target);        

    //    yield return new WaitForSeconds((Vector3.Distance(target, shooter.ProjectileLauncher.position) * Mathf.PI) / (2 * Mathf.Sqrt(2)) / shooter.PROJECTILE_SPEED);
    //    DestroyObstacle(Map.Board[Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z)]);
    //    if (Application.isEditor)
    //    {
    //        DestroyImmediate(shooter.lastShotProjectile.gameObject);
    //    }
    //    else
    //    {
    //        Destroy(shooter.lastShotProjectile.gameObject);
    //    }

    //}

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

    public void SendCommandToAttack(Unit Attacker, Unit Defender)
    {
        GameStateManager.Instance.Animate();
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC(
                "RPCAttack", PhotonTargets.All,
                Mathf.RoundToInt(Attacker.transform.position.x),
                Mathf.RoundToInt(Attacker.transform.position.z),
                Mathf.RoundToInt(Defender.transform.position.x),
                Mathf.RoundToInt(Defender.transform.position.z));
        }
        else
        {
            Attack(Attacker, Defender);
        }

    }

    [PunRPC]
    void RPCAttack(int AttackerX, int AttackerZ, int DefenderX, int DefenderZ)
    {
        Unit Attacker = Map.Board[AttackerX, AttackerZ].myUnit;
        Unit Defender = Map.Board[DefenderX, DefenderZ].myUnit;
        Attack(Attacker, Defender);
    }

    public event Action<Unit, Unit> AttackEvent;

    public void Attack(Unit Attacker, Unit Defender)
    {
        //this is juct copy/pasted from NewMovement system but i didnt knwo if i can make it public there (TurnTowards function).
        Vector3 vector3 = new Vector3(Defender.transform.position.x, Attacker.visuals.transform.position.y, Defender.transform.position.z);
        Attacker.transform.LookAt(vector3);
        Attacker.visuals.transform.LookAt(vector3);
        if (AttackEvent != null)
        {
            AttackEvent(Attacker, Defender);
        }
        attackingUnit = Attacker;
        attackTarget = Defender;
        Attacker.Attack(Defender);
        CheckIfLastAttacker();
    }

    public static bool CheckIfLastAttacker()
    {
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
        {
            foreach (BattlescapeLogic.Unit ally in Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerUnits)
            {
                if (ally.CanStillAttack() == true)
                {
                    return false;
                }
            }
            PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            return true;
        }
        return false;
    }

    public void SendCommandToRetaliate()
    {
        //note this Attack command is already networked therefore it does not need to be networked a second time inside this command we gonna send here
        SendCommandToAttack(attackTarget, attackingUnit);
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
        Log.SpawnLog(attackingUnit.name + " strikes back!");
        attackingUnit.statistics.numberOfRetaliations--;
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
        attackTarget = null;
    }    

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

    public void SendCommandToReduceDefence(BattlescapeLogic.Unit target, int value)
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
            ReduceDefence(posX, posZ, value);
        }
    }


    [PunRPC]
    void RPCReduceDefence(int posX, int posZ, int value)
    {
        ReduceDefence(posX, posZ, value);
    }
    void ReduceDefence(int posX, int posZ, int value)
    {
        BattlescapeLogic.Unit target = Map.Board[posX, posZ].myUnit;
        target.statistics.bonusDefence -= value;
        //target.DefenceReduction += value;
    }

    public int HowMuchShelteredFrom(BattlescapeLogic.Unit unit, Vector3 danger)
    {
        int shelter = 0;
        if (unit.IsInCombat())
        {
            return 0;
        }
        if (GetCoversOf(unit) == null)
        {
            return 0;
        }
        Vector3 endPos = transform.position + Vector3.up * 0.25f;
        Vector3 startPos = danger + Vector3.up * 0.25f;
        Vector3 direction = endPos - startPos;
        foreach (RaycastHit hit in Physics.RaycastAll(startPos, direction, Vector3.Distance(startPos, endPos) + 0.01f))
        {
            if (hit.collider.GetComponent<Obstacle_Cover>() && GetCoversOf(unit).Contains(hit.collider.GetComponent<Obstacle_Cover>()))
            {
                shelter += hit.collider.GetComponent<Obstacle_Cover>().CoverValue;
            }
        }
        return shelter;
    }
    List<Obstacle_Cover> GetCoversOf(BattlescapeLogic.Unit unit)
    {
        List<Obstacle_Cover> list = new List<Obstacle_Cover>();

        foreach (Tile tile in unit.currentPosition.neighbours)
        {
            if (tile.GetComponentInChildren<Obstacle_Cover>() != null)
            {
                list.Add(tile.GetComponentInChildren<Obstacle_Cover>());
            }
        }
        return list;
    }
}
