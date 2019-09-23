using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PassiveAbility : MonoBehaviour
{
    public string AbilityIconName;
    bool wasSelectedLastTimeWeChecked = false;
    bool wasHighlightedLastTimeWeChecked = false;
    protected GameObject AbilityIcon;
    ShowAbilitiesUI RealTooltip;
    protected GameObject IconForReal;
    ShowAbilitiesUI UnrealTooltip;
    protected GameObject IconForUnreal;
    protected UnitScript myUnit;
    [Header("Modyfier Values")]
    public int AttackModifierVersusUnitType;
    public int DefenceModifierVersusUnitType;
    public bool HasIcon;

    public abstract int GetAttack(UnitScript other);
    public abstract int GetDefence(UnitScript other);

    void AssignTooltips()
    {
        var temp = Resources.FindObjectsOfTypeAll<ShowAbilitiesUI>();
        foreach (var item in temp)
        {
            if (item.RealOne)
            {
                RealTooltip = item;
            }
            else
            {
                UnrealTooltip = item;
            }
        }
    }
    public void Start()
    {
        myUnit = this.GetComponent<UnitScript>();
        AssignTooltips();
        if (AbilityIconName != string.Empty && HasIcon)
        {
            AbilityIcon = (GameObject)Resources.Load(AbilityIconName);
            if (AbilityIcon == null)
            {
                Debug.LogError("No ability icon");
            }
        }
        ChangableStart();
    }

    public void Update()
    {
        if (AbilityIcon == null)
        {
            //Debug.Log(name);
        }
        ChangableUpdate();
        if (myUnit == MouseManager.Instance.SelectedUnit && wasSelectedLastTimeWeChecked == false && HasIcon)
        {
            // so we are selected, but didnt tell it to ourselves yet
            IconForReal = RealTooltip.AddAbilityIcon(AbilityIcon, true);
            wasSelectedLastTimeWeChecked = true;
        }
        if (myUnit != MouseManager.Instance.SelectedUnit && wasSelectedLastTimeWeChecked)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(IconForReal);
            }
            else
            {
                Destroy(IconForReal);
            }
            wasSelectedLastTimeWeChecked = false;
        }


        if (EnemyTooltipHandler.isOn && UnrealTooltip.Unit == myUnit && wasHighlightedLastTimeWeChecked == false && HasIcon)
        {
            IconForUnreal = UnrealTooltip.AddAbilityIcon(AbilityIcon, false);
            wasHighlightedLastTimeWeChecked = true;
        }
        if ((EnemyTooltipHandler.isOn == false || UnrealTooltip.Unit != myUnit) && wasHighlightedLastTimeWeChecked == true)
        {
            wasHighlightedLastTimeWeChecked = false;
            if (Application.isEditor)
            {
                DestroyImmediate(IconForUnreal);
            }
            else
            {
                Destroy(IconForUnreal);
            }

        }
    }

    protected abstract void ChangableStart();

    protected abstract void ChangableUpdate();

}
