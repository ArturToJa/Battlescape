using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantMapData : MonoBehaviour
{

    public static int mapWidth;
    public static int mapHeight;

    public static int tierOneObstacleCount;
    public static int tierTwoObstacleCount;
    public static int tierThreeObstacleCount;

    public static int fenceCount;

    public static int bigTreeCount;
    public static int smallTreeCount;
    public static int rockCount;
    public static bool hasChangedAnything = false;

    public void SetMapSizeToSmall()
    {
        mapWidth = 16;
        mapHeight = 11;
        hasChangedAnything = true;
    }

    public void SetMapSizeToMedium()
    {
        mapWidth = 18;
        mapHeight = 15;
        hasChangedAnything = true;
    }
    public void SetMapSizeToBig()
    {
        mapWidth = 20;
        mapHeight = 19;
        hasChangedAnything = true;
    }

    public void SetHighDestructibleCount()
    {
        tierOneObstacleCount = 5;
        tierTwoObstacleCount = 4;
        tierThreeObstacleCount = 3;
        fenceCount = 5;
        hasChangedAnything = true;
    }

    public void SetMediumDestructibleCount()
    {
        tierOneObstacleCount = 4;
        tierTwoObstacleCount = 3;
        tierThreeObstacleCount = 2;
        fenceCount = 3;
        hasChangedAnything = true;
    }

    public void SetLowDestructibleCount()
    {
        tierOneObstacleCount = 3;
        tierTwoObstacleCount = 2;
        tierThreeObstacleCount = 1;
        fenceCount = 2;
        hasChangedAnything = true;
    }

    public void SetAbsurdObstacleDensity()
    {
        bigTreeCount = 3;
        smallTreeCount = 8;
        rockCount = 5;
        hasChangedAnything = true;
    }

    public void SetHighObstacleDensity()
    {
        bigTreeCount = 2;
        smallTreeCount = 6;
        rockCount = 4;
        hasChangedAnything = true;
    }
    public void SetMediumObstacleDensity()
    {
        bigTreeCount = 1;
        smallTreeCount = 5;
        rockCount = 3;
        hasChangedAnything = true;
    }
    public void SetLowObstacleDensity()
    {
        bigTreeCount = 0;
        smallTreeCount = 4;
        rockCount = 2;
        hasChangedAnything = true;
    }

    public void DefaultSettings()
    {
        hasChangedAnything = false;
    }

}
