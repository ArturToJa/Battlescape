using UnityEngine;

namespace BattlescapeLogic
{
    public interface IDamageable: IMouseTargetable
    {

        BuffGroup buffs { get;}

        string GetMyName();

        int GetDistanceTo(Position postion);

        Player GetMyOwner();

        Vector3 GetMyPosition();

        void TakeDamage(Unit source, int dmg);

        int GetCurrentDefence();
    }
}
