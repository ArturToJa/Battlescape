using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class AbstractActiveTargetAbility : AbstractActiveAbility
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

        public override void OnClickIcon()
        {
            base.OnClickIcon();
            ColourPossibleTargets();
        }
        public override void ColourPossibleTargets()
        {
            foreach(Tile tile in Global.instance.currentMap.board)
            {
                if (CheckTarget(tile))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.green);
                }
            }
            foreach(Unit unit in Global.instance.GetAllUnits())
            {
                if (CheckTarget(unit))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.green);
                }
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            MonoBehaviour tar = target as MonoBehaviour;
            int x = (int)tar.transform.position.x;
            int y = (int)tar.transform.position.z;
            Tile targetTile = Tile.ToTile(new Position(x, y));
            return this.owner.currentPosition.DistanceTo(targetTile) <= range;
        }

        public override void OnAnimationEvent()
        {}
    }
}