using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public override void Die(Unit killer)
        {
            base.Die(killer);

            //lose game
            //for now:
            VictoryLossChecker.isAnyHeroDead = true;
            if (GetMyOwner() == Global.instance.playerTeams[0].players[0])
            {
                //we, Green player, lost
                VictoryLossChecker.gameResult = GameResult.RedWon;
            }
            else
            {
                VictoryLossChecker.gameResult = GameResult.GreenWon;
            }

        }
    }


}
