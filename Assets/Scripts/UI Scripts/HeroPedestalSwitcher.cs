using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroPedestalSwitcher : MonoBehaviour, IPointerEnterHandler
{
    SelectedUnitSwitcheroo sus;
    [SerializeField] GameObject PedestalUnit;
    [SerializeField] Text heroTypeSpot;

    void Start()
    {
        sus = FindObjectOfType<SelectedUnitSwitcheroo>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(PedestalUnit);
        sus.SetActiveUnitTo(PedestalUnit);
        heroTypeSpot.text = PedestalUnit.name;
    }
}