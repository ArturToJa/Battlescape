using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(AudioSource))]
public class AnimalSFXPlayer : MonoBehaviour {

	/// <summary>
	/// Shared instance of the AnimalSFXPlayer script.
	/// </summary>
	public static AnimalSFXPlayer instance;

	private AudioSource audioSource;

	#region Unity Callbacks

	void Awake () {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		audioSource = GetComponent<AudioSource>();
	}

	#endregion


	#region Public API

	/// <summary>
	/// Plays the selected animal SFX.
	/// </summary>
	/// <param name="animal">Target animal for the SFX</param>
	public void PlayAnimalSFX (Animal animal) {
		StartCoroutine (PlayAnimalSFXCoroutine (AnimalSFXAudioClips.instance.GetAudioClip (animal, Revision.Random), 1));
	}

	/// <summary>
	/// Plays the selected animal SFX.
	/// </summary>
	/// <param name="animal">Target animal for the SFX</param>
	/// <param name="repeatCount">Number of times the SFX will be repeated</param>
	public void PlayAnimalSFX (Animal animal, int repeatCount = 1) {
		StartCoroutine (PlayAnimalSFXCoroutine (AnimalSFXAudioClips.instance.GetAudioClip (animal, Revision.Random), repeatCount));
	}

	/// <summary>
	/// Plays the selected animal SFX.
	/// </summary>
	/// <param name="animal">Target animal for the SFX</param>
	/// <param name="revision">Target revision for the SFX</param>
	/// <param name="repeatCount">Number of times the SFX will be repeated</param>
	public void PlayAnimalSFX (Animal animal, Revision revison, int repeatCount = 1) {
		StartCoroutine (PlayAnimalSFXCoroutine (AnimalSFXAudioClips.instance.GetAudioClip (animal, revison), repeatCount));
	}

	#endregion


	#region Helpers

	IEnumerator PlayAnimalSFXCoroutine (AudioClip clip, int repeatCount) {

		audioSource.clip = clip;

		for (int i = 0; i < repeatCount; i++) {
			audioSource.Play ();
			yield return new WaitForSeconds(audioSource.clip.length);
		}

		yield break;
	}

	#endregion
}