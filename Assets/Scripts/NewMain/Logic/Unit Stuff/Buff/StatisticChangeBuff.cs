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
			owner.statistics.ApplyBonusStatistics(statistics);
		}

		protected override void RemoveChange()
		{
			owner.statistics.RemoveBonusStatistics(statistics);
		}
	}
}