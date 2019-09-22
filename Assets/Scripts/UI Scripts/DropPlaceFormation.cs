using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropPlaceFormation : MonoBehaviour//, IDropHandler
{/*
    [SerializeField] bool isReal;
    public bool isOccupied = false;
    [SerializeField] int x;
    [SerializeField] int z;
    public static DropPlaceFormation[,] DPFBoard;

    void Awake()
    {
        if (DPFBoard == null)
        {
            DPFBoard = new DropPlaceFormation[11,4];
        }
        DPFBoard[x, z] = this;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isOccupied == true || isReal == false)
        {
            return;
        }
        DropIt(DragableUnitIcon.objectBeingDragged);
        SaveLoadManager.unitPositions.Add(new LemurVector2(x, z), DragableUnitIcon.objectBeingDragged.GetComponent<DragableUnitIcon>().me);
    }

    public void DropIt(GameObject droppo)
    {
        if (SaveLoadManager.unitPositions == null)
        {
            SaveLoadManager.unitPositions = new Dictionary<LemurVector2, Unit>();
        }
        if (droppo.transform.parent != null && droppo.transform.parent.GetComponent<DropPlaceFormation>() != null)
        {
            DropPlaceFormation tempo = droppo.transform.parent.GetComponent<DropPlaceFormation>();
            tempo.isOccupied = false;
            SaveLoadManager.unitPositions.Remove(new LemurVector2(tempo.x, tempo.z));
        }
        droppo.transform.SetParent(transform);
        droppo.transform.localPosition = Vector3.zero;
        isOccupied = true;
    }
    */
}
