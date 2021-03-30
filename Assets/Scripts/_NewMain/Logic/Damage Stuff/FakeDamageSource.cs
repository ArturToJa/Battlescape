namespace BattlescapeLogic
{
    public class FakeDamageSource : IDamageSource
    {
        public bool CanPotentiallyDamage(IDamageable target)
        {
            return true;
        }

        public int GetAttackValue()
        {
            return 1000;
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