﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeSound;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveAbility : AbstractAbility, IActiveEntity
    {
        [SerializeField] string _abilityName;
        public string abilityName
        {
            get
            {
                return _abilityName;
            }
            protected set
            {
                _abilityName = value;
            }
        }

        [SerializeField] string _description;
        public string description
        {
            get
            {
                return _description;
            }
            protected set
            {
                _description = value;
            }
        }

        [SerializeField] Sprite _icon;
        public Sprite icon
        {
            get
            {
                return _icon;
            }
            protected set
            {
                _icon = value;
            }
        }        

        public IMouseTargetable target { get; set; }

        [Header("Limitations")]

        [SerializeField]
        protected TurnPhases legalPhases = TurnPhases.All;

        [SerializeField]
        bool costsMovement;

        [SerializeField] int _cooldown = 1;
        public int cooldown
        {
            get
            {
                return _cooldown;
            }
            protected set
            {
                _cooldown = value;
            }
        }
        int roundsTillOffCooldown = 0;



        [SerializeField] int _energyCost;

        public virtual void OnAnimationEvent()
        {
            return;
        }

        public int energyCost
        {
            get
            {
                return _energyCost;
            }
            protected set
            {
                _energyCost = value;
            }
        }

        [SerializeField] int _usesPerBattle;
        public int usesPerBattle
        {
            get
            {
                return _usesPerBattle;
            }
            protected set
            {
                _usesPerBattle = value;
            }
        }

        int _usesLeft = -1;
        public int usesLeft
        {
            get
            {
                if (_usesLeft == -1)
                {
                    _usesLeft = usesPerBattle;
                }
                return _usesLeft;
            }
            protected set
            {
                _usesLeft = value;
            }
        }

        [Header("Visuals and Sound")]
        [Space]
        [SerializeField]
        string _animationTrigger;
        public string animationTrigger
        {
            get
            {
                return _animationTrigger;
            }
            private set
            {
                _animationTrigger = value;
            }
        }
        [SerializeField] Sound sound;
        [SerializeField] string log;
        [SerializeField] GameObject castVisualEffect;
        [SerializeField] protected Color targetColouringColour;


        public static event Action OnAbilityClicked = delegate { };

        public static event Action OnAbilityFinished = delegate { };



        public virtual bool IsUsableNow()
        {
            return CheckMovementCost() && HasUsesLeft() && HasEnoughEnergy() && IsOffCooldown() && IsCorrectPhase();
        }

        protected bool CheckMovementCost()
        {
            return costsMovement == false || (owner.statistics.movementPoints >= owner.statistics.GetCurrentMaxMovementPoints());
        }

        protected bool IsOffCooldown()
        {
            return roundsTillOffCooldown == 0;
        }

        protected bool HasEnoughEnergy()
        {
            return owner.statistics.currentEnergy >= energyCost;
        }

        protected bool HasUsesLeft()
        {
            return (usesLeft > 0 || usesPerBattle == 0);
        }

        protected bool IsCorrectPhase()
        {
            switch (legalPhases)
            {
                case TurnPhases.None:
                    Debug.LogWarning("Legal phases for: " + abilityName + " is set to none; it makes no sense, consider changing it");
                    return false;
                case TurnPhases.All:
                    return true;
                default:
                    return legalPhases == GameRound.instance.currentPhase;
            }
        }






        public virtual void OnClickIcon()
        {
            Global.instance.currentEntity = this;
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            ColourPossibleTargets();
            OnAbilityClicked();
        }

        public abstract void ColourPossibleTargets();

        //NO IDEA if we even need this - in old code it coloured e.g. possible targets when hovering over ability, especially if ability is no target (used on click and not on activation
        public virtual void OnMouseHovered()
        {
            return;
        }
        
        public abstract bool IsLegalTarget(IMouseTargetable target);


        protected virtual void Activate()
        {
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            owner.statistics.currentEnergy -= energyCost;
            if (usesPerBattle > 0)
            {
                usesLeft--;
            }
            if (costsMovement)
            {
                owner.statistics.movementPoints = 0;
            }
            roundsTillOffCooldown = cooldown;
            LogConsole.instance.SpawnLog(log);
            Animate();
            DoVisualEffectFor(castVisualEffect, owner.gameObject);
            BattlescapeSound.SoundManager.instance.PlaySound(owner.gameObject, sound);
            OnFinish();
            //the thing below is necessary, but no idea WHEN and WHERE to turn it off.
            //PlayerInput.instance.isInputBlocked = true;
        }

        public void OnFinish()
        {
            owner.GetMyOwner().SelectUnit(owner);
            OnAbilityFinished();
        }

        public override void OnNewRound()
        {
            base.OnNewRound();
            if (roundsTillOffCooldown > 0)
            {
                roundsTillOffCooldown--;
            }
        }

        protected void Animate()
        {
            if (string.IsNullOrEmpty(animationTrigger) == false)
            {
                owner.animator.SetTrigger(animationTrigger);
            }            
        }

        protected void DoVisualEffectFor(GameObject vfx, GameObject target)
        {
            if (vfx != null)
            {
                Instantiate(vfx, target.transform.position, vfx.transform.rotation,target.transform);
            }
        }

        public void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (IsLegalTarget(clickedObject))
            {
                target = clickedObject;
                Activate();
            }
        }

        public void OnRightClick(IMouseTargetable target)
        {
            OnFinish();
        }

        public virtual void OnCursorOver(IMouseTargetable target, Vector3 exactMousePosition)
        {
            if (IsLegalTarget(target))
            {
                Cursor.instance.OnLegalTargetHovered();
            }
            else
            {
                Cursor.instance.OnInvalidTargetHovered();
            }
        }

        public void OnAbilityOver()
        {
            PlayerInput.instance.isInputBlocked = false;
        }
        protected void Update()
        {
            //if(abilityIsActive)
            //{OnUpdate();
        }
    }
}