using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverInfoCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public GameObject Tooltip { get; private set; }
    public string tooltipName;
    public string tooltipText;
    MouseHoverInfoCursor hoveredIcon;

    private void Awake()
    {
        Tooltip = GameObject.FindGameObjectWithTag("Tooltip");
        Tooltip.GetComponent<CanvasGroup>().alpha = 0f;
        Tooltip.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && hoveredIcon == this)
        {
            Tooltip.GetComponent<CanvasGroup>().alpha = 1f;
            Tooltip.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Tooltip.GetComponentsInChildren<Text>()[0].text = tooltipName;
            Tooltip.GetComponentsInChildren<Text>()[1].text = tooltipText;
            Tooltip.transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
            //and here
            Helper.CheckIfInBoundries(Tooltip.transform);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Tooltip.GetComponent<CanvasGroup>().alpha = 0f;
            Tooltip.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this is MouseHoverAbilityIconCursor == false)
        {
            CursorController.instance.SetCursorTo(CursorController.instance.infoCursor, CursorController.instance.infoCursor);
            CursorController.instance.isInfoByUI = true;
        }
        hoveredIcon = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredIcon = null;
        CursorController.instance.isInfoByUI = false;
    }
}
