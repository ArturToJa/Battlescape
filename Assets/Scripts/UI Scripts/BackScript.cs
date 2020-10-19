using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

public class BackScript : MonoBehaviour
{

    GameObject Parent;
    [SerializeField]
    GameObject objectOn;

    void Start()
    {
        Parent = GameObject.FindGameObjectWithTag("MainMenuScreens");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LevelLoader.isLoading == false)
        {
            Back();
        }
    }

    public void Back()
    {
        foreach (Transform child in Parent.transform)
        {
            child.gameObject.SetActive(false);
        }

        objectOn.SetActive(true);
    }
}
