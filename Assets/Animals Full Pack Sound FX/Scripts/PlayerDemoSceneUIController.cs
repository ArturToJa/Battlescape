using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemoSceneUIController : MonoBehaviour {

	public void OnLionButtonClicked () {
		AnimalSFXPlayer.instance.PlayAnimalSFX (Animal.Lion, Revision.First, 1);
	}

}
