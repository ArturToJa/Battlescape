using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShootingAttack : BaseAttack
    {
        //the prefab of missile shot by this unit
        [SerializeField] GameObject missilePrefab;

        //this is where EXACTLY we spawn the object - so that we dont spawn arrows in the middle of an archer etc.
        //I THINK it NEEDS to be a separate TRANSFORM, not a V3,  for ease reasons, because we can drag it in an inspector instead of copying and pasting values 
        //(i guess it is because of reference type vs value type? but i have no clue).
        [SerializeField] Transform spawningPoint;

        public ShootingAttack(Unit _myUnit) : base(_myUnit)
        {
        }

        public override void Attack(Unit target)
        {
            TurnTowards(target.transform.position);
            PlayAttackAnimation();
            //not sure if we 'DealDamage' here or later - after it hits the dude.
            //my concept owuld be to calculate the damage here - so that whatever happens after (maybe a player finds some bug allowing him to command some action before the missile hits, i don't care), does not matter
            //cause the damage is already counted and the unit's fate is already checked (but it owuld need to really already count if the unit is dead or not etc)
            // and it would only show it to the players after the missile hits.
            //the alternative is to do them ath after the missile hits or to show dmg before the hit - the most dumb version (it was implemented in old BS code tho ;/)
            DealDamageTo(target);
            GameObject missile = SpawnMissile(target.currentPosition);
            FlyMissile(missile, target.currentPosition);
        }

        protected override void PlayAttackAnimation()
        {
            myUnit.animator.SetTrigger("Shooting");
        }

        //Note, this has a Tile as a target and not a Unit - the reason being we might have AOE Abilities targetting 'empty' tiles (or e.g. Obstacles).
        GameObject SpawnMissile(Tile target)
        {
            GameObject newMissile = GameObject.Instantiate(missilePrefab);
            newMissile.transform.position = spawningPoint.position;
            newMissile.transform.LookAt(target.transform.position + new Vector3(0, 0.5f, 0));
            newMissile.transform.Rotate(Vector3.left, 45, Space.Self);
            //this whole rotating stuff can be SURELY simplified.
            //I just copied it from the old code cause im a lazy bastard.
            return newMissile;
        }

        IEnumerator FlyMissile(GameObject missile, Tile target)
        {
            //here we need some complex stuff! Someone smarter than me would be nice to have here ;D cause my missiles in the old BS... terrible!
            yield return null; 
        }
    }
}
