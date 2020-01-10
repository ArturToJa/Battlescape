using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattlescapeLogic;

public class UnitHealth : TurnChangeMonoBehaviour
{
    public Sprite Background;
    public Sprite RedFill;
    public Sprite GreenFill;
    BattlescapeLogic.Unit thisUnit;
    [SerializeField] Image fillOfABar;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] float barAnimationTime = 0.1f;

    void Awake()
    {
        SetStuff();
        TurnOffHealthbars();
    }
    void Update()
    {
        UpdateText();
        FillTheBar();
    }



    void SetStuff()
    {
        Canvas canvas = transform.root.GetComponentInChildren<Canvas>();
        foreach (Image child in canvas.GetComponentsInChildren<Image>())
        {
            child.raycastTarget = false;
        }
        thisUnit = this.transform.root.GetComponent<Unit>();
        fillOfABar = GetComponentsInChildren<Image>()[1];
        amount = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = Background;
    }

    void UpdateText()
    {
        amount.text = thisUnit.statistics.healthPoints + "/" + thisUnit.statistics.maxHealthPoints;
    }

    void FillTheBar()
    {
        float velocity = 0;
        fillOfABar.fillAmount = Mathf.SmoothDamp(fillOfABar.fillAmount, ((float)thisUnit.statistics.healthPoints / (float)thisUnit.statistics.maxHealthPoints), ref velocity, barAnimationTime);
    }

    public static void TurnOffHealthbars()
    {
        foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
        {
            UIManager.InstantlyTransitionActivity(unit.gameObject, false);
        }
    }
    void TurnOn()
    {
        UIManager.InstantlyTransitionActivity(gameObject, true);
    }
    void SetColour()
    {
        if (thisUnit == null)
        {
            SetStuff();
        }
        if (thisUnit.owner.IsCurrentLocalPlayer())
        {
            fillOfABar.sprite = GreenFill;
        }
        else
        {
            fillOfABar.sprite = RedFill;
        }        
    }

    public override void OnNewRound()
    {
        TurnOn();
    }

    public override void OnNewTurn()
    {
        SetColour();
    }

    public override void OnNewPhase()
    {
        return;
    }
}
