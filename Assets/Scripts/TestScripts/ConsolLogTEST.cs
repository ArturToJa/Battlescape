using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsolLogTEST : MonoBehaviour
{
    static int number = 0;
    // Use this for initialization
    void Start()
    {
        number++;
        GetComponent<Text>().text = "Test text number " + number;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
