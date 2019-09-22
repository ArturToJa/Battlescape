using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierDisplay : MonoBehaviour
{
    [SerializeField] Text AttackText;
    [SerializeField] Text DefenceText;
    [SerializeField] GameObject Visual;
    [SerializeField] Sprite[] Sprites;
    BasicStatModifier myUnitModifier;

    // Use this for initialization
    void Start()
    {
        myUnitModifier = GetComponentInParent<UnitTypes>().myUnit.GetComponent<BasicStatModifier>();
        if (myUnitModifier.favouriteEnemy != UnitType.Null)
        {
            Visual.GetComponent<Image>().sprite = Sprites[(int)myUnitModifier.favouriteEnemy];
            GetComponent<MouseHoverInfoCursor>().TooltipName = myUnitModifier.favouriteEnemy.ToString();
            GetComponent<MouseHoverInfoCursor>().TooltipText = "This unit is stronger against " + myUnitModifier.favouriteEnemy.ToString();
        }
        AttackText.text = myUnitModifier.AttackModifierVersusUnitType.ToString();
        DefenceText.text = myUnitModifier.DefenceModifierVersusUnitType.ToString();
    }    
}
