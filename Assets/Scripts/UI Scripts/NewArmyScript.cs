using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewArmyScript : MonoBehaviour
{
    [SerializeField] Toggle the25toggle;
    [SerializeField] Toggle the50toggle;
    [SerializeField] Button OKbutton;
    [SerializeField] GameObject HeroChoicer;
    string tempName;

    void Update()
    {
        OKbutton.interactable = (tempName != null && tempName != "");    
    }

    public void OK()
    {
        if (the25toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 25;
        }
        if (the50toggle.isOn)
        {
            SaveLoadManager.Instance.currentSaveValue = 50;
        }
        SaveLoadManager.Instance.currentSaveName = tempName;
        SaveLoadManager.Instance.Save();
        HeroChoicer.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void InputText(string text)
    {
        tempName = text;
    }
}
