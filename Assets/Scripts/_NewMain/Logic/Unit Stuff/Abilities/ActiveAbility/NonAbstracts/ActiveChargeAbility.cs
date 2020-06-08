using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveChargeAbility : AbstractAttackAbility
    {
        [Header("Charge Configuration")]
        [Space]
        [SerializeField] private bool MakeRangeEqualsMovementPoints;
        [SerializeField] private Color combatColor;
        [SerializeField] private float bonusDamageForEachMeter;
        [SerializeField] private bool canAttackAfterCharge;
        [Header("SelfBuff Configuration")]
        [Space]
        [SerializeField] private bool buffInstantly;
        [SerializeField] private bool buffAfterFullMove;
        [SerializeField] private bool buffAfterAttack;
        [SerializeField] private bool buffAlwaysAtTheEnd;


        private bool waitToAttack = false;
        private int prevMovementPoints;
        private float distanceTravelled;

        private void Update()
        {
            if (waitToAttack == true && owner.movement.isMoving == false)
            {
                List<Unit> targetsList = new List<Unit>();

                foreach (Tile tile in owner.GetMyTile().neighbours)
                {
                    if(tile.myUnit != null && tile.myUnit.GetMyOwner().team != owner.GetMyOwner().team)
                    {
                        targetsList.Add(tile.myUnit);
                    }
                }

                if (buffAlwaysAtTheEnd || (buffAfterFullMove && owner.statistics.movementPoints + rangeModifier == 0))
                {
                    SelfBuffNow();
                }

                if (targetsList.Count > 0)
                {
                    AttackOpponent(targetsList);
                }
                
                waitToAttack = false;
            }
        }

        protected override void Activate()
        {
            base.Activate();
            var targetedTile = target as Tile;
            distanceTravelled = Vector2.Distance(owner.transform.position, targetedTile.transform.position);
            prevMovementPoints = owner.statistics.movementPoints;
            Networking.instance.SendCommandToMove(owner, targetedTile);
            waitToAttack = true;

            if (buffInstantly)
            {
                SelfBuffNow();
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {
            if (target is Tile)
            {
                if (MakeRangeEqualsMovementPoints)
                {
                    range = owner.statistics.movementPoints;
                }
                var targetedTile = target as Tile;
                return IsInRange(targetedTile) && targetedTile.myObstacle == null && targetedTile.myUnit == null;
            }
            return false;
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile))
                {
                    foreach (Tile _tile in tile.neighbours)
                    {
                        if (_tile.myUnit != null && _tile.myUnit.GetMyOwner().team != owner.GetMyOwner().team)
                        {
                            tile.highlighter.TurnOn(combatColor);
                            break;
                        }
                        else
                        {
                            tile.highlighter.TurnOn(targetColouringColour);
                        }
                    }
                }
            }
        }

        private void AttackOpponent(List<Unit> unitList)
        {
            int randNumber = Random.Range(0, unitList.Count - 1);
            targetedUnit = unitList[randNumber];
            CalculateBonusDamage();
            owner.attack = new AbilityAttack(this);
            if (buffAfterAttack)
            {
                SelfBuffNow();
            }


            if (canAttackAfterCharge)
            {
                owner.attack.Attack(targetedUnit, false);
            }
            else
            {
                owner.attack.Attack(targetedUnit);
            }
        }

        private void CalculateBonusDamage()
        {
            bonusDamage += (int)(Mathf.Floor(distanceTravelled * bonusDamageForEachMeter));
        }

        private void SelfBuffNow()
        {
            ApplyBuffsToUnit(selfBuffs, owner);
        }

    }
}
