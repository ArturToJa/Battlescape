using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is an interface used on things, that can 'act' on IMouseTargettables: That is, Units, Abilities(their Icons most likely) and the Player himself.
namespace BattlescapeLogic
{
    public interface IActiveEntity
    {
        void OnLeftClick(IMouseTargetable target);
        void OnRightClick(IMouseTargetable target);
        void OnCursorOver(IMouseTargetable target);
    }
}