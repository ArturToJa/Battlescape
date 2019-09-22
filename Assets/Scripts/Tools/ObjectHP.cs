using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ObjectHP : MonoBehaviour
{

    public Sprite HPVisual;
    DestructibleScript thisObject;
    [SerializeField] Image fillOfABar;
    [SerializeField] float barAnimationTime = 0.1f;
    public bool isBeingAttacked = false;

    public void SwitchTrigger()
    {
        StartCoroutine(Switcherino());
    }

    IEnumerator Switcherino()
    {
        isBeingAttacked = true;
        yield return new WaitForSeconds(1f);
        isBeingAttacked = false;
    }

    void Start()
    {
        thisObject = this.transform.root.GetComponentInChildren<DestructibleScript>();
        fillOfABar = GetComponentsInChildren<Image>()[0];
        fillOfABar.sprite = HPVisual;
        fillOfABar.rectTransform.localScale = new Vector3(0.5f, 0.25f, 0.25f);
        if (this.GetComponent<CanvasGroup>() == null)
        {
            this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    void Update()
    {

        UpdateText();
        FillTheBar();
        UIManager.SmoothlyTransitionActivity(this.gameObject, transform.parent.parent.GetComponentInChildren<Renderer>().material.color == Color.red || isBeingAttacked, 0.1f);
    }

    void UpdateText()
    {
        if (thisObject.GetDurability() <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void FillTheBar()
    {
        float velocity = 0;
        fillOfABar.fillAmount = Mathf.SmoothDamp(fillOfABar.fillAmount, ((float)thisObject.GetCurrentDurability() / (float)thisObject.GetDurability()), ref velocity, barAnimationTime);
    }
}
