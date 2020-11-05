using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeSound;

[DisallowMultipleComponent]
public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.hoverSound);
    }

    
}
