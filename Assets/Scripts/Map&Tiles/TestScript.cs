using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    bool isOn = false;
    Renderer r;
    // Use this for initialization
    void Start()
    {
        r = GetComponent<Renderer>();
        r.enabled = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Application.isEditor)
        {
            isOn = !isOn;
        }
        r.enabled = isOn;
    }
}
