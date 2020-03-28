using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattlescapeUI
{
    public class UnitHealthbar : AbstractHealthbar
    {
        BattlescapeLogic.Unit myUnit;
        [SerializeField] List<Sprite> barColours;

        TextMeshProUGUI healthText; 

        protected override void OnStart()
        {
            healthBarFill = GetComponentsInChildren<Image>()[1];
            myUnit = GetComponentInParent<BattlescapeLogic.Unit>();
            myObject = myUnit.transform;
            healthText = GetComponentInChildren<TextMeshProUGUI>();
            SetColour();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            UpdateText();
        }

        protected override float GetPercent()
        {
            return (float)myUnit.statistics.healthPoints / (float)myUnit.statistics.maxHealthPoints;
        }

        void UpdateText()
        {
            healthText.text = myUnit.statistics.healthPoints + "/" + myUnit.statistics.maxHealthPoints;
        }

        void SetColour()
        {
            //Debug.Log(healthBarFill);
            //Debug.Log(healthBarFill.sprite);
            //Debug.Log(barColours);
            //Debug.Log(myUnit);
            //Debug.Log(myUnit.GetMyOwner());
            //Debug.Log(myUnit.GetMyOwner().colour);
            healthBarFill.sprite = barColours[(int)myUnit.GetMyOwner().colour];
        }
    }
}