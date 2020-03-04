using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

public class MouseHoverAbilityIconCursor : MouseHoverInfoCursor
{   
    ActiveAbilityTooltip aat;
    public AbstractActiveAbility myAbility { get; set; }
    static bool increaseHover = false;

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
        increaseHover = true;
        aat.SkillNameTxt.text = myAbility.abilityName;
        aat.SkillOracleTxt.text = myAbility.description;
        if (myAbility.usesPerBattle > 0)
        {
            aat.LimitedText.text = "x" + myAbility.usesLeft;
            aat.LimitedField.SetActive(true);
        }
        else
        {
            aat.LimitedField.SetActive(false);
        }
        
        aat.CostText.text = "-" + myAbility.energyCost;
        if (SceneManager.GetActiveScene().name != "_ManagementScene")
        {
            myAbility.OnMouseHovered();
        }

        //Add icon for cooldown?

    }

    public void NotHover()
    {
        increaseHover = false;
    }


}
