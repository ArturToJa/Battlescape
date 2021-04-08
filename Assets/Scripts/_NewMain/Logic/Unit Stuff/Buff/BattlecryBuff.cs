using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BattlecryBuff : StatisticChangeBuff
    {
        [SerializeField] GameObject onHitVisual;
        List<GameObject> visuals = new List<GameObject>();
        [SerializeField] ChangeableStatistics onHitStats;
        int hitCount = 0;
        
        public void OnDamageDealt(Damage damage, IDamageable target)
        {
            if (damage.source.GetMyOwner() == source.owner && damage.isHit)
            {
                hitCount++;
                Unit owner = buffGroup.owner as Unit;
                owner.statistics.ApplyBonusStatistics(onHitStats);
                visuals.Add(Instantiate(onHitVisual, transform.position, onHitVisual.transform.rotation, transform));
            }
        }

        protected override void RemoveChange()
        {
            base.RemoveChange();
            for (int i = 0; i < hitCount; i++)
            {
                Unit owner = buffGroup.owner as Unit;
                owner.statistics.RemoveBonusStatistics(onHitStats);
                if (Application.isEditor)
                {
                    DestroyImmediate(visuals[0]);
                }
                else
                {
                    Destroy(visuals[0]);
                }
            }
            Damage.OnDamageDealt -= OnDamageDealt;
        }
    }
}