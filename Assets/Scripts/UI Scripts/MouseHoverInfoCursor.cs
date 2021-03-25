using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattlescapeLogic;

public class MouseHoverInfoCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip { get; private set; }
    public string tooltipName;
    public string tooltipText;
    MouseHoverInfoCursor hoveredIcon;

    private void Awake()
    {
        tooltip = GameObject.FindGameObjectWithTag("Tooltip");
        tooltip.GetComponent<CanvasGroup>().alpha = 0f;
        tooltip.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && hoveredIcon == this)
        {
            tooltip.GetComponent<CanvasGroup>().alpha = 1f;
            tooltip.GetComponent<CanvasGroup>().blocksRaycasts = true;
            tooltip.GetComponentsInChildren<Text>()[0].text = tooltipName;
            tooltip.GetComponentsInChildren<Text>()[1].text = tooltipText;
            tooltip.transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
            //and here
            Helper.CheckIfInBoundries(tooltip.transform);
        }
        if (Input.GetMouseButtonUp(1))
        {
            tooltip.GetComponent<CanvasGroup>().alpha = 0f;
            tooltip.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {       
        hoveredIcon = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredIcon = null;
    }
}
