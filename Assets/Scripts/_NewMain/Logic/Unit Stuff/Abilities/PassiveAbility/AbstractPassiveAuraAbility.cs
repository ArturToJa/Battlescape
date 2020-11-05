using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAuraAbility : AbstractPassiveAbility
    {
        [SerializeField] int _range;
        public int range
        {
            get
            {
                return _range;
            }
            protected set
            {
                _range = value;
            }
        }

        public override void OnNewRound()
        {
            // Get Units in range of owner
            // filter these units
            // apply buff on these units
            ApplyAuraForPlayerTeams();
        }

        void ApplyAuraForPlayerTeams()
        {
            foreach (PlayerTeam playerTeam in Global.instance.playerTeams)
            {
                if (filter.FilterTeam(playerTeam))
                {
                    ApplyAuraForTeam(playerTeam);
                }
            }
        }

        void ApplyAuraForTeam(PlayerTeam playerTeam)
        {
            foreach (Player player in playerTeam.players)
            {
                if (filter.FilterPlayer(player))
                {
                    ApplyAuraForPlayer(player);
                }
            }
        }

        void ApplyAuraForPlayer(Player player)
        {
            foreach (Unit unit in player.playerUnits)
            {
                Position unitPosition = unit.currentPosition.bottomLeftCorner.position;
                Position ownerPosition = owner.currentPosition.bottomLeftCorner.position;
                if (IsInRange(unit) && filter.FilterUnit(unit))
                {
                    ApplyBuffsToUnit(placeableBuffs, unit);
                }
            }
        }

        bool IsInRange(Unit unit)
        {
            return owner.currentPosition.bottomLeftCorner.position.DistanceTo(unit.currentPosition.bottomLeftCorner.position) <= range;
        }
    }
}