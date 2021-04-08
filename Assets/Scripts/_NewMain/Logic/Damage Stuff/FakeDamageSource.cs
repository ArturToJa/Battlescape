namespace BattlescapeLogic
{
    public class FakeDamageSource : IDamageSource
    {
        public int attack { get; }
        public FakeDamageSource(int _attack)
        {
            attack = _attack;
        }

        public bool CanPotentiallyDamage(IDamageable target)
        {
            return true;
        }

        public Unit GetMyOwner()
        {
            return null;
        }

        public PotentialDamage GetPotentialDamageAgainst(IDamageable target)
        {
            return new PotentialDamage(1000, 0, 1);
        }

        public void OnKillUnit(Unit unit)
        {
            return;
        }
    }
}