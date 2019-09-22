using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] float speed = 2;
    public bool doBFT = true;
    public bool IsNotBeingDestroyed = true;


    private void Start()
    {
        doBFT = true;
    }
    void Update()
    {
        if (IsNotBeingDestroyed == false)
        {
            return;
        }
        if (doBFT == false)
        {
            return;
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                Color temp = item.material.color;
                temp.a = Mathf.Lerp(temp.a, 0.4f, speed * Time.deltaTime);
                item.material.color = temp;
            }
        }
        else
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                Color temp = item.material.color;
                temp.a = Mathf.Lerp(temp.a, 1, speed * Time.deltaTime);
                item.material.color = temp;
            }
        }
    }
}
