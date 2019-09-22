using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowOnPedestal : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField] RenderTexture myUnit;
    [SerializeField] RawImage Image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image.texture = myUnit;
    }

    
}
