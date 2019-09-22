using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarSticker : MonoBehaviour
{
    [SerializeField] GameObject HPBar;

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
        HPBar.transform.position = pos;
    }
}
