using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityTooltip : MonoBehaviour
{

    public Text SkillNameTxt;
    public Text SkillOracleTxt;
    public GameObject LimitedField;
    public Text LimitedText;
    public Text CostText;
    public float maxHovering = 1;
    public float isHovering = 0;
    bool isOpened = false;
    // Is Opened is just an internal tool to check if a tooltip was already opened (to not make this update  open opened tooltip every frame or to not close a closed one).


    Ability_Basic hoveredAbility;

    // Use this for initialization
    void Start()
    {
        LimitedField = LimitedText.transform.parent.gameObject;
        CloseTooltip();
        maxHovering = PlayerPrefs.GetFloat("TimeTillOpenTooltip");
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering>maxHovering && !isOpened)
        {
            OpenTooltip();
        }
        else if (isOpened && isHovering<maxHovering)
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
