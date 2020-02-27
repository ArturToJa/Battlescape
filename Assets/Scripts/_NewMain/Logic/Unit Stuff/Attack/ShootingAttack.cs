using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShootingAttack : AbstractAttack
    {
        //the prefab of missile shot by this unit

        //this is where EXACTLY we spawn the object - so that we dont spawn arrows in the middle of an archer etc.
        //I THINK it NEEDS to be a separate TRANSFORM, not a V3,  for ease reasons, because we can drag it in an inspector instead of copying and pasting values 
        //(i guess it is because of reference type vs value type? but i have no clue).
        [SerializeField] Transform spawningPoint;

        public ShootingAttack(Unit _myUnit) : base(_myUnit)
        {
            if (_myUnit.meleeWeaponVisual != null)
            {
                _myUnit.meleeWeaponVisual.SetActive(false);
            }
        }

        public override void Attack(Unit target)
        {
            base.Attack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Shooting");
        }

        //Note, this has a Tile as a target and not a Unit - the reason being we might have AOE Abilities targetting 'empty' tiles (or e.g. Obstacles).
        void SpawnMissile(Tile target)
        {
            GameObject missileObject = GameObject.Instantiate(sourceUnit.missilePrefab, sourceUnit.transform.position, sourceUnit.transform.rotation);
            Missile missileScript = missileObject.GetComponent<Missile>();
            missileScript.startingPoint = missileScript.transform.position;
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missileScript.sourceUnit = sourceUnit;
            missileScript.target = targetUnit;
        }

        IEnumerator FlyMissile(GameObject missile, Tile target)
        {
            //here we need some complex stuff! Someone smarter than me would be nice to have here ;D cause my missiles in the old BS... terrible!
            yield return null;
        }

        // Ranged unit does nothing on it's attack animation
        public override void OnAttackAnimation()
        {
        }

        public override void OnRangedAttackAnimation()
        {
            SpawnMissile(targetUnit.currentPosition);
        }
    }
}
