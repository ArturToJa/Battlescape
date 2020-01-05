using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattlescapeLogic;

public class UnitHealth : MonoBehaviour
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
    public static void TurnOnHealthbars()
    {
        foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
        {
            UIManager.InstantlyTransitionActivity(unit.gameObject, true);
        }
    }
    public static void SetColour()
    {
        switch (Global.instance.MatchType)
        {
            case MatchTypes.Online:
                foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
                {
                    if (unit.thisUnit == null)
                    {
                        unit.SetStuff();
                    }
                    if (unit.thisUnit.owner.type == PlayerType.Local)
                    {
                        unit.fillOfABar.sprite = unit.GreenFill;
                    }
                    else
                    {
                        unit.fillOfABar.sprite = unit.RedFill;
                    }
                }
                break;
            case MatchTypes.HotSeat:
                foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
                {
                    if (unit.thisUnit == null)
                    {
                        unit.SetStuff();
                    }
                    if (GameRound.instance.currentPlayer == unit.thisUnit.owner)
                    {
                        unit.fillOfABar.sprite = unit.GreenFill;
                    }
                    else
                    {
                        unit.fillOfABar.sprite = unit.RedFill;
                    }
                }
                break;
            case MatchTypes.Singleplayer:
                foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
                {
                    if (unit.thisUnit == null)
                    {
                        unit.SetStuff();
                    }
                    if (unit.thisUnit.owner.type == PlayerType.Local)
                    {
                        unit.fillOfABar.sprite = unit.GreenFill;
                    }
                    else
                    {
                        unit.fillOfABar.sprite = unit.RedFill;
                    }
                }
                break;
            default:
                break;
        }


    }
}
