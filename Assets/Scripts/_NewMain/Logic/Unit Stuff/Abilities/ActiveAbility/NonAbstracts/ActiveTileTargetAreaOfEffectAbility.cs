using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTileTargetAreaOfEffectAbility : AbstractActiveTileTargetAbility
    {
        [SerializeField] int size;

        [SerializeField] List<GameObject> vfxOnTargetArea;

        [SerializeField] List<GameObject> buffs;

        public override void ColourPossibleTargets()
        {
            //We want to colour tiles we are over, not possible tiles with this ability IMO
            return;
        }

        public void ColourCurrentTargets(Tile hoveredTile)
        {
            if (IsLegalTarget(hoveredTile) == false)
            {
                return;
            }
            if (size < 1)
            {
                Debug.LogError("This value should always be at least 1 - otherwise this is not an AOE ability. COnsider changing this value or cahnging this ability's type");
            }
            foreach (Tile tile in GetTargetsForTile(hoveredTile))
            {
                tile.highlighter.TurnOn(targetColouringColour);
            }
        }

        protected List<Tile> GetTargetsForTile(Tile tile)
        {
            List<Tile> returnList = new List<Tile>();
            for (int i = -size + 1; i < size; i++)
                for (int j = -size + 1; j < size; j++)
                {
                    if (tile.position.x + i < Global.instance.currentMap.mapWidth && tile.position.x + i >= 0 && tile.position.z + j < Global.instance.currentMap.mapHeight && tile.position.z + j >= 0)
                    {
                        returnList.Add(Global.instance.currentMap.board[tile.position.x + i, tile.position.z + j]);
                    }
                }
            return returnList;
        }

        protected override void Activate()
        {
            base.Activate();
            Tile targetTile = target as Tile;
            DoVisualEffectsFor(vfxOnTargetArea, targetTile.gameObject);
            foreach (Tile tile in GetTargetsForTile(targetTile))
            {
                if (tile.myUnit != null)
                {
                    ApplyBuffsToUnit(buffs, tile.myUnit);
                }
            }

        }

        void DoVisualEffectsFor(List<GameObject> effects, GameObject target)
        {
            foreach (GameObject effect in effects)
            {
                DoVisualEffectFor(effect, target);
            }
        }

        public override void OnCursorOver(IMouseTargetable target)
        {
            if (target is Tile)
            {
                BattlescapeGraphics.ColouringTool.UncolourAllTiles();
                ColourCurrentTargets(target as Tile);
            }           
        }
    }
}