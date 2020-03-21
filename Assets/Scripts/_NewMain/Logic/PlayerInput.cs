using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeGraphics;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

namespace BattlescapeLogic
{
    public class PlayerInput : MonoBehaviour
    {
        bool _isInputBlocked;
        public bool isInputBlocked
        {
            get
            {
                return _isInputBlocked;
            }
            set
            {
                _isInputBlocked = value;
                ResetCursor();
            }
        }
        //this is the old AnimatingState - it's being set to true when someone is moving or attacking or so on.
        public static PlayerInput instance { get; private set; }
        IMouseTargetable hoveredObject;
        //let's try to not make it public. It is mostly for un-hovering (OnMouseHoverExit).
        GameObject lastUI;
        //This is only to optimise stuff, when mouse is over UI, as GetComponent is heavy and no need to do it each frame.

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
                isInputBlocked = false;
            }
            else
            {
                Destroy(this);
            }
            AbstractActiveAbility.OnAbilityFinished += ResetCursor;
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name.Contains("_GameScene_") == false)
            {
                Cursor.instance.SetToDefault();
                return;
            }
            if (Helper.IsOverNonHealthBarUI())
            {
                Cursor.instance.SetToDefault();
            }
            else
            {
                DoMouse();
            }
            if (Application.isEditor)
            {
                DoCheats();
            }
            DoKeyboard();
        }

        void DoKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                GameRound.instance.OnPressEndButton();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //close a window
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                // toggle grid
                //I dont know if Map is the perfect guy to toggle grid, but I had to put it somewhere?
                Global.instance.currentMap.ToggleGrid();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //Here there sould be some check if it's OUR turn or stuff?
                GameRound.instance.currentPlayer.SelectRandomUnit();
            }

            //On click PERSISTING (as long as the mouse button is being pressed)

            if (Input.GetKey(KeyCode.LeftControl))
            {
                ColouringTool.ColourUnitsThatStillCanMoveOrAttack();
                //Highlight destructibles
                //Also: obstacles get progressively more and more transparent
            }
            if (CameraController.Instance != null)
            {
                if ((Input.mousePosition.x <= CameraController.Instance.panBoarderThickness) || Input.GetKey(KeyCode.A) && InGameInputField.IsNotTypingInChat() || Input.GetKey(KeyCode.LeftArrow))
                {
                    CameraController.Instance.OnCameraMove(Vector3.left);
                }
                if ((Input.mousePosition.x >= Screen.width - CameraController.Instance.panBoarderThickness) || Input.GetKey(KeyCode.D) && InGameInputField.IsNotTypingInChat() || Input.GetKey(KeyCode.RightArrow))
                {
                    CameraController.Instance.OnCameraMove(Vector3.right);
                }
                if ((Input.mousePosition.y >= Screen.height - CameraController.Instance.panBoarderThickness) || Input.GetKey(KeyCode.W) && InGameInputField.IsNotTypingInChat() || Input.GetKey(KeyCode.UpArrow))
                {
                    CameraController.Instance.OnCameraMove(Vector3.forward);
                }
                if ((Input.mousePosition.y <= CameraController.Instance.panBoarderThickness) || Input.GetKey(KeyCode.S) && InGameInputField.IsNotTypingInChat() || Input.GetKey(KeyCode.DownArrow))
                {
                    CameraController.Instance.OnCameraMove(Vector3.back);
                }
                if (Input.GetKey(KeyCode.Space) && InGameInputField.IsNotTypingInChat())
                {
                    CameraController.Instance.ResetCamera();
                }
            }


            //On click UP:

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                ColouringTool.UncolourAllUnits();
            }
        }

        void DoCheats()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // selected unit gets 1000 damage (dies xD)
                GameRound.instance.currentPlayer.selectedUnit.TakeDamage(GameRound.instance.currentPlayer.selectedUnit, 1000);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (GameRound.instance.currentPlayer.selectedUnit != null && !GameRound.instance.currentPlayer.selectedUnit.buffs.IsEmpty())
                {
                    GameRound.instance.currentPlayer.selectedUnit.buffs[0].RemoveFromTargetInstantly();
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StatisticChangeBuff defenceDebuff = Instantiate(Resources.Load("Buffs/MechanicsBuffs/Combat Wound") as GameObject).GetComponent<StatisticChangeBuff>();
                defenceDebuff.ApplyOnTarget(GameRound.instance.currentPlayer.selectedUnit);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log(Global.instance.currentEntity);
            }
        }

        void DoMouse()
        {
            if (GameRound.instance.currentPlayer == null)
            {
                Cursor.instance.SetToDefault();
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                IMouseTargetable newHoveredObject = hitInfo.collider.transform.GetComponentInParent<IMouseTargetable>();
                //just caching.
                if (newHoveredObject != hoveredObject)
                {
                    if (hoveredObject != null)
                    {
                        //Old hovered object existed so we  de-hover it with On Exit
                        hoveredObject.OnMouseHoverExit();
                    }

                    //Change hovered object to new
                    hoveredObject = newHoveredObject;
                    // New object is hovered, so play On Enter.
                    hoveredObject.OnMouseHoverEnter();
                    if (Global.instance.currentEntity != null)
                    {
                        Global.instance.currentEntity.OnCursorOver(hoveredObject);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Global.instance.currentEntity.OnLeftClick(hoveredObject);
                }
            }
            else
            {
                if (GameRound.instance.currentPlayer.selectedUnit == null && Cursor.instance.isInfoByUI == false)
                {
                    Cursor.instance.SetToDefault();
                }
                else
                {
                    Cursor.instance.OnInvalidTargetHovered();
                }
                if (hoveredObject != null)
                {
                    //We HAD an object and now we don't so lets play On Exit.
                    hoveredObject.OnMouseHoverExit();
                    hoveredObject = null;
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                Global.instance.currentEntity.OnRightClick(hoveredObject);
                //This can be also null - its ok! Ability will cancel and other IActiveEntitys will do nothing ;)

            }
            if (Input.GetMouseButton(1) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            {
                CameraController.Instance.RotateCamera();
            }
        }

        void ResetCursor()
        {
            hoveredObject = null;
        }
    }
}
