using UnityEngine;

namespace BattlescapeLogic
{
    public interface IDamageable : IMouseTargetable, IOnTilePlaceable
    {

        BuffGroup buffs { get; }

        string GetMyName();

        Player GetMyOwner();

        Vector3 GetMyPosition();

        void TakeDamage(Damage dmg);

        int GetCurrentDefence();
        float ChanceOfBeingHitBy(IDamageSource source);

        bool IsInvulnerable();

        void OnHitReceived(Damage damage);
        void OnMissReceived(Damage damage);
        void OnHitReceivedWhenInvulnerable(Damage damage);
        bool IsAlive();
    }
}
