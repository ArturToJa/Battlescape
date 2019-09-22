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
    [SerializeField] UnitScript myUnit;

    void Start()
    {
        sus = FindObjectOfType<SelectedUnitSwitcheroo>();
        myUnit = PedestalUnit.GetComponent<UnitScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sus.SetActiveUnitTo(PedestalUnit);
        heroTypeSpot.text = myUnit.unitUnit.Name;
    }
}