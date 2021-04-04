using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BattlescapeSound;

[DisallowMultipleComponent]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    void Awake()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.Log("Sounds but no button on: " + gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.hoverSound);
    }

    void OnClick()
    {
        SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.clickSound);
    }


}
