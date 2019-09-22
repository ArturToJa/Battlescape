using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTooltipHandler : MonoBehaviour
{
    [SerializeField] Text Title;

    public static bool isOn;

    void LateUpdate()
    {
        CheckForInput();

        if (!isOn)
        {
            UpdateTitle();
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
        //here
        Helper.CheckIfInBoundries(transform);
    }

    private void UpdateTitle()
    {
        if (MouseManager.Instance.MouseoveredUnit != null)
        {
            Title.text = MouseManager.Instance.MouseoveredUnit.name;
        }
    }

    private void CheckForInput()
    {
        if (MouseManager.Instance.MouseoveredUnit != null && Input.GetMouseButtonDown(1))
        {
            isOn = true;
        }
        if (isOn == true && Input.GetKeyDown(KeyCode.Escape))
        {
            isOn = false;
        }
        UIManager.SmoothlyTransitionActivity(this.gameObject, isOn, 0.05f);
    }
    public void TurnOff()
    {
        isOn = false;
    }
}
