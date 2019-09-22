using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitMovement : MonoBehaviour
{
    [SerializeField] bool disableStrangeFlipping;
    public Transform myBody;
    Vector3 Destination;
    protected float SMOOTH_DISTANCE = 0.3f;
    [SerializeField] float visualMovementSpeed = 2.0f;
    public float VISUAL_MOVEMENT_SPEED { get; private set; }
    public Sound[] StepSounds;
    AudioSource stepSource;
    UnitScript myUnitScript;
    BodyTrigger bodyTrigger;

    public bool CanMove = true;    

    int CurrMoveSpeed;
    [SerializeField] int baseMoveSpeed;


    [HideInInspector] public bool isMoving;

    protected virtual void Start()
    {
        bodyTrigger = GetComponentInChildren<BodyTrigger>();
        myUnitScript = GetComponent<UnitScript>();
        CombatController.Instance.AttackEvent += OnAttack;
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        stepSource = gameObject.AddComponent<AudioSource>();
        CurrMoveSpeed = baseMoveSpeed;
        Destination = transform.position;
        VISUAL_MOVEMENT_SPEED = visualMovementSpeed * PlayerPrefs.GetFloat("moveSpeedAnimation");

        
    }

    public void OnAttack(UnitScript Attacker, UnitScript Defender, int damage)
    {
        if (Attacker == myUnitScript)
        {
            LookAtTheTarget(Defender.transform.position, bodyTrigger.RotationInAttack);
        }
    }

    public void OnNewTurn()
    {
        if (TurnManager.Instance.TurnCount <= 1)
        {
            return;
        }
        CanMove = true;
    }
    public void Step()
    {
        PlayRandomStep();
    }

    void PlayRandomStep()
    {
        Sound s = StepSounds[Random.Range(0, StepSounds.Length)];
        s.oldSource = stepSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    protected virtual void Update()
    {
        if (myBody == null)
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "Body" && child.gameObject.activeSelf)
                {
                    myBody = child;
                }
            }
        }
        Move();
       // CheckIfCanMove();
    }
    public int GetCurrentMoveSpeed(bool needRealValue)
    {
        if (needRealValue ||(GetComponent<UnitScript>().EnemyList == null || GetComponent<UnitScript>().EnemyList.Count == 0))
        {
            return CurrMoveSpeed;
        }
        else
        {
            return 1;
        }
    }

    public void IncrimentMoveSpeedBy(int speed)
    {
        CurrMoveSpeed += speed;
    }
    

   /* private void CheckIfCanMove()
    {
        if (this.GetComponent<UnitScript>().CheckIfIsInCombat())
        {
            canMove = false;
        }
    }*/

    public bool HasStoppedMoving()
    {
        if (Vector3.Distance(Destination, transform.position) < 0.5f * SMOOTH_DISTANCE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, Destination) > 0.2f)
        {
            LookAtTheTarget(Destination,0);
        }
        this.transform.Translate((Destination - transform.position) * Time.deltaTime * VISUAL_MOVEMENT_SPEED, Space.World);
    }

    public void SetDestination(Vector3 destination)
    {
        Destination = destination;
    }

    public Vector3 GetDestination()
    {
        return Destination;
    }

    public float GetSmoothDistance()
    { return SMOOTH_DISTANCE; }

    public void LookAtTheTarget(Vector3 target, float rotate)
    {
        if (disableStrangeFlipping)
        {
            myBody.LookAt(new Vector3(target.x, myBody.transform.position.y, target.z));
        }
        else
        {
            myBody.LookAt(new Vector3(target.x, transform.position.y, target.z));
        }
        myBody.Rotate(new Vector3(0, rotate, 0));

    }

    public int GetBaseMS()
    {
        return baseMoveSpeed;
    }

    public void TeleportTo(Tile tile)
    {
        transform.position = tile.transform.position;
        UnitScript myUnit = GetComponent<UnitScript>();
        tile.OnUnitEnterTile(myUnit);
        Destination = tile.transform.position;      
    }
    public bool IsAbleToMove()
    {

        return (CanMove && GetComponent<UnitScript>().IsFrozen == false);
    }
}
