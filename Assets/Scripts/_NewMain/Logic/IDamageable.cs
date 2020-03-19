using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlescapeLogic
{
    public interface IDamageable
    {
        void TakeDamage(Unit source, int dmg);
    }
}
