using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;


namespace BattlescapeLogic
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] Slider slider;
        public static bool isLoading = false;



        void Start()
        {
            SceneManager.sceneLoaded += SceneIsLoaded;
        }        

        public void LoadScene(string scene)
        {
            AudioManager.isPlayingGameOverMusic = false;
            StartCoroutine(LoadAsync(scene));
        }


        IEnumerator LoadAsync(string scene)
        {
            isLoading = true;
            GameObject.Find("DisablingClicksPanel").GetComponent<Image>().raycastTarget = true;

            if (IsChangingSceneType(scene, SceneManager.GetActiveScene().name) && AudioManager.Instance != null && AudioManager.Instance.currentMusic != null)
            {
                AudioManager.Instance.StartCoroutine(AudioManager.Instance.SwellDown(AudioManager.Instance.currentMusic));
            }
            slider.gameObject.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            if (scene == "_MENU")
            {
                Destroy(BattlescapeLogic.Networking.instance.gameObject);
            }
            while (operation.isDone == false)
            {

                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }


        }

        bool IsChangingSceneType(string oldScene, string newScene)
        {
            if (oldScene.Contains("_GameScene_") || newScene.Contains("_GameScene_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void SceneIsLoaded(Scene scene, LoadSceneMode mode)
        {
            isLoading = false;
        }
    }
}
