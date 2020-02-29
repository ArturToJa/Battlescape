using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAuraAbility : AbstractPassiveAbility
    {
        public int range { get; protected set; }

        public override void OnNewRound()
        {
            // Get Units in range of owner
            // filter these units
            // apply buff on these units
            ApplyAuraForPlayerTeams();
        }

        private void ApplyAuraForPlayerTeams()
        {
            foreach (PlayerTeam playerTeam in Global.instance.playerTeams)
            {
                if ((filter[(int)AbilityFilter.Ally] && owner.owner.team == playerTeam) ||
                    (filter[(int)AbilityFilter.Enemy] && owner.owner.team != playerTeam))
                {
                    ApplyAuraForPlayers(playerTeam);
                }
            }
        }

        private void ApplyAuraForPlayers(PlayerTeam playerTeam)
        {
            foreach (Player player in playerTeam.players)
            {
                if ((filter[(int)AbilityFilter.SelfPlayer] && owner.owner == player) ||
                    (filter[(int)AbilityFilter.OtherPlayer] && owner.owner != player))
                {
                    ApplyAuraForUnit(player);
                }
            }
        }

        private void ApplyAuraForUnit(Player player)
        {
            foreach (Unit unit in player.playerUnits)
            {
                Position unitPosition = unit.currentPosition.position;
                Position ownerPosition = owner.currentPosition.position;
                if (Mathf.Abs(Mathf.Sqrt(Mathf.Pow(unitPosition.x, 2) + Mathf.Pow(unitPosition.z, 2)) -
                              Mathf.Sqrt(Mathf.Pow(ownerPosition.x, 2) + Mathf.Pow(ownerPosition.z, 2))) <= range)
                {
                    if ((filter[(int)AbilityFilter.Self] && unit == owner) &&
                       ((filter[(int)AbilityFilter.Hero] && unit.GetType().Equals(typeof(Hero))) ||
                        (filter[(int)AbilityFilter.Regular] && !(unit.GetType().Equals(typeof(Hero))))) &&
                       ((filter[(int)AbilityFilter.Ranged] && unit.attackType == AttackTypes.Ranged) ||
                        (filter[(int)AbilityFilter.Melee] && unit.attackType == AttackTypes.Melee)) &&
                       ((filter[(int)AbilityFilter.Ground] && unit.movementType == MovementTypes.Ground) ||
                        (filter[(int)AbilityFilter.Flying] && unit.movementType == MovementTypes.Flying)))
                    {
                        ApplyBuffsToUnit(unit);
                        bool vara = unit.GetType().Equals(typeof(Hero));
                    }
                }
            }
        }


    }
}