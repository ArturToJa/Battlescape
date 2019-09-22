using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_LogCreation : MonoBehaviour
{
    // Use this for initialization
    static int number = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            number++;
            Log.SpawnLog("This is a log number: " + number);
        }
    }
}
