using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupDmgScript : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI damageText;

    public void Execute()
    {
        StartCoroutine(ExeCoroutine());        
    }

    public void SetText(string text)
    {
        damageText = animator.GetComponent<TextMeshProUGUI>();
        damageText.text = text;
    }

    IEnumerator ExeCoroutine()
    {
        animator.SetTrigger("Animate");
        yield return null;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(this.gameObject, clipInfo[0].clip.length);

    }
}
