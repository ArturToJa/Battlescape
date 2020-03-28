﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Hero : Unit
    {
        [SerializeField] Sprite _avatar;
        public Sprite avatar
        {
            get
            {
                return _avatar;
            }
            private set
            {
                _avatar = value;
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
