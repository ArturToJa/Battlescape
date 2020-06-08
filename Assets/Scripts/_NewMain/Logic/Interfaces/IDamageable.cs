using UnityEngine;

namespace BattlescapeLogic
{
    public interface IDamageable: IMouseTargetable
    {

        BuffGroup buffs { get;}

        string GetMyName();

        int GetDistanceTo(MultiTile postion);

        Player GetMyOwner();

        Transform GetMyTransform();

        Vector3 GetMyPosition();

        void TakeDamage(Unit source, int dmg);

        int GetCurrentDefence();
        float ChanceOfBeingHitBy(Unit source);
    }
}
