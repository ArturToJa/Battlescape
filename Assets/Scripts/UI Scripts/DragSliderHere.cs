using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSliderHere : MonoBehaviour
{
    [SerializeField] RectTransform slider;
    [SerializeField] RectTransform newParent;

    // Use this for initialization
    public void DragSlider()
    {
        slider.SetParent(newParent, false);
        slider.localPosition = new Vector2(0, -150);
    }
}
