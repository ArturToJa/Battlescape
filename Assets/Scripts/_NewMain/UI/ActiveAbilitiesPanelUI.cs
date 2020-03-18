using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

namespace BattlescapeUI
{
    public class ActiveAbilitiesPanelUI : MonoBehaviour
    {

        [SerializeField] bool _isRightClickTooltip;
        public bool isRightClickTooltip
        {
            get
            {
                return _isRightClickTooltip;
            }
            set
            {
                _isRightClickTooltip = value;
            }
        }

        [SerializeField] GameObject abilityIconPrefab;
        [SerializeField] GameObject cancelButton;

        //THESE two and their functions probably dont belong here idk
        [SerializeField] Sprite unclickedFrameAbility;
        [SerializeField] Sprite clickedFrameAbility;

        public Unit myUnit { get; private set; }

        Dictionary<AbstractActiveAbility, GameObject> iconDictionary;

        private void Start()
        {
            if (isRightClickTooltip == false)
            {
                Unit.OnUnitSelected += SetAbilitiesInPanel;
                AbstractActiveAbility.OnAbilityClicked += SetAbilityFrame;
                AbstractActiveAbility.OnAbilityClicked += OnAbilityChosen;
            }
            else
            {
                EnemyTooltipHandler.instance.OnRightclickTooltipOn += SetAbilitiesInPanel;
            }

        }

        private void Update()
        {
            
            if (isRightClickTooltip == false)
            {
                if (myUnit != null)
                {
                    UpdateActivity();
                }                
            }
        }

        public void OnAbilityChosen()
        {
            cancelButton.SetActive(true);
        }






        void SetAbilitiesInPanel(Unit unit)
        {
            myUnit = unit;
            iconDictionary = new Dictionary<AbstractActiveAbility, GameObject>();
            foreach (Transform child in transform)
            {
                child.name = "RemovedAbility";
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
            foreach (AbstractActiveAbility ability in unit.abilities)
            {
                AddAbility(ability);
            }
        }

        void AddAbility(AbstractActiveAbility ability)
        {
            GameObject abilityIcon = Instantiate(abilityIconPrefab, this.transform);
            iconDictionary.Add(ability, abilityIcon);
            abilityIcon.transform.GetChild(0).GetComponent<Image>().sprite = ability.icon;
            abilityIcon.name = ability.abilityName;
            if (ability.usesPerBattle > 0)
            {
                abilityIcon.transform.GetChild(1).GetComponent<Text>().text = ability.usesLeft.ToString();
            }

            abilityIcon.transform.GetChild(2).GetComponent<Text>().text = ability.energyCost.ToString();
            MouseHoverInfoCursor iconInfo = abilityIcon.GetComponentInChildren<MouseHoverInfoCursor>();
            if (isRightClickTooltip == false)
            {
                var iconInfoTemp = iconInfo as MouseHoverAbilityIconCursor;
                iconInfoTemp.myAbility = ability;
            }                        
            
            iconInfo.tooltipName = ability.abilityName;
            iconInfo.tooltipText = ability.description;
            if (isRightClickTooltip == false)
            {
                abilityIcon.GetComponentInChildren<Button>().onClick.AddListener(ability.OnClickIcon);
            }
        }

        //Basically - sets every icon to its ability's actual 'isUsableNow' condition (active if yes)
        void UpdateActivity()
        {
            foreach (var thing in iconDictionary)
            {
                thing.Value.GetComponentInChildren<Button>().interactable = (thing.Key.IsUsableNow());
            }
        }

        public void CancelAbility()
        {
            Global.instance.currentEntity = GameRound.instance.currentPlayer;
        }

        void SetAbilityFrame()
        {
            foreach (var thing in iconDictionary)
            {
                if (thing.Key == (Global.instance.currentEntity as AbstractActiveAbility))
                {
                    thing.Value.GetComponent<Image>().sprite = clickedFrameAbility;
                }
                else
                {
                    thing.Value.GetComponent<Image>().sprite = unclickedFrameAbility;
                }
                //This happens, among others, on ability use, so ability with limited use needs to change text now ;)
                if (thing.Key.usesPerBattle > 0)
                {
                    thing.Value.transform.GetChild(1).GetComponent<Text>().text = thing.Key.usesLeft.ToString();
                }
            }
        }
    }
}