using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class SelectionIndicator : MonoBehaviour
{
    Image myImage;
    [SerializeField] Sprite basic;
    [SerializeField] Sprite hero;

    void Start()
    {
        myImage = GetComponentInChildren<Image>();
    }
    
    public void SetActiveFor(Unit unit)
    {
        if (unit is Hero)
        {
            this.transform.position = unit.transform.position + new Vector3(0, 0.1f, 0);
            myImage.sprite = hero;
        }
        else
        {
            this.transform.position = unit.transform.position;
            myImage.sprite = basic;
        }
        this.transform.parent = unit.transform;
    }

    public void SetInactive()
    {
        this.transform.position = new Vector3(100, 100, 100);
        this.transform.parent = null;
    }



}
