using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    /* public static PhotonView GetOrAddPhotonView(GameObject myGameObject)
     {
         PhotonView photonView = myGameObject.GetComponent<PhotonView>();
         if (photonView == null)
         {
             photonView = myGameObject.AddComponent<PhotonView>();
             photonView.viewID = PhotonNetwork.AllocateViewID();
         }

         return photonView;
     }*/
    /// <summary>
    /// Returns all tiles in range of 'range' from 'tile'. Note that 'tile' is not included!
    /// </summary>
    public static List<Tile> GetTilesInRangeOf(Tile tile, int range)
    {
        int x = Mathf.RoundToInt(tile.transform.position.x);
        int z = Mathf.RoundToInt(tile.transform.position.z);
        List<Tile> ReturnList = new List<Tile>();
        for (int i = -range; i <= range; i++)
            for (int j = -range; j <= range; j++)
            {
                int tileX = x + i;
                int tileZ = z + j;
                if (
                    tileX < 0 || tileX > Map.mapWidth -1 || tileZ < 0 || tileZ > Map.mapHeight -1
                    )
                {
                    continue;
                }
                if (
                    Map.Board[tileX,tileZ] != null &&
                    Map.Board[tileX,tileZ] != tile
                    )
                {
                    ReturnList.Add(Map.Board[tileX, tileZ]);
                }
            }
        return ReturnList;
    }

    /// <summary>
    /// Returns all UnitScripts of the SAME Player in range of 'range' from 'unit'. Note that 'unit' is not included!
    /// </summary>
    public static List<UnitScript> GetAlliesInRange(UnitScript unit, int range)
    {
        List<UnitScript> AlliesInRange = new List<UnitScript>();
        foreach (Tile tile in GetTilesInRangeOf(unit.myTile, range))
        {
            if (tile.myUnit != null && tile.myUnit.PlayerID == unit.PlayerID)
            {
                AlliesInRange.Add(tile.myUnit);
            }
        }
        return AlliesInRange;
    }

    /// <summary>
    /// Returns all UnitScripts of the OTHER Player in range of 'range' from 'unit'.
    /// </summary>
    public static List<UnitScript> GetEnemiesInRange(UnitScript unit, int range)
    {
        List<UnitScript> EnemiesInRange = new List<UnitScript>();
        foreach (Tile tile in GetTilesInRangeOf(unit.myTile, range))
        {
            if (tile.myUnit != null && tile.myUnit.PlayerID != unit.PlayerID)
            {
                EnemiesInRange.Add(tile.myUnit);
            }
        }
        return EnemiesInRange;
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
}
