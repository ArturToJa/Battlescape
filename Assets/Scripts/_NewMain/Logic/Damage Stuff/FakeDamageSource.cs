namespace BattlescapeLogic
{
    public class FakeDamageSource : IDamageSource
    {
        int attack;
        public FakeDamageSource(int _attack)
        {
            attack = _attack;
        }

        public bool CanPotentiallyDamage(IDamageable target)
        {
            return true;
        }

        public int GetAttackValue()
        {
            return attack;
        }

        public ModifierGroup GetMyDamageModifiers()
        {
            return null;
        }

        public string GetOwnerName()
        {
            return "God Himself";
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