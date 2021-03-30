using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is an interface used on things, that can 'act' on IMouseTargettables: That is, Units, ActiveAbilities and the Player himself.
//If the current ActiveEntity (in Global.instance) is Player it means no unit is selected;
//If it is Unit, it means a Unit is selected, but no Ability is selected on that unit.
//If it is an ActiveAbility, then that Ability is being targetted right now (note - no target abilities 'become' current ActiveEntity for less than one frame (not sure tho, check that, they should) and immidiately get used and stop being Active.
namespace BattlescapeLogic
{
    public interface IActiveEntity
    {
        void OnLeftClick(IMouseTargetable target, Vector3 exactClickPoint);
        void OnRightClick(IMouseTargetable target);
        void OnCursorOver(IMouseTargetable target, Vector3 exactMousePosition);
    }
}