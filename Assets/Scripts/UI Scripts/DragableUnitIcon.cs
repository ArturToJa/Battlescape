using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUnitIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    float delay = 0.5f;
    float timeSinceClick = 0f;
    bool wasClickedThisSecond = false;
    public static GameObject objectBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    public Unit me;

    private void Start()
    {
        if (GetComponent<UnitScript>() != null)
        {
            me = GetComponent<UnitScript>().unitUnit;
        }
    }


    private void Update()
    {
        timeSinceClick += Time.deltaTime;
        if (timeSinceClick > delay)
        {

            wasClickedThisSecond = false;
        }
        if (objectBeingDragged != gameObject && GetComponent<CanvasGroup>() != null)
        {
            //we aint being dragged, therefore we have to be able to be. This line is most likley redundant but maybe it fights the deployment bug.
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        objectBeingDragged = gameObject;
        startPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        objectBeingDragged = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        if (wasClickedThisSecond == false)
        {
            foreach (DragableUnitIcon DUI in FindObjectsOfType<DragableUnitIcon>())
            {
                DUI.wasClickedThisSecond = false;
            }
            wasClickedThisSecond = true;
            timeSinceClick = 0;

        }
        else
        {
            foreach (DragableUnitIcon DUI in FindObjectsOfType<DragableUnitIcon>())
            {
                DUI.wasClickedThisSecond = false;
            }
            wasClickedThisSecond = false;
            timeSinceClick = 0;
            PreGameAI tool = new PreGameAI(FindObjectOfType<AI_Controller>().DeploymentPanel);
            tool.Position(me, tool.ChooseTheTile(me));
            Destroy(this.gameObject);

        }
        
    }
}
