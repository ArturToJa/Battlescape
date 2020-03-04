using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UnitPanel;
    [SerializeField] GameObject MovementPanel;
    [SerializeField] GameObject selectedUnitStats;
    [SerializeField] Text unitName;
    BattlescapeLogic.Unit unitWeShowStatsOf;

    [SerializeField] GameObject EnergyBar;
    [SerializeField] Image fillOfABar;
    [SerializeField] Text amount;
    [SerializeField] float barAnimationTime = 0.1f;

    public static UIManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        UpdateSelectedUnit();
        SetPanelsActivity();
        SetUnitName();
        if (MouseManager.instance.selectedUnit != null)
        {
            FillTheBar();
        }
    } 
    

    void UpdateSelectedUnit()
    {       
        if (GameRound.instance.currentPlayer != null && GameRound.instance.currentPlayer.type == PlayerType.AI)
        {
           //??
        }
        else
        {
            unitWeShowStatsOf = MouseManager.instance.selectedUnit;
        }
    }

    

    private void SetUnitName()
    {
        if (unitWeShowStatsOf != null)
        {
            unitName.text = unitWeShowStatsOf.name;
        }
    }

    private void SetPanelsActivity()
    {
        SmoothlyTransitionActivity(UnitPanel, unitWeShowStatsOf != null, 0.1f);
    }

    public static void SmoothlyTransitionActivity(GameObject UIElement, bool active, float time)
    {
        if (UIElement.GetComponent<CanvasGroup>() == null)
        {
            UIElement.AddComponent<CanvasGroup>();
        }
        if (active)
        {
            float Alpha = UIElement.transform.GetComponent<CanvasGroup>().alpha;
            float velocity = 0;

            Alpha = Mathf.SmoothDamp(Alpha, 1f, ref velocity, time);
            UIElement.transform.GetComponent<CanvasGroup>().alpha = Alpha;
        }
        else
        {
            float Alpha = UIElement.transform.GetComponent<CanvasGroup>().alpha;
            float velocity = 0;

            Alpha = Mathf.SmoothDamp(Alpha, 0f, ref velocity, 0.5f * time);
            UIElement.transform.GetComponent<CanvasGroup>().alpha = Alpha;
        }
        SetPanelsInteraction(UIElement);
    }

    public static void InstantlyTransitionActivity(GameObject UIElement, bool active)
    {
        if (UIElement.GetComponent<CanvasGroup>() == null)
        {
            UIElement.AddComponent<CanvasGroup>();
        }
        if (active)
        {
            UIElement.transform.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            UIElement.transform.GetComponent<CanvasGroup>().alpha = 0;
        }
        SetPanelsInteraction(UIElement);

    }
    static void SetPanelsInteraction(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().interactable = panel.GetComponent<CanvasGroup>().alpha > 0.9f;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = panel.GetComponent<CanvasGroup>().alpha > 0.9f;
    }

    void FillTheBar()
    {
        Unit unit = MouseManager.instance.selectedUnit;
        float velocity = 0;
        fillOfABar.fillAmount = Mathf.SmoothDamp(fillOfABar.fillAmount, ((float)unit.statistics.currentEnergy / (float)Statistics.maxEnergy), ref velocity, barAnimationTime);
        amount.text = "Energy: " + unit.statistics.currentEnergy.ToString() + "/" + Statistics.maxEnergy.ToString();
    }
}
