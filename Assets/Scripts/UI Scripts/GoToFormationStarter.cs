using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToFormationStarter : MonoBehaviour
{/*
    [SerializeField] Transform DeploymentPanel;
    [SerializeField] ArmyBuilder ab;
    [SerializeField] 
    public void GoTo()
    {
        SaveLoadManager.Instance.Save();
        FindObjectOfType<SaveLoadManager>().CopyDataToSave();
        CreateAllUnits();
        SaveLoadManager.Instance.LoadPlayerArmy();
        SaveLoadManager.Instance.RecreateUnitsList();
        if (SaveLoadManager.unitPositions != null && SaveLoadManager.unitPositions.Keys.Count != 0)
        {
            LoadFormation(SaveLoadManager.unitPositions);
        }
    }
    void CreateAllUnits()
    {
        foreach (Unit theUnit in SaveLoadManager.Instance.UnitsList)
        {
            CreateUnit(theUnit.thisBox, theUnit.thisSprite, theUnit);
        }
        CreateHero(ArmyBuilder.Instance.Hero.thisBox, ArmyBuilder.Instance.Hero.thisRealSprite, ArmyBuilder.Instance.Hero);
    }
    void CreateUnit(GameObject Unit, RenderTexture _sprite, Unit _me)
    {
        var temp = Instantiate(Unit, DeploymentPanel);
        temp.GetComponentInChildren<RawImage>().texture = _sprite;
        temp.GetComponent<DragableUnitIcon>().me = _me;
    }
    void CreateHero(GameObject Unit, Sprite _sprite, Unit _me)
    {
        var temp = Instantiate(Unit, DeploymentPanel);
        temp.GetComponentInChildren<RawImage>().gameObject.SetActive(false);
        temp.GetComponentsInChildren<Image>(true)[1].gameObject.SetActive(true);
        temp.GetComponentsInChildren<Image>()[1].sprite = _sprite;
        temp.GetComponent<DragableUnitIcon>().me = _me;        
    }
    void LoadFormation(Dictionary<LemurVector2,Unit> unitPositions)
    {
        foreach (var pair in unitPositions)
        {
            Debug.Log(pair.Value);
            DropPlaceFormation.DPFBoard[pair.Key.x, pair.Key.z].DropIt(GetUnitsObject(pair.Value));
        }
    }
    GameObject GetUnitsObject(Unit unit)
    {
        foreach (Transform t in DeploymentPanel)
        {
            if (t.GetComponent<DragableUnitIcon>().me == unit)
            {
                return t.gameObject;
            }
        }
        Debug.LogError("Can not find such object!");
        return null;
    }*/
}
