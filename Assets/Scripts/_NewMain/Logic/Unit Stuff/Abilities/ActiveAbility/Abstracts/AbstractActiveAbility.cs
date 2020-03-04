using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveAbility : AbstractAbility
    {
        static AbstractActiveAbility _currentlyUsedAbility;
        public static AbstractActiveAbility currentlyUsedAbility
        {
            get
            {
                return _currentlyUsedAbility;
            }
            set
            {
                _currentlyUsedAbility = value;
                OnCurrentlyUsedAbilityChanged();
            }
        }

        public IMouseTargetable target { get; set; }

        [Header("Cost statistics")]        

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


        public static event System.Action OnCurrentlyUsedAbilityChanged = delegate { };

        protected abstract bool IsUsableNow();

        public bool IsUsableNowBase()
        {
            return IsUsableNow() && (usesLeft > 0 || usesPerBattle == 0) && owner.statistics.currentEnergy >= energyCost && roundsTillOffCooldown == 0;
        }

        public abstract bool IsLegalTarget(IMouseTargetable target);


        public void OnClickIcon()
        {
            currentlyUsedAbility = this;
            ColourTargets();
        }

        public abstract void ColourTargets();

        //NO IDEA if we even need this - in old code it coloured e.g. possible targets when hovering over ability, especially if ability is no target (used on click and not on activation
        public virtual void OnMouseHovered()
        {
            return;
        }
        

        public void BaseActivate()
        {
            owner.statistics.currentEnergy -= energyCost;
            if (usesPerBattle > 0)
            {
                usesLeft--;
            }
            roundsTillOffCooldown = cooldown;
            Activate();
            currentlyUsedAbility = null;
        }

        public abstract void Activate();

        public void Cancel()
        {
            currentlyUsedAbility = null;
        }

        public override void OnNewRound()
        {
            base.OnNewRound();
            if (roundsTillOffCooldown > 0)
            {
                roundsTillOffCooldown--;
            }
        }
    }
}