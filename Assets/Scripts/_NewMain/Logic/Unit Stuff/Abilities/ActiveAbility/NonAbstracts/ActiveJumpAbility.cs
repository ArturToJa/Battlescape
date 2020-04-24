using UnityEngine;
using System.Collections.Generic;
namespace BattlescapeLogic
{
   public class ActiveJumpAbility : AbstractActiveAbility
   {
       [Header("Jump Configuration")]
       [Space]
       [SerializeField] bool Units;
       [SerializeField] bool Obstacles;
       [SerializeField] float JumpLenght;
       [SerializeField] Color possibleTileColour;

        protected override void Activate()
        {
            base.Activate();
            //teleport unit to targetted position with animation
        }

        public override void OnCursorOver(IMouseTargetable target)
        {
            if (target is Obstacle && IsLegalTarget(target))
            {
                var targettedObstacle = target as Obstacle;
                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourPossibleTargets();

                if (possibleTile(targettedObstacle.transform.position) != null)
                {
                    possibleTile(targettedObstacle.transform.position).highlighter.TurnOn(possibleTileColour);
                }
            }
            else if (target is Unit && IsLegalTarget(target))
            {
                var targettedUnit = target as Unit;

                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourPossibleTargets();
                if (possibleTile(targettedUnit.transform.position) != null)
                {
                    possibleTile(targettedUnit.transform.position).highlighter.TurnOn(possibleTileColour);
                }
            }
            else
            {
                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourPossibleTargets();
            }
        }

        Tile possibleTile(Vector3 targetPos)
        {

            var VectorToTarget = -transform.position + targetPos;

            Ray ray = new Ray(targetPos, VectorToTarget);
            RaycastHit[] hits = Physics.RaycastAll(ray, JumpLenght);

            var list = new List<Tile>();

            foreach (var hit in hits)
            {
                if (hit.transform.GetComponent<Tile>() != null && hit.transform.GetComponent<Tile>().myObstacle == null && hit.transform.GetComponent<Tile>().myUnit == null)
                {
                    list.Add(hit.transform.GetComponent<Tile>());
                }
            }

            if (list.Count != 0)
            {
                return list[list.Count - 1];
            }
            else return null;

        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Obstacle && Obstacles)
            {
                Obstacle targetObstacle = target as Obstacle;
                return IsInRange(targetObstacle) && filter.FilterObstacle(targetObstacle);
            }
            if(target is Unit && Units)
            {
                Unit targetUnit = target as Unit;
                return IsInRange(targetUnit) && filter.FilterUnit(targetUnit);
            }
            return false;
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile.myObstacle) || IsLegalTarget(tile.myUnit))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

    }
}
