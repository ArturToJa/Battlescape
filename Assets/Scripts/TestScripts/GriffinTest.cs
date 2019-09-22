using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriffinTest : MonoBehaviour
{
    Animator a;

    // Use this for initialization
    void Start()
    {
        a = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            a.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            a.SetTrigger("Wound");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            a.SetTrigger("Death");
        }
    }

}
