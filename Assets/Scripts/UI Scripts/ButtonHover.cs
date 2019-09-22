using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<ButtonSounds>().PlaySound(FindObjectOfType<ButtonSounds>().HoverSource);
    }

    
}
