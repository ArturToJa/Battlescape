using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

namespace BattlescapeUI
{
    public class BuffsPanelUI : MonoBehaviour
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

        Dictionary<string, int> alreadyRepresentedBuffs;

        [SerializeField] GameObject buffPrefab;

        public Unit myUnit { get; private set; }

        void Start()
        {
            AbstractBuff.OnBuffDestruction += OnBuffCreatedOrDestroyed;
            AbstractBuff.OnBuffCreation += OnBuffCreatedOrDestroyed;
            if (isRightClickTooltip == false)
            {
                MouseManager.instance.OnUnitSelection += UpdateBuffPanel;
            }
            else
            {
                EnemyTooltipHandler.instance.OnRightclickTooltipOn += UpdateBuffPanel;
            }
        }

        void UpdateBuffPanel(Unit unit)
        {
            myUnit = unit;
            foreach (Transform child in transform)
            {
                child.name = "RemovedBuff";
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
            alreadyRepresentedBuffs = new Dictionary<string, int>();
            foreach (AbstractBuff buff in unit.buffs)
            {
                AddBuff(buff);
            }
            
        }

        void AddBuff(AbstractBuff buff)
        {
            if (alreadyRepresentedBuffs.ContainsKey(buff.buffName))
            {
                alreadyRepresentedBuffs[buff.buffName]++;
                GameObject buffIcon = transform.Find(buff.buffName).gameObject;
                buffIcon.transform.GetChild(0).GetComponent<Text>().text = "x" + alreadyRepresentedBuffs[buff.buffName];
            }
            else
            {
                alreadyRepresentedBuffs.Add(buff.buffName, 1);
                GameObject buffIcon = Instantiate(buffPrefab, this.transform);
                buffIcon.GetComponent<Image>().sprite = buff.icon;
                buffIcon.name = buff.buffName;
                MouseHoverInfoCursor hoverInfo = buffIcon.GetComponentInChildren<MouseHoverInfoCursor>();
                hoverInfo.tooltipName = buff.buffName;
                hoverInfo.tooltipText = buff.description;
            }
        }

        void OnBuffCreatedOrDestroyed(AbstractBuff buff)
        {
            Unit owner = buff.buffGroup.owner as Unit;
            if (owner != null && owner == myUnit)
            {
                UpdateBuffPanel(myUnit);
            }
        }       
    }
}