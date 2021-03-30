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
        aat.abilityNameTxt.text = myAbility.abilityName;
        aat.abilityDescriptionText.text = myAbility.description;
        if (myAbility.usesPerBattle > 0)
        {
            string red = ColorUtility.ToHtmlStringRGB(Global.instance.colours.red);
            string green = ColorUtility.ToHtmlStringRGB(Global.instance.colours.green);
            if (myAbility.usesLeft > 0)
            {
                aat.abilityDescriptionText.text += "\n" + "<color=#" + green + ">Uses left: " + myAbility.usesLeft.ToString() + "/" + myAbility.usesPerBattle.ToString() + "</color>";
            }
            else
            {
                aat.abilityDescriptionText.text += "\n" + "<color=#" + red + ">Uses left: " + myAbility.usesLeft.ToString() + "/" + myAbility.usesPerBattle.ToString() + "</color>";
            }         
        }       

        if (myAbility.roundsTillOffCooldown > 0)
        {
            string yellow = ColorUtility.ToHtmlStringRGB(Global.instance.colours.yellow);
            aat.abilityDescriptionText.text += "\n" + "<color=#" + yellow + ">Rounds until off of cooldown: " + myAbility.roundsTillOffCooldown.ToString() + "/" + myAbility.cooldown.ToString() + "</color>";
        }

        string blue = ColorUtility.ToHtmlStringRGB(Global.instance.colours.blue);
        aat.abilityDescriptionText.text += "\n" + "<color=#" + blue + ">Energy cost: " + myAbility.energyCost.ToString() + "</color>";

        if (SceneManager.GetActiveScene().name != "_ManagementScene")
        {
            myAbility.OnMouseHovered();
        }
    }

    public void NotHover()
    {
        increaseHover = false;
        if (SceneManager.GetActiveScene().name != "_ManagementScene")
        {
            myAbility.OnMouseUnHovered();
        }
    }


}
