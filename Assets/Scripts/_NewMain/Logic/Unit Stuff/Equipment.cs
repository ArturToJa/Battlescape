using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{

    [System.Serializable]
    public class Equipment
    {
        [SerializeField] Weapon primaryWeapon;
        [SerializeField] Weapon secondaryWeapon;



        

        public void EquipMainMeleeWeapon()
        {
            Weapon weapon = GetMyMainMeleeWeapon();
            if (weapon != null)
            {
                Equip(weapon);
            }
        }

        public void EquipMainRangedWeapon()
        {
            Weapon weapon = GetMyMainRangedWeapon();
            if (weapon != null)
            {
                Equip(weapon);
            }
        }       

        public void EquipPrimaryWeapon()
        {
            primaryWeapon.Equip();
            secondaryWeapon.Hide();
        }

        public void EquipSecondaryWeapon()
        {
            primaryWeapon.Hide();
            secondaryWeapon.Equip();
        }

        public void HideWeapons()
        {
            primaryWeapon.Hide();
            secondaryWeapon.Hide();
        }





        void Equip(Weapon weapon)
        {
            if (weapon == primaryWeapon)
            {
                EquipPrimaryWeapon();
            }
            else if(weapon == secondaryWeapon)
            {
                EquipSecondaryWeapon();
            }
            else
            {
                Debug.LogError("Equipping a non-defined weapon!");
            }
        }        

        Weapon GetMyMainRangedWeapon()
        {
            if (primaryWeapon.isRanged)
            {
                return primaryWeapon;
            }
            else if(secondaryWeapon.isRanged)
            {
                return secondaryWeapon;
            }
            else
            {
                return null;
            }
        }

        Weapon GetMyMainMeleeWeapon()
        {
            if (primaryWeapon.isRanged == false)
            {
                return primaryWeapon;
            }
            else if (secondaryWeapon.isRanged == false)
            {
                return secondaryWeapon;
            }
            else
            {
                return null;
            }
        }
    }
}