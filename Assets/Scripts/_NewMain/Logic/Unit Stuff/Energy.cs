using System;

namespace BattlescapeLogic
{
    public class Energy
    {
        public const int max = 100;
        public const int starting = max / 2;
        public const int baseRegen = 20;
        
        int _current = starting;
        public int current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value > max)
                {
                    _current = max;
                }
                else if (value < 0)
                {
                    _current = 0;
                }
                else
                {
                    _current = value;
                }
                OnValueChanged(this);
            }
        }
        public int bonusRegen { get; set; }

        public static event Action<Energy> OnValueChanged = delegate { };

        public int GetCurrentRegen()
        {
            return baseRegen + bonusRegen;
        }

        public void OnOwnerTurn()
        {
            current += GetCurrentRegen();            
        }
    }
}