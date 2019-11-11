using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField] Sprite basic;
    [SerializeField] Sprite hero;

    void Update()
    {
        if (MouseManager.Instance.SelectedUnit != null)
        {
            this.transform.position = MouseManager.Instance.SelectedUnit.transform.position;
            if (MouseManager.Instance.SelectedUnit is Hero)
            {
                this.transform.position = MouseManager.Instance.SelectedUnit.transform.position +new Vector3(0, 0.01f, 0);
                GetComponentInChildren<Image>().sprite = hero;
            }
            else
            {
                GetComponentInChildren<Image>().sprite = basic;
            }
        }
        else
        {
            this.transform.position = new Vector3(100, 100, 100);
        }
    }
}
