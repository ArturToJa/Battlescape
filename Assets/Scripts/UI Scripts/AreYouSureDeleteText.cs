using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreYouSureDeleteText : MonoBehaviour
{

    [SerializeField] Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Are you sure to delete: " + SaveLoadManager.Instance.currentSaveName + "?";
    }
}
