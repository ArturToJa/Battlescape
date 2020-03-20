using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarSticker : MonoBehaviour
{
    Camera mainCamera;
    BattlescapeLogic.Unit myUnit;
    [SerializeField] float offset;

    private void Start()
    {
        mainCamera = Camera.main;
        myUnit = transform.GetComponentInParent<BattlescapeLogic.Unit>();
    }
    void LateUpdate()
    {
        transform.position = mainCamera.WorldToScreenPoint(myUnit.transform.position + Vector3.up * offset);
    }
}
