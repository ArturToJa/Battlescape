using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using System;

namespace BattlescapeLogic
{

    public static class Helper
    {
        public static GameObject FindChildWithTag(GameObject parent, string tag)
        {
            var temp = parent.GetComponentsInChildren<Transform>();
            foreach (Transform tr in temp)
            {
                if (tr.tag == tag)
                {
                    return tr.gameObject;
                }
            }
            return null;
        }

        public static void CheckIfInBoundries(Transform t)
        {
            float x = t.position.x;
            float z = t.position.z;
            float width = t.GetComponent<RectTransform>().rect.width;
            float height = t.GetComponent<RectTransform>().rect.height;
            if (x + 0.5f * width > Screen.width)
            {
                x = Screen.width - 0.5f * width;
            }
            if (x - 0.5f * width < 0)
            {
                x = 0.5f * width;
            }
            if (z + 0.5f * height > Screen.height)
            {
                z = Screen.height - 0.5f * height;
            }
            if (z - 0.5f * height < 0)
            {
                z = 0.5f * height;
            }
            t.SetPositionAndRotation(new Vector3(x, t.position.y, z), Quaternion.identity);
        }

        public static bool IsOverNonHealthBarUI()
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                return false;
            }
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return (results.Count > 0 && (results[0].gameObject.transform.root.tag != "Unit"));
        }



    }
}