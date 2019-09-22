using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUnitSwitcheroo : MonoBehaviour
{

    List<GameObject> Units;

    private void Start()
    {
        Units = new List<GameObject>();
        foreach (Transform item in this.transform)
        {
            if (item.GetComponent<UnitScript>() != null)
            {
                Units.Add(item.gameObject);
            }
        }
    }

    public void SetActiveUnitTo(GameObject unit)
    {
        if (Units.Contains(unit) == false)
        {
            Debug.LogError("Trying to show on a pedestal a unit that is not in pedestal's children");
            return;
        }
        else
        {
            unit.SetActive(true);
            foreach (GameObject r in Units)
            {
                if (r != unit)
                {
                    r.SetActive(false);
                }
            }
        }
    }
}
