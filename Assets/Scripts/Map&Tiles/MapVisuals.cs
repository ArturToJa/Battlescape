using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class MapVisuals : MonoBehaviour
{
    public TileType[] tileTypes;
    Dictionary<GameObject, int> amountOfStuff;
    [SerializeField] List<GameObject> SingleTileTreeObstacles;
    [SerializeField] List<GameObject> FourTileTreeObstacles;
    [SerializeField] List<GameObject> RockObstacles;
    [SerializeField] List<GameObject> FloorStones;
    [SerializeField] List<GameObject> Grass;
    [SerializeField] List<GameObject> Mushrooms;
    [SerializeField] List<GameObject> DropValueOneDestructibles;
    [SerializeField] List<GameObject> DropValueTwoDestructibles;

    public void SetData()
    {
        OneValueDestructibleCount = PersistantMapData.tierOneObstacleCount;
        TwoValueDestructibleCount = PersistantMapData.tierTwoObstacleCount;
        ThreeValueDestructibleCount = PersistantMapData.tierThreeObstacleCount;
        FenceCount = PersistantMapData.fenceCount;
        RockCount = PersistantMapData.rockCount;
        OneTileTreeCount = PersistantMapData.smallTreeCount;
        FourTileTreeCount = PersistantMapData.bigTreeCount;
    }

    [SerializeField] List<GameObject> DropValueThreeDestructibles;
    [SerializeField] List<GameObject> Fences;
    public int OneValueDestructibleCount;
    public int TwoValueDestructibleCount;
    public int ThreeValueDestructibleCount;
    public int FenceCount;
    public int RockCount;
    public int GrassCount;
    public int ShroomCount;
    public int OneTileTreeCount;
    public int FourTileTreeCount;
    public int FloorStoneCount;
    [SerializeField] float allowedPercentage;
    int terminator = 0;

    void CreateNonObstacles(List<GameObject> ObjectType, int objectCount, int Xborder, int Zborder, float allowedOffset)
    {
        for (int i = 0; i < objectCount; i++)
        {

            Tile tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
            while (/*tile.hasDoodad ||*/ tile.hasObstacle)
            {
                tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
                if (CheckForTermination())
                {
                    Debug.Log("Terminated without creating: " + ObjectType[0]);
                    return;
                }
            }
            Instantiate(PseudoRandomlyChooseObject(ObjectType, objectCount, ObjectType.Count), tile.transform.position + RandomlyChangePosition(allowedOffset), Quaternion.Euler(tile.transform.rotation.x, Random.Range(0, 360), transform.rotation.z), tile.transform);
            //tile.hasDoodad = true;
        }
    }

    bool CheckForTermination()
    {
        terminator++;
        if (terminator == 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject PseudoRandomlyChooseObject(List<GameObject> ObjectType, int objectCount, int amountOfExistingTypes)
    {
        if (amountOfStuff == null)
        {
            amountOfStuff = new Dictionary<GameObject, int>();
        }
        GameObject temp = null;
        do
        {
            temp = ObjectType[Random.Range(0, ObjectType.Count)];
            if (amountOfStuff.ContainsKey(temp))
            {
                amountOfStuff[temp]++;
            }
            else
            {
                amountOfStuff.Add(temp, 1);
            }
        } while (amountOfStuff[temp] > CalculateAllowedAmount(objectCount, amountOfExistingTypes));

        return temp;

    }

    int CalculateAllowedAmount(int amountOfTypeToGenerate, int amountOfExistingTypes)
    {
        int allowedAmount = Mathf.CeilToInt(allowedPercentage * amountOfTypeToGenerate);
        if ((float)amountOfTypeToGenerate / (float)amountOfExistingTypes > (float)allowedAmount)
        {
            allowedAmount = Mathf.CeilToInt((float)amountOfTypeToGenerate / (float)amountOfExistingTypes);
        }
        return allowedAmount;
    }

    public void RandomlyPutObstacles()
    {
        HelperRPO();
    }



    public void RandomlyPutObstacles(int seed)
    {
        Random.InitState(seed);
        HelperRPO();
    }

    void HelperMakeBigTree(List<GameObject> ObstacleType, int obstacleCount, Tile tile)
    {
        GameObject tree = Instantiate(PseudoRandomlyChooseObject(ObstacleType, obstacleCount, ObstacleType.Count), tile.transform.position, Quaternion.Euler(tile.transform.rotation.x, Random.Range(0, 360), transform.rotation.z), tile.transform);
        tile.myObstacle = tree;
        //tile.isShootable = false;
        //IF we want to redo the'isShootable' effect- we need to change myObstacle to a new class (Obstacle) being a component on Obstacles or sth like that!
        //we need to offset the obstacle so it can lie on 4 tiles
        tree.transform.position = new Vector3(tree.transform.position.x + 0.5f, tree.transform.position.y, tree.transform.position.z + 0.5f);
        //we need to add "fake" obstacles (and switch their renderers off obviously) on 3 neighbour tiles and set them to nonWalkable too
        InstantiateFakeObstacle(tile, tree, 1, 0);
        InstantiateFakeObstacle(tile, tree, 0, 1);
        InstantiateFakeObstacle(tile, tree, 1, 1);
    }
    void HelperRPO()
    {
        HelperMakeBigTree(FourTileTreeObstacles, FourTileTreeCount, Map.Board[7, 7]);
        HelperMakeBigTree(FourTileTreeObstacles, FourTileTreeCount, Map.Board[7, 4]);
        //CreateObstacles(FourTileTreeObstacles, FourTileTreeCount, 5, 3, 4);
        CreateObstacles(SingleTileTreeObstacles, OneTileTreeCount, 3, 1, 1);
        CreateObstacles(RockObstacles, RockCount, 3, 2, 1);
        CreateObstacles(DropValueThreeDestructibles, ThreeValueDestructibleCount, 5, 0, 1);
        CreateObstacles(DropValueTwoDestructibles, TwoValueDestructibleCount, 4, 0, 1);
        CreateObstacles(DropValueOneDestructibles, OneValueDestructibleCount, 3, 0, 1);
        CreateObstacles(Fences, FenceCount, 5, 0, 1);
        CreateNonObstacles(FloorStones, FloorStoneCount, 1, 1, 0.1f);
        CreateGrass(Grass, GrassCount, 1, 1);
        CreateGrass(Mushrooms, ShroomCount, 1, 1);
    }
    void CreateGrass(List<GameObject> ObjectType, int objectCount, int Xborder, int Zborder)
    {
        for (int i = 0; i < objectCount; i++)
        {
            Tile tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
            GameObject randomGrass = PseudoRandomlyChooseObject(ObjectType, objectCount, ObjectType.Count);
            Instantiate(randomGrass, tile.transform.position + RandomlyChangePosition(0.5f), Quaternion.Euler(tile.transform.rotation.x, Random.Range(0, 360), transform.rotation.z), tile.transform);
        }
    }

    Vector3 RandomlyChangePosition(float displacement)
    {
        float x = Random.Range(-displacement, displacement);
        float z = Random.Range(-displacement, displacement);
        return new Vector3(x, 0, z);
    }

    void CreateObstacles(List<GameObject> ObstacleType, int obstacleCount, int Xborder, int Zborder, int tileCount)
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            Tile tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
            while (tile.hasObstacle || IsThereSomethingNeighbouring((int)tile.transform.position.x, (int)tile.transform.position.z))
            {
                tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
                if (CheckForTermination())
                {
                    terminator = 0;
                    while (tile.hasObstacle)
                    {
                        terminator++;
                        tile = Map.Board[Random.Range(Xborder, Map.mapWidth - Xborder), Random.Range(Zborder, Map.mapHeight - Zborder)];
                        if (CheckForTermination())
                        {
                            return;
                        }
                    }
                    break;
                }
            }
            var tree = Instantiate(PseudoRandomlyChooseObject(ObstacleType, obstacleCount, ObstacleType.Count), tile.transform.position, Quaternion.Euler(tile.transform.rotation.x, Random.Range(0, 360), transform.rotation.z), tile.transform);
            tile.myObstacle = tree;
            switch (tileCount)
            {

                case 4:
                    //tile.isShootable = false;
                    //we need to offset the obstacle so it can lie on 4 tiles
                    tree.transform.position = new Vector3(tree.transform.position.x + 0.5f, tree.transform.position.y, tree.transform.position.z + 0.5f);
                    //we need to add "fake" obstacles (and switch their renderers off obviously) on 3 neighbour tiles and set them to nonWalkable too
                    InstantiateFakeObstacle(tile, tree, 1, 0);
                    InstantiateFakeObstacle(tile, tree, 0, 1);
                    InstantiateFakeObstacle(tile, tree, 1, 1);
                    break;
                default:
                    break;
            }
        }
    }

    bool IsThereSomethingNeighbouring(int x, int z)
    {
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = z - 1; j < z + 2; j++)
            {
                if ((i == x && j == z) || i < 0 || i >= Map.Board.GetLength(0) || j < 0 || j >= Map.Board.GetLength(1))
                {
                    continue;
                }
                //so we are not on original tile and we checked the possibility that we can be on the edge (no neighbour in a way)
                if ((Map.Board[i, j].hasObstacle))
                {
                    // we HAVE a neighbouring obstacle
                    return true;
                }
            }
        }
        // we found nothing
        return false;
    }

    void InstantiateFakeObstacle(Tile tile, GameObject Obstacle, int x, int z)
    {
        Tile Neighbour = Map.Board[(int)tile.transform.position.x + x, (int)tile.transform.position.z + z];
        Neighbour.myObstacle = Obstacle;
        //Neighbour.isShootable = false;
        var additionalTree = Instantiate(Obstacle, Neighbour.transform);
        foreach (Renderer r in additionalTree.GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
        foreach (Collider c in additionalTree.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        //additionalTree.GetComponent<Renderer>().enabled = false;

    }

}
