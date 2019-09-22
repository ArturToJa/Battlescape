using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindow : MonoBehaviour
{

    [SerializeField] GameObject window;

        public void OpenTheWindow()
    {
        StartCoroutine(Opening());
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
