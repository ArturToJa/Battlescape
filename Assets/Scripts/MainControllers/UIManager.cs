﻿using System;
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
    public GameObject AbilitiesPanel;
    public GameObject AbilityPrefab;
    [SerializeField] GameObject CancelAbilityButton;

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
        UpdateAbilitiesEnabled();
        if (MouseManager.instance.selectedUnit != null)
        {
            FillTheBar();
        }
    }

    public static void UpdateAbilitiesPanel(GameObject Panel, GameObject Prefab, BattlescapeLogic.Unit Unit)
    {
        RemoveChildrenOf(Panel.transform);
        foreach (Ability_Basic ability in Unit.GetComponents<Ability_Basic>())
        {
            ability.MyObject = Instantiate(Prefab, Panel.transform);
            ability.MyObject.GetComponentInChildren<AbilityIconScript>().myAbility = ability;
            ability.MyObject.GetComponentInChildren<AbilityIconScript>().myImage.sprite = ability.mySprite;
            ability.MyObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { ability.BaseUse(); });

        }
    }

    void UpdateAbilitiesEnabled()
    {
        if (MouseManager.instance.selectedUnit == null)
        {
            return;
        }
        foreach (Ability_Basic ability in MouseManager.instance.selectedUnit.GetComponents<Ability_Basic>())
        {
            if (ability.MyObject != null)
            {
                ability.MyObject.GetComponentInChildren<Button>().interactable = (ability.IsUsableNowBase() && MouseManager.instance.selectedUnit.movement.isMoving == false);
            }
        }
        SmoothlyTransitionActivity(CancelAbilityButton, Ability_Basic.currentlyUsedAbility != null, 0.1f);
    }

    static void RemoveChildrenOf(Transform panel)
    {
        int temp = panel.childCount;
        for (int i = 0; i < temp; i++)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
    }

    void UpdateSelectedUnit()
    {
        if (Global.instance.IsCurrentPlayerAI() == false)
        {
            unitWeShowStatsOf = MouseManager.instance.selectedUnit;
        }
        if (Global.instance.IsCurrentPlayerAI() == true)
        {
            SetOurOwnUnitSelection();
        }
    }

    void SetOurOwnUnitSelection()
    {
        //THIS should be done via PlayerInput xD
        //if (Input.GetMouseButton(0) && MouseManager.Instance.MouseoveredUnit != null)
        //{
        //    if (MouseManager.Instance.MouseoveredUnit != unitWeShowStatsOf)
        //    {
        //        unitWeShowStatsOf = MouseManager.Instance.MouseoveredUnit;
        //    }
        //    else
        //    {
        //        unitWeShowStatsOf = null;
        //    }
        //}
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
        //SmoothlyTransitionActivity(MovementPanel, GameStateManager.Instance.GameState == GameStates.MoveState, 0.1f);
    }

    public void ToggleShowingStats()
    {
        selectedUnitStats.SetActive(!selectedUnitStats.activeSelf);
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

    public void CancelAbility()
    {
        Ability_Basic.currentlyUsedAbility.BaseCancelUse();
    }

    void FillTheBar()
    {
        Unit unit = MouseManager.instance.selectedUnit;
        float velocity = 0;
        fillOfABar.fillAmount = Mathf.SmoothDamp(fillOfABar.fillAmount, ((float)unit.statistics.currentEnergy / (float)Statistics.maxEnergy), ref velocity, barAnimationTime);
        amount.text = "Energy: " + unit.statistics.currentEnergy.ToString() + "/" + Statistics.maxEnergy.ToString();
    }
}
