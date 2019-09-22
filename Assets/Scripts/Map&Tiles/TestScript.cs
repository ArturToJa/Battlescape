using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    bool switcher;
    Renderer r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && Application.isEditor)
        {
            switcher = !switcher;
        }
        r.enabled = switcher;
	}
}
