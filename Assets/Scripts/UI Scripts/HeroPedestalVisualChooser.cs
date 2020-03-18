using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HeroPedestalVisualChooser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        switch (SaveLoadManager.instance.race)
        {
            case Race.Human:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case Race.Elves:
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case Race.Neutral:
                Debug.LogError("This hero is neutral and should probably not be: " + transform.root.position);
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
