using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using System;

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
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(results.Count);
            Debug.Log(results[0].gameObject.transform.root.tag != "Unit");
        }        
        return (results.Count > 0 && (results[0].gameObject.transform.root.tag != "Unit"));
    }
   

    /// <summary>
    /// Returns the position of the object 'snapped' to closest full integer position
    /// </summary>
    /// <param name="thing">Object to be 'snapped'</param>
    /// <returns></returns>
    public static Vector3 RoundPosition(GameObject thing)
    {
        return new Vector3((int)(thing.transform.position.x + 0.5f), (int)(thing.transform.position.y + 0.5f), (int)(thing.transform.position.z + 0.5f));
        
    }

    //No fricking idea where to put this so it goes to Helper ;<
    public static bool WouldBeInAttackRange(Unit unit, Tile where, Vector3 target)
    {
        Bounds FullRange = new Bounds(where.transform.position, new Vector3(2 * unit.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * unit.statistics.GetCurrentAttackRange() + 0.25f));
        if (unit.statistics.minimalAttackRange > 0)
        {
            Bounds miniRange = new Bounds(where.transform.position, new Vector3(2 * unit.statistics.minimalAttackRange + 0.25f, 5, 2 * unit.statistics.minimalAttackRange + 0.25f));
            return miniRange.Contains(target) == false && FullRange.Contains(target);
        }
        else
        {
            return FullRange.Contains(target);
        }
    }
}
