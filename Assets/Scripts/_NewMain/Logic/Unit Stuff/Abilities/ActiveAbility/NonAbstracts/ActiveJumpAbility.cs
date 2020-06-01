using UnityEngine;
using System.Collections.Generic;
namespace BattlescapeLogic
{
    public class ActiveJumpAbility : AbstractActiveAbility
    {
        [Header("Unit can jump through:")]
        [Space]
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
                if (hit.transform.GetComponent<Tile>() != null && hit.transform.GetComponent<Tile>().myObstacle != null && hit.transform.GetComponent<Tile>().myObstacle.isTall)
                {
                    break;
                }
                else if (hit.transform.GetComponent<Tile>() != null && hit.transform.GetComponent<Tile>().myObstacle == null && hit.transform.GetComponent<Tile>().myUnit == null)
                {
                    list.Add(hit.transform.GetComponent<Tile>());
                }
            }

            if (list.Count != 0)
            {
                return list[list.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Obstacle)
            {
                Obstacle targetObstacle = target as Obstacle;
                return IsInRange(targetObstacle) && filter.FilterObstacle(targetObstacle);
            }
            return false;
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (tile.myObstacle!=null && IsLegalTarget(tile.myObstacle))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

    }
}
