using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] GameObject itemEquipped;
       
        [SerializeField] GameObject itemHidden;
        
        public void Hide()
        {
            if (itemHidden != null)
            {
                itemHidden.SetActive(true);
            }
            if (itemEquipped != null)
            {
                itemEquipped.SetActive(false);
            }
        }

        public void Equip()
        {
            if (itemHidden != null)
            {
                itemHidden.SetActive(false);
            }
            if (itemEquipped != null)
            {
                itemEquipped.SetActive(true);
            }
        }
    }

    [System.Serializable]
    public class Weapon : Item
    {
        [SerializeField] bool _isRanged;
        public bool isRanged
        {
            get
            {
                return _isRanged;
            }
            private set
            {
                _isRanged = value;
            }
        }
    }
}