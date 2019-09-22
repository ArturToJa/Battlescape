using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteSaves : MonoBehaviour
{
    [SerializeField] GameObject Window;
    [SerializeField] Transform ExistingSaves;
    public void CloseWindow()
    {
        StartCoroutine(Closing());
    }

    IEnumerator Closing()
    {
        while (Window.GetComponent<CanvasGroup>().alpha > 0.1f)
        {
            UIManager.SmoothlyTransitionActivity(Window, false, 0.1f);
        }
        yield return null;
        Window.GetComponent<CanvasGroup>().alpha = 0;
    }

    IEnumerator Opening()
    {
        while (Window.GetComponent<CanvasGroup>().alpha < 0.9f)
        {
            UIManager.SmoothlyTransitionActivity(Window, true, 0.1f);
        }
        yield return null;
        Window.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void Delete()
    {
        if (FindObjectOfType<LoadWindowButtons>().canDelete == false)
        {
            return;
        }
        StartCoroutine(Closing());
        File.Delete(Application.persistentDataPath + "/Armies/" + SaveLoadManager.Instance.currentSaveValue.ToString() + "points/" + SaveLoadManager.Instance.currentSaveName + ".lemur");
        foreach (Transform child in ExistingSaves)
        {
            if (child.gameObject.name == SaveLoadManager.Instance.currentSaveName)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }
        SaveLoadManager.Instance.currentSaveName = null;
    }

    public void OpenWindow()
    {
        StartCoroutine(Opening());
    }
}
