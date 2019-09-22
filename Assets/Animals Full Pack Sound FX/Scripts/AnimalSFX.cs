using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Audio/Animal SFX")]
public class AnimalSFX : MonoBehaviour {

	[Space (10)]
	[SerializeField]
	[Tooltip("The selected animal of the SFX")]
	private Animal animal = Animal.Crocodile;
	[SerializeField]
	[Tooltip("The selected revision of the SFX")]
	private Revision revision = Revision.First;
	[SerializeField]
	[Tooltip("Number of times the SFX will be repeated")]
	private int repeatCount = 1;
	[SerializeField]
	[Tooltip("Determines if the SFX will be played on awake")]
	private bool playOnAwake = false;


	#region Unity Callbacks

	void Start () {
		if (playOnAwake) {
			Play ();
		}
	}

	#endregion


	#region Public API

	/// <summary>
	/// Plays the SFX of the selected animal and revision.
	/// </summary>
	public void Play () {
		AnimalSFXPlayer.instance.PlayAnimalSFX (animal, revision, repeatCount);
	}

	/// <summary>
	/// Changes the selected animal.
	/// </summary>
	/// <param name="animal">Target animal for the SFX</param>
	public void ChangeAnimal (Animal animal) {
		this.animal = animal;
	}

	/// <summary>
	/// Changes the selected revision.
	/// </summary>
	/// <param name="revision">Target revision for the SFX</param>
	public void ChangeRevision (Revision revision) {
		this.revision = revision;
	}

	/// <summary>
	/// Changes number of times the SFX will be repeated.
	/// </summary>
	/// <param name="repeatCount">Number of times the SFX will be repeated</param>
	public void ChangeRepeatCount (int repeatCount) {
		this.repeatCount = repeatCount;
	}

	#endregion

}
