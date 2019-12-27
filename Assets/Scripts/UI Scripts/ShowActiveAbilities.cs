using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowActiveAbilities : MonoBehaviour
{
    [SerializeField] UnitStatShower USS;
    [SerializeField] bool ManagementScene;
    BattlescapeLogic.Unit myUnit;
    BattlescapeLogic.Unit myPrevUnit;
    [SerializeField] GameObject ActiveAbilityPrefab;

    void Update()
    {

        myUnit = SetCurrentUnit();

        if (myPrevUnit != myUnit && myUnit != null)
        {
            UpdateActives();
        }

        myPrevUnit = myUnit;
    }

    BattlescapeLogic.Unit SetCurrentUnit()
    {
        if (ManagementScene)
        {
            return USS.currUnit;
        }
        else
        {
            //if (MouseManager.Instance.MouseoveredUnit == null || EnemyTooltipHandler.isOn)
            //{
            //    return null;
            //}
            //return MouseManager.Instance.MouseoveredUnit;
        }
        return null;
    }

    void UpdateActives()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Ability_Basic ability in myUnit.GetComponents<Ability_Basic>())
        {
            //UIManager.UpdateAbilitiesPanel(gameObject, ActiveAbilityPrefab, myUnit);
            GameObject NewIcon = Instantiate(ActiveAbilityPrefab, this.transform);
            NewIcon.GetComponent<Image>().sprite = ability.mySprite;
            NewIcon.GetComponent<MouseHoverInfoCursor>().TooltipName = ability.Name;
            NewIcon.GetComponent<MouseHoverInfoCursor>().TooltipText = ability.TooltipInfo;
        }
    }
}
