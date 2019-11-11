using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AnimController : MonoBehaviour
{
    // NOTE - currently ALL actions in shooting phase are considered NOT to be melee attacks and will therefore NOT send melee attack animation to opponents computer. 
    // If we ever need to make any melee attacks in shooting phase, we need to change this!



    public Animator MyAnimator;
    BattlescapeLogic.Unit myUnit;
    [SerializeField] GameObject sword;
    [HideInInspector] public bool isHit;

    void Start()
    {
        if (MyAnimator == null)
        {
            MyAnimator = GetComponentInChildren<Animator>();
        }
        myUnit = GetComponent<BattlescapeLogic.Unit>();
        CombatController.Instance.AttackEvent += OnAttack;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Sword")
            {
                if (sword == null)
                {
                    sword = child.gameObject;
                }
                child.gameObject.SetActive(false);
                child.gameObject.name = "theSword";
                //                Debug.Log(child.gameObject.name);
            }
        }
    }


    private void Update()
    {
        SetInCombat(myUnit.IsInCombat());
        //AnimateWalking(US.movement.isMoving);
        if (isHit)
        {
            isHit = false;
            AnimateWound();
        }
    }
    public void OnAttack(BattlescapeLogic.Unit Attacker, BattlescapeLogic.Unit Defender)
    {
        if (Attacker != myUnit)
        {
            return;
        }
        if (Attacker.IsRanged())
        {
            if (sword != null)
            {
                StartCoroutine(ShowSword());
            }
            AnimateAttack();
        }
    }

    public void ArmPrimaryWeapon()
    {
        MyAnimator.SetTrigger("ArmPrimary");
    }

    public void ArmSecondaryWeapon()
    {
        MyAnimator.SetTrigger("ArmSecondary");
    }

    public void SetRanged(bool isRanged)
    {
        MyAnimator.SetBool("IsRanged", isRanged);
    }

    public void AnimateWalking(bool boool)
    {
        MyAnimator.SetBool("Walking", boool);
    }

    public void AnimateAttack()
    {
        MyAnimator.SetTrigger("Attack");
    }
    public void SetJumping(bool b)
    {
        MyAnimator.SetBool("IsJumping", b);
    }


    public void SpecialAttack()
    {
        MyAnimator.SetTrigger("SpecialAttack");
    }

    public void AnimateDeath()
    {
        MyAnimator.SetTrigger("Death");
    }

    public void AnimateWound()
    {
        MyAnimator.SetTrigger("Wound");
    }

    public void SetInCombat(bool boool)
    {
        MyAnimator.SetBool("InCombat", boool);
    }

    public void SetEating(bool isEating)
    {
        MyAnimator.SetBool("IsEating", isEating);
    }

    public void Shoot()
    {
        MyAnimator.SetTrigger("Shooting");
    }
    public void Cast()
    {
        MyAnimator.SetTrigger("Cast");
    }

    public IEnumerator ShowSword()
    {
        if (myUnit is Hero)
        {
            //myUnit.ToggleWeapon(hero.secondaryWeaponInHand, hero.secondaryWeaponHidden);
        }
        else
        {
            sword.SetActive(true);
        }
        yield return new WaitForSeconds(0.8f);
        if (myUnit is Hero)
        {
            //myUnit.ToggleWeapon(hero.secondaryWeaponHidden, hero.secondaryWeaponInHand);
        }
        else
        {
            sword.SetActive(false);
        }
    }
}
