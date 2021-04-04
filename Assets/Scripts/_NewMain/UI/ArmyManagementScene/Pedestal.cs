using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

public class Pedestal : MonoBehaviour
{
    [SerializeField] GameObject capsule;

    GameObject myUnit;

    void Awake()
    {
        MouseoveredButtonUnitScript.OnUnitHovered += ShowUnit;
    }

    public void ShowUnit(UnitCreator unitCreator)
    {
        if (myUnit != null)
        {
            Destroy(myUnit);
        }
        myUnit = Instantiate(unitCreator.prefab, capsule.transform.position, Quaternion.identity, capsule.transform);
        myUnit.GetComponent<Unit>().enabled = false;
        myUnit.GetComponent<DragableUnit>().enabled = false;
        myUnit.GetComponentInChildren<UnitColours>().enabled = false;
        myUnit.GetComponentInChildren<AnimationEvents>().enabled = false;
        myUnit.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        if (myUnit.GetComponent<Hero>() != null)
        {
            myUnit.GetComponentInChildren<Spinner>().gameObject.SetActive(false);
        }
        foreach (AbstractAbility ability in myUnit.GetComponents<AbstractAbility>())
        {
            ability.enabled = false;
        }


    }
}
