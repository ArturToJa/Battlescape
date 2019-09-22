using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbilityIconScript : MonoBehaviour
{
    public Image myImage;
    public Text LimitedText;
    public Text EnergyText;

    ActiveAbilityTooltip aat;
    public Ability_Basic myAbility;
    static bool increaseHover = false;
    Text uses;
    public static bool IsAnyAbilityHovered;

    void Start()
    {
        aat = FindObjectOfType<ActiveAbilityTooltip>();
    }

    void Update()
    {
        if (increaseHover)
        {
            aat.isHovering += Time.deltaTime;
        }
        else
        {
            aat.isHovering = 0;
        }
    }

    public void Hover()
    {
        IsAnyAbilityHovered = true;
        increaseHover = true;
        aat.SkillNameTxt.text = myAbility.Name;
        aat.SkillOracleTxt.text = myAbility.TooltipInfo;
        if (myAbility.LimitedUses)
        {
            aat.LimitedText.text = "x" + myAbility.UsesLeft;
            aat.LimitedField.SetActive(true);
        }
        else
        {
            aat.LimitedField.SetActive(false);
        }
        
        aat.CostText.text = "-" + myAbility.EnergyCost;
        if (SceneManager.GetActiveScene().name != "_ManagementScene")
        {
            myAbility.OnHover();
        }

    }

    public void NotHover()
    {
        IsAnyAbilityHovered = false;
        increaseHover = false;
    }

}
