using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour
{

    [SerializeField] CanvasGroup cg;
    [SerializeField] bool hasMum;

    public void CloseMe()
    {
        StartCoroutine(Closing());
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }        
    }

    IEnumerator Closing()
    {
        if (hasMum == false)
        {
            while (this.transform.parent.GetComponent<CanvasGroup>().alpha > 0.01f)
            {
                UIManager.SmoothlyTransitionActivity(this.transform.parent.gameObject, false, 0.1f);
                yield return null;
            }
            this.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
        }
        else
        {
            while (this.transform.parent.parent.GetComponent<CanvasGroup>().alpha > 0.01f)
            {
                UIManager.SmoothlyTransitionActivity(this.transform.parent.parent.gameObject, false, 0.1f);
                yield return null;
            }
            this.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
        }
        
        
    }
}
