using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWindow : MonoBehaviour
{

    [SerializeField] GameObject window;
    [SerializeField] Transform ExiSaves;
    [SerializeField] GameObject savePrefab;

    public void OpenTheWindow()
    {
        StartCoroutine(Opening());
        SaveLoadManager.instance.ReCreateSaves(ExiSaves);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.GetComponent<CanvasGroup>().interactable = false;
    }

    IEnumerator Opening()
    {
        while (window.GetComponent<CanvasGroup>().alpha < 0.95)
        {
            UIManager.SmoothlyTransitionActivity(window, true, 0.05f);
            yield return null;
        }
    }

    
}
