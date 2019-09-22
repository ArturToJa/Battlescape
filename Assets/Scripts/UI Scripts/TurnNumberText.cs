using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnNumberText : MonoBehaviour
{

    public Text TurnMumber;
    [SerializeField] float speed = 1;
    bool isRaisingColour = true;
    public bool isOff;

    void Update()
    {
        if (isOff)
        {
            return;
        }
        if (TurnManager.Instance.TurnCount > 16)
        {
            TurnMumber.text = "Time is up!";
            TurnMumber.color = Color.yellow;
            return;
        }
        if (TurnManager.Instance.TurnCount <= 0)
        {
            TurnMumber.text = "Drag units to position them!";
            return;
        }
        if (TurnManager.Instance.isEndgameTrue)
        {
            if (TurnManager.Instance.TurnCount == 16)
            {
                TurnMumber.text = "Final Turn";
            }
            else
            {
                TurnMumber.text = "Turns to end: " + (16 - TurnManager.Instance.TurnCount).ToString();
            }        
            ColourChange();
        }
        else
        {
            TurnMumber.text = "Turn Number: " + TurnManager.Instance.TurnCount.ToString() + "/16";
        }
    }

    void ColourChange()
    {
        if (TurnMumber.color.g >= 0.9f && isRaisingColour)
        {
            isRaisingColour = false;
        }
        else if (TurnMumber.color.g <= 0.1f && isRaisingColour == false)
        {
            isRaisingColour = true;
        }
        float g;
        float b;
        if (isRaisingColour)
        {
            b = TurnMumber.color.b + (Time.deltaTime * speed);
            g = TurnMumber.color.g + (Time.deltaTime * speed);
        }
        else
        {
            b = TurnMumber.color.b - (Time.deltaTime * speed);
            g = TurnMumber.color.g - (Time.deltaTime * speed);
        }
        TurnMumber.color = new Color(TurnMumber.color.r, g, b, TurnMumber.color.a);

    }

    public void ResetColour()
    {
        TurnMumber.color = Color.white;
    }
}
