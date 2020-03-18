using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadWindowButtons : MonoBehaviour
{

    [SerializeField] Button confirmButton;
    [SerializeField] Button backButton;
    [SerializeField] Button deleteButton;
    [SerializeField] Transform existingSaves;
    [SerializeField] GameObject savePrefab;
    public bool canDelete { get; set; }

    void Start()
    {
        SaveLoadManager.instance.ReCreateSaves(existingSaves);       
        backButton.onClick.AddListener(Back);
    }

    void Update()
    {
        UIManager.SmoothlyTransitionActivity(confirmButton.gameObject, (string.IsNullOrEmpty(SaveLoadManager.instance.currentSaveName) == false), 0.01f);
        UIManager.SmoothlyTransitionActivity(deleteButton.gameObject, (string.IsNullOrEmpty(SaveLoadManager.instance.currentSaveName) == false && canDelete), 0.01f);
    }
    public void Back()
    {
        SaveLoadManager.instance.currentSaveName = null;
        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
    }

    public void OnConfirmInGame()
    {
        SaveLoadManager.instance.LoadArmyToGame();
    }
}
