﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
public static CameraController Instance { get; private set; }

    Coroutine routine;
    public float scrollSpeed = 20f;
    public Vector2 panLimit;
    public Transform[] views;
    public float transitionSpeed;
    public float rotationSpeed;
    [SerializeField] Transform currentView;
    public bool isCameraMoving = false;
    public bool manualCamera = false;
    public bool correctCamera = true;
    public float panSpeed = 1f;
    public float panBoarderThickness = 3f;
    public float minY = 1f;
    public float maxY = 4f;
    [SerializeField] bool doAnimate = false;
    [SerializeField] float minYRotation;
    [SerializeField] float maxYRotation;
    [SerializeField] GameObject middle;
    Vector3 startector;
    [SerializeField] BoxCollider bounds;
    Bounds b;
    public bool canChangeVerticalRoration = false;

    public bool canRotate;
    Vector3 LocalRotation;
    public float sensitivity = 4f;
    float rotateDampening = 10f;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        currentView = views[0];
        startector = new Vector3(0, 10f, -5.5f);
        b = bounds.bounds;

        if (PlayerPrefs.GetInt("canChangeYRotation") == 1)
        {
            canChangeVerticalRoration = true;
        }
        else
        {
            canChangeVerticalRoration = false;
        }

        panSpeed = PlayerPrefs.GetFloat("panSpeed");
        sensitivity = PlayerPrefs.GetFloat("rotationSpeed");
    }
    void Update()
    {
       /* if (MouseManager.Instance.SelectedUnit != null && Input.GetKeyDown(KeyCode.C))
        {
            if (SetPosToUnitRoutine != null)
            {
                StopCoroutine(SetPosToUnitRoutine);
            }
            SetPosToUnitRoutine = StartCoroutine(SetCameraToUnit(MouseManager.Instance.SelectedUnit));
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetCameraFree();
        }*/
       /* isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        if (isOutside)
        {
            return;
        }*/
        if (manualCamera)
        {
            CheckForInput();
        }
    }

    void CheckForInput()
    {
        if (SetPosition(transform.localPosition, transform.position) != null)
        {
            transform.localPosition = (Vector3)SetPosition(transform.localPosition, transform.position);
        }
        //SetPosition(transform.localPosition);
        canRotate = Input.GetMouseButton(1);
        if (canRotate)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                LocalRotation.x += Input.GetAxis("Mouse X") * sensitivity;
                if (canChangeVerticalRoration)
                {
                    ChangeVerticalLocation();
                }
            }
        }
        if (Input.GetKey(KeyCode.Space) && InGameInputField.IsNotTypingInChat())
        {
            canRotate = false;
            transform.localPosition = startector;
            LocalRotation = Quaternion.identity.eulerAngles;
        }
    }

    private void ChangeVerticalLocation()
    {
        LocalRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        LocalRotation.y = Mathf.Clamp(LocalRotation.y, minYRotation, maxYRotation);
    }

    private Vector3? SetPosition(Vector3 pos, Vector3 posWorld)
    {
        if (Input.GetMouseButton(1) == false)
        {
            if ((Input.mousePosition.y >= Screen.height - panBoarderThickness ) || (Input.GetKey(KeyCode.W) && InGameInputField.IsNotTypingInChat()) || (Input.GetKey(KeyCode.UpArrow) && InGameInputField.IsNotTypingInChat()))
            {
                pos += Vector3.forward * Time.deltaTime * panSpeed;
                //pos.z += panSpeed * Time.deltaTime;
            }
            if ((Input.mousePosition.y <= panBoarderThickness) || (Input.GetKey(KeyCode.S) && InGameInputField.IsNotTypingInChat()) || (Input.GetKey(KeyCode.DownArrow) && InGameInputField.IsNotTypingInChat()))
            {
                pos += Vector3.back * Time.deltaTime * panSpeed;
                //pos.z -= panSpeed * Time.deltaTime;
            }
            if ((Input.mousePosition.x >= Screen.width - panBoarderThickness) || (Input.GetKey(KeyCode.D) && InGameInputField.IsNotTypingInChat()) || (Input.GetKey(KeyCode.RightArrow) && InGameInputField.IsNotTypingInChat()))
            {
                pos += Vector3.right * Time.deltaTime * panSpeed;
                //pos.x += panSpeed * Time.deltaTime;
            }
            if ((Input.mousePosition.x <= panBoarderThickness) || (Input.GetKey(KeyCode.A) && InGameInputField.IsNotTypingInChat()) || (Input.GetKey(KeyCode.LeftArrow) && InGameInputField.IsNotTypingInChat()))
            {
                pos += Vector3.left * Time.deltaTime * panSpeed;
                //pos.x -= panSpeed * Time.deltaTime;
            }
        }


        /*float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += scroll * scrollSpeed * 30f * Time.deltaTime;*/

        /*pos.x = Mathf.Clamp(pos.x, (-panLimit.x + 7.5f)/pos.y, (panLimit.x + 7.5f)/pos.y);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, (-panLimit.y-50)/pos.y, (panLimit.y-20f)/pos.y);*/
        ;

        if (b.Contains(pos + this.transform.parent.position) == false)
        {
            return null;
            
        }
        return pos;
    }

    public void SetCurrentViewTo(int i)
    {
        if (i < views.Length)
        {
            currentView = views[i];
        }
    }

    void LateUpdate()
    {
        if (!correctCamera)
        {

            Quaternion QT = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, QT, Time.deltaTime * rotateDampening);
            return;
        }

        RepositionToCurrentView();

    }

    private void RepositionToCurrentView()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
        Vector3 currAngle = new Vector3
            (
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.rotation.eulerAngles.x, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.rotation.eulerAngles.y, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.rotation.eulerAngles.z, Time.deltaTime * rotationSpeed)
            );
        transform.eulerAngles = currAngle;
    }

    public IEnumerator CheckIfPositionAndRotationMatchDesired()
    {
        yield return new WaitForSeconds(1f);

        if (doAnimate)
        {
            ShowStartAnimation();
            yield return new WaitForSeconds(30f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        correctCamera = false;
        manualCamera = true;
    }

    public void ShowStartAnimation()
    {
        GetComponent<Animation>().Play();
    }

    //we use this one, the basic one, in selecting unit. It is cool clean and makes camera just go towards a unit for a moment (1f second currently).
    IEnumerator SetCameraToUnit(UnitScript unit)
    {
        manualCamera = false;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, unit.transform.position + new Vector3(0, startector.y, -3), Time.deltaTime * transitionSpeed);
            LocalRotation = Quaternion.identity.eulerAngles;
            yield return null;
        }
        manualCamera = true;

    }
    // this one will be used for following a unit as long as the "condition" for continuing the loop is met - like "follow unit untill it ends its movement".
    IEnumerator SetCameraToUnit(UnitScript unit, bool condition)
    {
        manualCamera = false;
        while (condition)
        {
            transform.position = Vector3.Lerp(transform.position, unit.transform.position + new Vector3(0, startector.y, -3), Time.deltaTime * transitionSpeed);
            LocalRotation = Quaternion.identity.eulerAngles;
            yield return null;
        }
        manualCamera = true;
        
    }

    /*public void SetCameraFree()
    {
        manualCamera = true;
    }*/
    /*Vector3.Distance(transform.position, unit.transform.position + new Vector3(0, startector.y, -3)) > 0.2f*/

    public void SetCamToU(UnitScript unit, bool condition)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
       routine = StartCoroutine(SetCameraToUnit(unit,condition));
    }
    public void SetCamToU(UnitScript unit)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(SetCameraToUnit(unit));
    }
}
