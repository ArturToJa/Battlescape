using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    public bool isDeletable = false;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SaveLoadManager.instance.currentSaveName = this.gameObject.name;
        FindObjectOfType<LoadWindowButtons>().canDelete = isDeletable;
    }
}
