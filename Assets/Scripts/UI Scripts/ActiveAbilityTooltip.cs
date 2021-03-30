using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BattlescapeLogic;

public class ActiveAbilityTooltip : MonoBehaviour
{
    [SerializeField] Text _abilityNameText;
    public Text abilityNameTxt => _abilityNameText;


    [SerializeField] Text _abilityDescriptionText;
    public Text abilityDescriptionText => _abilityDescriptionText;

        
    float maxHovering = 1;
    public float isHovering { get; set; }
    bool isOpened = false;
    // Is Opened is just an internal tool to check if a tooltip was already opened (to not make this update  open opened tooltip every frame or to not close a closed one).


    void Start()
    {
        CloseTooltip();
        maxHovering = PlayerPrefs.GetFloat("TimeTillOpenTooltip");
    }


    void Update()
    {
        if (isHovering>maxHovering && !isOpened)
        {
            OpenTooltip();
        }
        else if (isOpened && isHovering ==0 )
        {
            CloseTooltip();
        }
    }

    public void OpenTooltip()
    {
        isOpened = true;
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseTooltip()
    {
        isOpened = false;
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
