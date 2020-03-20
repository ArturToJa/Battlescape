using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{

    //This is a 'buff' that does NOT buff at all, it is JUST a device used to animate a buffed entity differently when its buffed by another buff.
    public class AnimationBuff : AbstractBuff
    {
        [SerializeField] string setBool;
        [SerializeField] bool setBoolValue;

        public override void ApplyChange()
        {
            if (string.IsNullOrEmpty(setBool) == false)
            {
                owner.animator.SetBool(setBool, setBoolValue);
            }
            else
            {
                Debug.LogError("Did not set string value!");
            }
        }

        protected override void RemoveChange()
        {
            owner.animator.SetBool(setBool, !setBoolValue);
        }
    }
}