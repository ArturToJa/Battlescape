using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleHighlightsUI : MonoBehaviour
{
    Text thisText;
    Toggle thisToggle;
    // Use this for initialization
    void Start()
    {
        thisText = GetComponentInChildren<Text>();
        thisToggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisToggle.isOn)
        {
            thisText.text = "Hide Unmoved";
        }
        else
        {
            thisText.text = "Show Unmoved";
        }
    }

}
