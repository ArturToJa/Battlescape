using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AnimController))]
[DisallowMultipleComponent]
public class UnitScript : MonoBehaviour
{
    [Header("Major Info")]
    public bool isPlayerOne;
    public int PlayerID
    {
        get
        {
            if (isPlayerOne)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
    public int OpponentID
    {
        get
        {
            if (isPlayerOne)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public UnitType unitType;
    public Unit unitUnit;
    public bool isRealUnit;

    public Animator animator;
    public Statistics statistics;
    public AbstractMovement newMovement { get; private set; }
    public GameObject visuals {get; set;}

    public int baseQuitCombatPercent = 50;
    public int QuitCombatPercent { get; set; }

    [Header("Types")]
    public bool isColossal = false;
    public bool isRanged = false;
    public bool isUndead = false;
    public bool isSpecial = false;
    public bool isConstruct = false;
    public bool isSummonned = false;

    [Header("Skills")]
    public bool isPoisonous = false;
    public bool isProtector = false;
    public bool isStoppingRetaliation = false;
    [SerializeField] bool canAttack = true;
    public bool CanAttack
    {
        get
        {
            return canAttack && !IsFrozen;
        }
    }
    [SerializeField] bool _DoesRetaliateByDefault;
    public bool DoesRetalByDefault
    {
        get
        {
            return _DoesRetaliateByDefault;
        }
    }
    public bool DoesRetaliate { get; set; }
    public Tile myTile;

    [HideInInspector] public bool isQuittingCombat = false;
    [HideInInspector] public bool hasJustArrivedToCombat = false;
    public bool CanStillAttack()
    {
        return (statistics.numberOfAttacks > 0);
    }
    public bool CanStillMove()
    {
        return statistics.movementPoints > 0;
    }

    [HideInInspector] public bool HasRetaliatedThisTurn = false;
    public bool CanCurrentlyRetaliate
    {
        get
        {
            return DoesRetaliate && HasRetaliatedThisTurn == false && IsFrozen == false;
        }
    }
    [HideInInspector] public bool IsFrozen = false;
    public bool BlocksHits { get; private set; }
    public int PoisonCounter { get; private set; }
    public List<UnitScript> EnemyList
    {
        get
        {
            List<UnitScript> enemies = new List<UnitScript>();
            if (myTile != null)
            {
                List<Tile> neighbours = myTile.neighbours;
                foreach (Tile tile in neighbours)
                {
                    if (tile.myUnit != null)
                    {
                        if (tile.myUnit.PlayerID != this.PlayerID)
                        {
                            enemies.Add(tile.myUnit);
                        }
                    }
                }
            }
            return enemies;
        }
    }

    bool isNameSet = false;
    public List<UnitScript> AllyList
    {
        get
        {
            List<UnitScript> allies = new List<UnitScript>();
            if (myTile != null)
            {
                List<Tile> neighbours = myTile.neighbours;
                foreach (Tile tile in neighbours)
                {
                    if (tile.myUnit != null)
                    {
                        if (tile.myUnit.PlayerID == this.PlayerID)
                        {
                            allies.Add(tile.myUnit);
                        }
                    }
                }
            }
            return allies;
        }
    }

    public Sound[] HitSounds;
    AudioSource hitSource;

    public Sound[] SwingSounds;
    AudioSource swingSource;

    public Sound[] deathSounds;
    AudioSource deathSource;

    public Sound[] StepSounds;
    AudioSource stepSource;

    void Start()
    {
        statistics.healthPoints = statistics.maxHealthPoints;
        statistics.movementPoints = statistics.baseMaxMovementPoints;
        if (isRealUnit)
        {
            TurnManager.Instance.NewTurnEvent += OnNewTurn;
        }
        BlocksHits = false;
        deathSource = gameObject.AddComponent<AudioSource>();
        hitSource = gameObject.AddComponent<AudioSource>();
        swingSource = gameObject.AddComponent<AudioSource>();
        stepSource = gameObject.AddComponent<AudioSource>();
        QuitCombatPercent = baseQuitCombatPercent;
        DoesRetaliate = DoesRetalByDefault;
        if (isRealUnit == false)
        {
            return;
        }
        int myX = Mathf.RoundToInt(transform.position.x);
        int myZ = Mathf.RoundToInt(transform.position.z);
        myTile = Map.Board[myX, myZ];
        visuals = Helper.FindChildWithTag(gameObject, "Body");
        newMovement = Global.instance.movementTypes[(int)MovementTypes.Ground];       
    }

    public void OnNewTurn()
    {
        if (TurnManager.Instance.TurnCount <= 1)
        {
            return;
        }
        statistics.movementPoints = statistics.GetCurrentMaxMovementPoints();        
        statistics.numberOfAttacks = statistics.maxNumberOfAttacks;
        hasJustArrivedToCombat = false;
        HasRetaliatedThisTurn = false;

        //if (!CheckIfIsInCombat())
        //{
        //    RegenerateDefenceOutOfCombat();
        //}
        }

    void SetName()
    {
        this.gameObject.name = unitUnit.Name;
        if (this.GetComponent<HeroScript>() != null)
        {
            this.gameObject.name += " " + HeroNames.PlayerHeroNames[PlayerID];
        }
    }

    void Update()
    {
        if (isRealUnit)
        {
            CheckIfIsInCombat();
        }
        if (myTile != null && isNameSet == false)
        {
            SetName();
            isNameSet = true;
        }

    }

    bool StandsNextToDestructible(DestructibleScript target)
    {
        return (myTile.neighbours.Contains(target.transform.parent.GetComponent<Tile>()));
    }

    public void DeathSound()
    {
        Sound s = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
        s.oldSource = deathSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    public void Hit()
    {
        PlayRandomAttack();
    }

    void PlayRandomAttack()
    {
        Sound s = HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)];
        s.oldSource = hitSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }
    public void Swing()
    {
        PlayRandomSwing();
    }

    void PlayRandomSwing()
    {
        Sound s = SwingSounds[UnityEngine.Random.Range(0, SwingSounds.Length)];
        s.oldSource = swingSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }
    public void PlayRandomStep()
    {
        Sound s = StepSounds[UnityEngine.Random.Range(0, StepSounds.Length)];
        s.oldSource = stepSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    public bool CanAttackDestructible(DestructibleScript target, bool showText)
    {
        if (CheckIfIsInCombat() && Input.GetMouseButtonDown(0) && showText)
        {
            PopupTextController.AddPopupText("Cannot destroy objects while in combat!", PopupTypes.Info);
        }
        return (StandsNextToDestructible(target) && CanStillAttack() == true && CheckIfIsInCombat() == false && TurnManager.Instance.CurrentPhase == TurnPhases.Attack);
    }

    public bool CheckIfIsInCombat()
    {
        if (GameStateManager.Instance.IsItPreGame())
        {
            return false;
        }
        if (isQuittingCombat)
        {
            return false;
        }
        if (EnemyList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Die()
    {
        myTile.SetMyUnitTo(null);
        if (DeathEvent != null)
        {
            DeathEvent();
        }
        StartCoroutine(DeathCoroutine());
    }
    public event Action DeathEvent;    
    IEnumerator DeathCoroutine()
    {
        if (MouseManager.Instance.SelectedUnit == this)
        {
            MouseManager.Instance.Deselect();
        }

        Global.instance.playerTeams[OpponentID].players[0].AddPoints(statistics.cost);

        if (GetComponent<HeroScript>() != null)
        {
            if (OpponentID == 0)
            {
                VictoryLossChecker.gameResult = GameResult.GreenWon;
            }
            else
            {
                VictoryLossChecker.gameResult = GameResult.RedWon;
            }
            VictoryLossChecker.isAHeroDead = true;
        }
        GetComponentInChildren<AnimController>().AnimateDeath();
        yield return new WaitForSeconds(3f);
        transform.position = new Vector3(100, 100, 100);
        gameObject.SetActive(false);

    }
    public bool DealDamage(int damage, bool gotHit, bool isPoisoned, bool isShot)
    {

        statistics.healthPoints -= damage;
        if (gotHit)
        {
            if (!isShot)
            {
                GetComponentInChildren<AnimController>().AnimateWound();
            }           
        }
        if (damage > 0)
        {
            //add buff here. 
        }
        if (IsAlive() == false)
        {
            Die();
        }
        return IsAlive() == false;
    }   

    // This will be re-done with buffs. No need to keep it in old code, as its not key for the game to work, and can be done easily in new code.
    /*public void RegenerateDefenceOutOfCombat()
    {
        if (statistics.GetCurrentDefence() < Defence && DefenceReduction > 0)
        {
            statistics.GetCurrentDefence()++;
            DefenceReduction--;
            PopupTextController.AddPopupText("Regenerate!", PopupTypes.Stats);
            Log.SpawnLog(name + " regenerates 1 point of defence.");
        }
    }*/

    //HERE there was some poison code, but poison NEVER existed in PC-version of the game anyway. The Undead or any other 'poisonous 'units will come one day, but not now. ;)
    public int HowMuchShelteredFrom(Vector3 danger)
    {
        int shelter = 0;
        if (CheckIfIsInCombat())
        {
            return 0;
        }
        if (GetMyCovers() == null)
        {
            return 0;
        }
        Vector3 endPos = transform.position + Vector3.up * 0.25f;
        Vector3 startPos = danger + Vector3.up * 0.25f;
        Vector3 direction = endPos - startPos;
        foreach (RaycastHit hit in Physics.RaycastAll(startPos, direction, Vector3.Distance(startPos, endPos) + 0.01f))
        {
            if (hit.collider.GetComponent<Obstacle_Cover>() && GetMyCovers().Contains(hit.collider.GetComponent<Obstacle_Cover>()))
            {
                shelter += hit.collider.GetComponent<Obstacle_Cover>().CoverValue;
            }
        }
        return shelter;
    }

    List<Obstacle_Cover> GetMyCovers()
    {
        List<Obstacle_Cover> list = new List<Obstacle_Cover>();
        if (CheckIfIsInCombat())
        {
            Debug.Log("I am in combat so shouldnt get as far?");
            return null;
        }
        else
        {
            foreach (Tile tile in myTile.neighbours)
            {
                if (tile.GetComponentInChildren<Obstacle_Cover>() != null)
                {
                    list.Add(tile.GetComponentInChildren<Obstacle_Cover>());
                }
            }
        }
        return list;
    }

       
    public void BlockHitsForTurns(int turns)
    {
        StartCoroutine(BlockHitsForTurnsCoroutine(turns));
    }

    IEnumerator BlockHitsForTurnsCoroutine(int turns)
    {
        int oldTurn = TurnManager.Instance.TurnCount;
        while (turns > 0)
        {
            if (oldTurn != TurnManager.Instance.TurnCount)
            {
                turns--;
            }
            oldTurn = TurnManager.Instance.TurnCount;
            BlocksHits = true;
            yield return null;
        }
        BlocksHits = false;
    }

    public void Move(Tile newPosition)
    {
        if (CanStillMove())
        {
            newMovement.ApplyUnit(this);
            StartCoroutine(newMovement.MoveTo(newPosition));
        }
        else
        {
            Debug.LogError("why moving when cannot move?");
        }
    }

    public bool IsAlive()
    {
        return statistics.healthPoints > 0;
    }
}

public enum UnitType
{
    Null, Cannonmeat, Shooters, Infantry, Cavalry, Creatures
}

