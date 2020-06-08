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

        public override void OnCursorOver(IMouseTargetable target, Vector3 exactMousePosition)
        {
            if (target is Obstacle && IsLegalTarget(target))
            {
                var targettedObstacle = target as Obstacle;
                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourPossibleTargets();

                if (possibleTile(targettedObstacle.transform.position) != null)
                {
                    foreach(Tile tile in possibleTile(targettedObstacle.transform.position))
                    {
                        tile.highlighter.TurnOn(possibleTileColour);
                    }
                }
            }
            else
            {
                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourPossibleTargets();
            }
        }

        MultiTile possibleTile(Vector3 targetPos)
        {

            //var VectorToTarget = -transform.position + targetPos;
            //
            //Ray ray = new Ray(targetPos, VectorToTarget);
            //RaycastHit[] hits = Physics.RaycastAll(ray, JumpLenght);
            //
            //
            //
            //foreach (var hit in hits)
            //{
            //    if (hit.transform.GetComponent<Tile>() != null && hit.transform.GetComponent<Tile>().myObstacle != null && hit.transform.GetComponent<Tile>().myObstacle.isTall)
            //    {
            //        break;
            //    }
            //    else if (hit.transform.GetComponent<Tile>() != null && hit.transform.GetComponent<Tile>().myObstacle == null && hit.transform.GetComponent<Tile>().myUnit == null)
            //    {
            //        list.Add(hit.transform.GetComponent<Tile>());
            //    }
            //}

            var list = new List<MultiTile>();

            Obstacle targetedObstacle = target as Obstacle;

            Vector3 dest = owner.currentPosition.center - targetedObstacle.currentPosition.center;
            dest.Normalize();

            for(int i = 1; i <= JumpLenght; i++)
            {
                if(owner.currentPosition.Offset((int)dest.x * i, (int)dest.z * i).IsWalkable() == true)
                {
                    list.Add(owner.currentPosition.Offset((int)dest.x * i, (int)dest.z * i));
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

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
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
            foreach (Obstacle obstalce in Global.instance.currentMap.obstacles)
            {
                if (IsLegalTarget(obstalce))
                {
                    foreach (Tile tile in obstalce.currentPosition)
                    {
                        tile.highlighter.TurnOn(targetColouringColour);
                    }
                }
            }
        }

    }
}
