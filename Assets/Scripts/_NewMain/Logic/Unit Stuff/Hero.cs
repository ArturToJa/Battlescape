using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattlescapeLogic
{
    public class Hero : Unit
    {      
        [SerializeField] Sprite _avatarHighlightedTransparent;
        public Sprite avatarHighlightedTransparent
        {
            get
            {
                return _avatarHighlightedTransparent;
            }
            private set
            {
                _avatarHighlightedTransparent = value;
            }
        }

        [SerializeField] Sprite _avatarTransparent;
        public Sprite avatarTransparent
        {
            get
            {
                return _avatarTransparent;
            }
            private set
            {
                _avatarTransparent = value;
            }
        }

        public string heroName { get; set; }

        public static event Action<Hero> OnHeroDeath = delegate { };

        public override void Die(Unit killer)
        {
            base.Die(killer);
            GetMyOwner().Lose();
            OnHeroDeath(this);
        }
    }


}
