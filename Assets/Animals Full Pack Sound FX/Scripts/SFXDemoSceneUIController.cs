using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXDemoSceneUIController : MonoBehaviour {

	public GameObject lion;

	public void OnLionButtonClicked () {
		lion.GetComponent<AnimalSFX> ().Play ();
	}

}
