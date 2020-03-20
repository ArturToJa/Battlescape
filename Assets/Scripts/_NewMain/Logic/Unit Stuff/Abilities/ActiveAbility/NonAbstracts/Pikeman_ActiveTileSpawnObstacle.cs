using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Pikeman_ActiveTileSpawnObstacle : ActiveTileSpawnObstacle
    {
        [Space]
        [SerializeField] Sound onHitTheGroundSound;

        //LITERALLY only difference from base is that Pikeman loses his pike while using this - he regains it a moment later (takes one from his back and it magically doubles so that he still has 2).
        //Its fired from the next animation, not connected with this ability at all :D
        //Also - has sound on sticking it into the ground!

        public override void OnAnimationEvent()
        {
            base.OnAnimationEvent();
            owner.equipment.HideWeapons();
            BattlescapeSound.SoundManager.instance.PlaySound(owner.gameObject, onHitTheGroundSound);
        }
    }
}