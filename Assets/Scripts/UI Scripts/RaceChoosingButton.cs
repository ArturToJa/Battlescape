using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceChoosingButton : MonoBehaviour {

    [SerializeField] string FactionName;
    [SerializeField] string FactionDescription;


    public void OnHover()
    {
        if (ArmyBuilder.Instance == null)
        {
            RaceChoosingManager.Instance.FactionDescriptionText.text = FactionDescription;
            RaceChoosingManager.Instance.FactionNameText.text = FactionName;
        }
        else
        {
            ArmyBuilder.Instance.FactionDescriptionText.text = FactionDescription;
            ArmyBuilder.Instance.FactionNameText.text = FactionName;
        }
        
    }    
}
