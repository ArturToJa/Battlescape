using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float Tempo = 10;
    [SerializeField] int axis = 2;
    private void Update()
    {
        switch (axis)
        {
            case 0:
                transform.Rotate(new Vector3(Tempo * Time.deltaTime, 0, 0));
                break;
            case 1:
                transform.Rotate(new Vector3(0, Tempo * Time.deltaTime, 0));
                break;
            case 2:
                transform.Rotate(new Vector3(0, 0, Tempo * Time.deltaTime));
                break;
            default:
                break;
        }
    }

}
