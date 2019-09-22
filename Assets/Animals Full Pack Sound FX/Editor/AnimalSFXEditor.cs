using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
[CustomEditor(typeof(AnimalSFX))]
public class AnimalSFXEditor : Editor {

	private AnimalSFX _component;

	public void OnEnable() {
		_component = (AnimalSFX) target;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(!Application.isPlaying);
		if (GUILayout.Button(EditorGUIUtility.FindTexture("PlayButton"))) {
			_component.Play();
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.Space();
	}
}

public class AnimalSFXPlayerEditor {

	[MenuItem("GameObject/Animal SFX Player", false, 15)]
	private static void NewMenuOption() {
		GameObject animalSFXPlayer = new GameObject ("Animal SFX Player");
		animalSFXPlayer.AddComponent<AnimalSFXPlayer> ();

		string localPath = "Assets/Animals Full Pack Sound FX/Prefabs/AnimalSFXAudioClips.prefab";
		GameObject animalSFXAudioClipsPrefab = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) as GameObject;
		if (animalSFXAudioClipsPrefab != null) {
			PrefabUtility.InstantiatePrefab(animalSFXAudioClipsPrefab);
		} else {
			Debug.LogWarning("Make sure that AnimalSFXAudioClips prefab is found in the following path <b>" + localPath + "</b> then try again!");
		}
	}
}
