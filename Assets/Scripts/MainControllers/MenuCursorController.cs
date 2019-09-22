using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCursorController : MonoBehaviour
{
    [SerializeField] Texture2D normalCursor;
    [SerializeField] Texture2D clickingCursor;

    [SerializeField] Texture2D infoCursor;

    bool skipSettingCursor;

    public static MenuCursorController Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    void Update()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(clickingCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void SetCursorToInfo()
    {
        skipSettingCursor = true;
        Cursor.SetCursor(infoCursor, Vector2.zero, CursorMode.Auto);
    }

    public void StopSettingInfoCursor()
    {
        skipSettingCursor = false;
    }
}
