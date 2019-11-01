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
    UnitScript thisUnit;
    [SerializeField] Image fillOfABar;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] float barAnimationTime = 0.1f;

    void Awake()
    {
        SetStuff();      
    }
    void Update()
    {
        UpdateText();
        FillTheBar();
    }

    void SetStuff()
    {
        thisUnit = this.transform.root.GetComponent<UnitScript>();
        fillOfABar = GetComponentsInChildren<Image>()[1];
        amount = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Image>().sprite = Background;
    }

    void UpdateText()
    {
        amount.text = thisUnit.CurrentHP + "/" + thisUnit.MaxHP;
    }

    void FillTheBar()
    {
        float velocity = 0;
        fillOfABar.fillAmount = Mathf.SmoothDamp(fillOfABar.fillAmount, ((float)thisUnit.CurrentHP / (float)thisUnit.MaxHP), ref velocity, barAnimationTime);
    }
   public static void SetColour()
    {
        switch (GameStateManager.Instance.MatchType)
        {
            case MatchTypes.Online:
                foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
                {
                    if (unit.thisUnit == null)
                    {
                        unit.SetStuff();
                    }
                    if (Global.instance.playerTeams[unit.thisUnit.PlayerID].players[0].type == PlayerType.Local)
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
                    if (TurnManager.Instance.PlayerToMove == unit.thisUnit.PlayerID)
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
                    if (Global.instance.playerTeams[unit.thisUnit.PlayerID].players[0].type == PlayerType.Local)
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
