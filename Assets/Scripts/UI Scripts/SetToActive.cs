using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToActive : MonoBehaviour
{
    [SerializeField] Transform mum;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UIManager.SmoothlyTransitionActivity(this.gameObject, mum.childCount == 0, 0.1f);
    }
}
