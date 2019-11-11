using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class UnitSounds
    {

        [SerializeField] Sound _movementSound;
        public Sound movementSound
        {
            get
            {
                return _movementSound;
            }
            set
            {
                _movementSound = value;
            }
        }
        [SerializeField] Sound _deathSound;
        public Sound deathSound
        {
            get
            {
                return _deathSound;
            }
            set
            {
                _deathSound = value;
            }
        }
        [SerializeField] Sound _hitSound;
        public Sound hitSound
        {
            get
            {
                return _hitSound;
            }
            set
            {
                _hitSound = value;
            }
        }
        [SerializeField] Sound _shootSound;
        public Sound shootSound
        {
            get
            {
                return _shootSound;
            }
            set
            {
                _shootSound = value;
            }
        }
        [SerializeField] Sound _attackSound;
        public Sound attackSound
        {
            get
            {
                return _attackSound;
            }
            set
            {
                _attackSound = value;
            }
        }
    }
}

