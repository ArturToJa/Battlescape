using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadWindowButtons : MonoBehaviour {

    [SerializeField] GameObject Yes;
    [SerializeField] GameObject Choose;
    [SerializeField] GameObject BackButton;
    [SerializeField] GameObject Delete;
    [SerializeField] Transform ExiSaves;
    [SerializeField] GameObject savePrefab;
    public bool canDelete = false;

    private void Start()
    {
        SaveLoadManager.Instance.ReCreateSaves(ExiSaves);
        if (Yes != null)
        {
            Yes.GetComponent<Button>().onClick.AddListener(SaveLoadManager.Instance.LoadArmyToGame);
        }
        BackButton.GetComponent<Button>().onClick.AddListener(Back);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(SaveLoadManager.Instance.currentSaveName);
        }
        if (Yes != null)
        {
            UIManager.SmoothlyTransitionActivity(Yes, (string.IsNullOrEmpty(SaveLoadManager.Instance.currentSaveName) == false), 0.01f);

        }
        if (Choose != null)
        {
            UIManager.SmoothlyTransitionActivity(Choose, (string.IsNullOrEmpty(SaveLoadManager.Instance.currentSaveName) == false), 0.01f);

        }

        UIManager.SmoothlyTransitionActivity(Delete, (string.IsNullOrEmpty(SaveLoadManager.Instance.currentSaveName) == false && canDelete), 0.01f);
    }   
    public void Back()
    {
        SaveLoadManager.Instance.currentSaveName = null;
        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
    }
}
