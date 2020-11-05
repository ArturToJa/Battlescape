using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
	public class StatisticChangeBuff : AbstractBuff
	{
		public ChangeableStatistics statistics;
		public override void ApplyChange()
		{
			Unit owner = buffGroup.owner as Unit;
			owner.statistics.ApplyBonusStatistics(statistics);
		}

		protected override void RemoveChange()
		{
			Unit owner = buffGroup.owner as Unit;
			owner.statistics.RemoveBonusStatistics(statistics);
		}

		protected override bool IsAcceptableTargetType(IDamageable target)
		{
			return Tools.TypeComparizer<IDamageable, Unit>(target);
		}
	}
}