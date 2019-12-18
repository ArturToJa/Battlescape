using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeGraphics;

namespace BattlescapeLogic
{
    public class PlayerInput : MonoBehaviour
    {

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            // MOUSE:

            if (Input.GetMouseButton(1))
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    CameraController.Instance.RotateCamera();
                }
            }





            // these are 'cheatcodes' for testing purposes, only working in editor (not in built game)
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // selected unit gets 1000 damage (dies xD)
                    MouseManager.Instance.SelectedUnit.OnHit(MouseManager.Instance.SelectedUnit, 1000);
                }

            }

            //these are actual 'shortcuts'/controls:

            //On click DOWN:

            if (Input.GetKeyDown(KeyCode.End) /* && maybe correct situation?*/)
            {
                //next phase (end phase/turn)
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //close a window
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                // toggle grid
                //I dont know if Map is the perfect guy to toggle grid, but I had to put it somewhere?
                Map.ToggleGrid();
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //Highlight destructibles?
            }
            if (Input.GetKeyDown(KeyCode.Tab) && GameStateManager.Instance.IsCurrentPlayerAI() == false)
            {
                MouseManager.Instance.SelectNextAvailableUnit();
            }

            //On click PERSISTING (as long as the mouse button is being pressed)

            if (Input.GetKey(KeyCode.LeftControl))
            {
                UnitHighlighter.ToggleHighlight();
                //Also: obstacles get progressively more and more transparent
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                //move camera left, but only in some scenarios i think?
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                //move camera right, but only in some scenarios i think?
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                //move camera up, but only in some scenarios i think?
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                //move camera down, but only in some scenarios i think?
            }
            if (Input.GetKey(KeyCode.Space) && InGameInputField.IsNotTypingInChat())
            {
                CameraController.Instance.ResetCamera();
            }

            //On click UP:

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                UnitHighlighter.ToggleHighlight();
            }

        }












        public static bool LeftClickDown()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}