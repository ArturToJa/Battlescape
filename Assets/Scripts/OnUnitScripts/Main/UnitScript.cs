using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[RequireComponent(typeof(UnitMovement))]
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
    bool isAlive = true;


    [Header("Statistics")]
    [SerializeField]
    int _maxHP;
    public int MaxHP
    {
        get
        {
            return _maxHP;
        }
    }
    public int CurrentHP { get; private set; }
    public int DiceNumber;
    [SerializeField] int BaseDamage = 10;
    public int CurrentDamage { get; set; }
    [SerializeField] int Attack;
    int _currAttack;
    public int CurrentAttack
    {
        get
        {
            if (GetComponent<ShootingScript>() != null && CheckIfIsInCombat() && GetComponent<ShootingScript>().doesNotLoseAttackInCombat == false)
            {
                return 0;
            }
            else
            {
                return _currAttack;
            }

        }
        set
        {
            _currAttack = value;
        }

    }
    [SerializeField] int Defence;
    public int CurrentDefence { get; set; }
    public int DefenceReduction { get; set; }

    public int Value;

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
    [HideInInspector] public bool hasAttacked = false;
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

    void Start()
    {
        if (isRealUnit)
        {
            TurnManager.Instance.NewTurnEvent += OnNewTurn;
        }
        BlocksHits = false;
        deathSource = gameObject.AddComponent<AudioSource>();
        hitSource = gameObject.AddComponent<AudioSource>();
        swingSource = gameObject.AddComponent<AudioSource>();
        CurrentHP = MaxHP;
        CurrentAttack = Attack;
        CurrentDefence = Defence;
        CurrentDamage = BaseDamage;
        QuitCombatPercent = baseQuitCombatPercent;
        DoesRetaliate = DoesRetalByDefault;
        if (isRealUnit == false)
        {
            return;
        }
        int myX = Mathf.RoundToInt(transform.position.x);
        int myZ = Mathf.RoundToInt(transform.position.z);
        myTile = Map.Board[myX, myZ];
    }

    public void OnNewTurn()
    {
        if (TurnManager.Instance.TurnCount <= 1)
        {
            return;
        }

        hasAttacked = false;
        hasJustArrivedToCombat = false;
        HasRetaliatedThisTurn = false;

        if (!CheckIfIsInCombat())
        {
            RegenerateDefenceOutOfCombat();
        }

        if (PoisonCounter > 0)
        {
            PoisonEffect();
        }
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
            CheckIfIsAlive();

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

    public bool CanAttackDestructible(DestructibleScript target, bool showText)
    {
        if (CheckIfIsInCombat() && Input.GetMouseButtonDown(0) && showText)
        {
            PopupTextController.AddPopupText("Cannot destroy objects while in combat!", PopupTypes.Info);
        }
        return (StandsNextToDestructible(target) && hasAttacked == false && CheckIfIsInCombat() == false && TurnManager.Instance.CurrentPhase == TurnPhases.Attack);
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
    public event Action DeathEvent;
    void CheckIfIsAlive()
    {
        if (CurrentHP <= 0 && isAlive)
        {
            //this needs to happen BEFORE the DeathEvent or we need to refactor trhe whole wthing into like ONE? XD
            myTile.SetMyUnitTo(null);
            if (DeathEvent != null)
            {
                DeathEvent();
            }
            StartCoroutine(DeathCoroutine());

        }
    }
    IEnumerator DeathCoroutine()
    {
        isAlive = false;
        if (MouseManager.Instance.SelectedUnit == this)
        {
            MouseManager.Instance.Deselect();
        }

        Player.Players[OpponentID].PlayerScore += Value;

        if (GetComponent<HeroScript>() != null)
        {
            Player.Players[OpponentID].HasWon = true;
            VictoryLossChecker.isAHeroDead = true;
        }
        GetComponentInChildren<AnimController>().AnimateDeath();
        yield return new WaitForSeconds(3f);
        transform.position = new Vector3(100, 100, 100);
        gameObject.SetActive(false);

    }
    public bool DealDamage(int damage, bool gotHit, bool isPoisoned, bool isShot)
    {

        CurrentHP -= damage;
        if (gotHit)
        {
            if (!isShot)
            {
                GetComponentInChildren<AnimController>().AnimateWound();
            }
            if (isPoisoned)
            {
                GetPoisoned(1);
            }
        }
        if (damage > 0)
        {
            CurrentDefence += DefenceReduction;
            DefenceReduction = 0;
        }
        return CurrentHP <= 0;
    }

    public void Heal(int value, bool canOverheal)
    {
        //maybe in the future ill need to do stuffo n every heal idk better safe than sorry
        CurrentHP += value;
        if (CurrentHP > MaxHP && canOverheal == false)
        {
            CurrentHP = MaxHP;
        }
    }

    public void RegenerateDefenceOutOfCombat()
    {
        if (CurrentDefence < Defence && DefenceReduction > 0)
        {
            CurrentDefence++;
            DefenceReduction--;
            PopupTextController.AddPopupText("Regenerate!", PopupTypes.Stats);
            Log.SpawnLog(name + " regenerates 1 point of defence.");
        }
    }

    public void GetPoisoned(int value)
    {
        PoisonCounter += value;
        PopupTextController.AddPopupText("Poison!!", PopupTypes.Info);
        Log.SpawnLog(name + " gets poisoned!");
    }

    public void PoisonEffect()
    {
        GetComponentInChildren<AnimController>().AnimateWound();
        PopupTextController.AddPopupText("Poison!!", PopupTypes.Info);
        Log.SpawnLog("Poison affects " + name + "!");
        if (PoisonCounter >= 4)
        {
            CurrentHP--;
            Log.SpawnLog(name + " gets 1 point of damage from poison!");
        }

        CurrentDefence--;
        PoisonCounter--;
    }

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

    public int GetBaseAttack()
    {
        return Attack;
    }
    public int GetBaseDefence()
    {
        return Defence;
    }
    public int GetBaseMS()
    {
        return GetComponent<UnitMovement>().GetBaseMS();
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
}

public enum UnitType
{
    Null, Cannonmeat, Shooters, Infantry, Cavalry, Creatures
}

