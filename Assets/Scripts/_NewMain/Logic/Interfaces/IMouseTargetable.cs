using UnityEngine;
namespace BattlescapeLogic
{
    public interface IMouseTargetable
    {
        void OnMouseHoverEnter(Vector3 exactMousePosition);
        void OnMouseHoverExit();        
    }
}