using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnimalSFXAudioClips : MonoBehaviour {

	/// <summary>
	/// Shared instance of the AnimalSFXAudioClips script.
	/// </summary>
	public static AnimalSFXAudioClips instance;

	[SerializeField]
	private AnimalMultipleSFX[] animalsMultipleSFX;

	#region Unity Callbacks

	void Awake () {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	#endregion


	#region Helpers

	/// <summary>
	/// Returns the appropriate AudioClip according to the passed arguments.
	/// </summary>
	/// <param name="animal">Target animal for the SFX AudioClip</param>
	/// <param name="revision">Target revision for the SFX AudioClip</param>
	/// <returns>AudioClip</returns>
	public AudioClip GetAudioClip (Animal animal, Revision revison) {
		AnimalMultipleSFX animalMultipleSFX = animalsMultipleSFX [(int)animal];

		int clipIndex = Mathf.Min (animalMultipleSFX.animalSFXClips.Length - 1, (int)revison);
		if (revison == Revision.Random) {
			clipIndex = UnityEngine.Random.Range (0, animalMultipleSFX.animalSFXClips.Length);
		}
		return animalMultipleSFX.animalSFXClips [clipIndex];
	}

	#endregion

}

public enum Revision {
	First,
	Second,
	Third,
	Forth,
	Fifth,
	Sixth,
	Seventh,
	Eighth,
	Nineth,
	Random
}

public enum Animal {
	Crocodile,
	Elephant,
	Hippopotamus,
	Lion,
	Rhinoceros,
	Zebra,
	Crow,
	Duck,
	GoldenEagle,
	GreatHornedOwl,
	Pigeon,
	Seagull,
	Sparrow,
	Chicken,
	Cow,
	Goat,
	Horse,
	Pig,
	Rooster,
	Sheep,
	Turkey,
	Bear,
	Boar,
	Fox,
	Monkey,
	Rabbit,
	Stag,
	StandardWolf
}

[Serializable]
public class AnimalMultipleSFX {
	public Animal animal;
	public AudioClip[] animalSFXClips;
}
