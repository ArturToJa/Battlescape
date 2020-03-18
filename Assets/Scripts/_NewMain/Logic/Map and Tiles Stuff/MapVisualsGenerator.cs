using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class MapVisualsGenerator
    {
        [SerializeField] List<MapVisualsSpecification> specs;
        [SerializeField] List<OnTileObject> allLegalObjects;
        [SerializeField] float allowedPercentage = 0.25f;

        Dictionary<string, int> alreadyGeneratedStuff = new Dictionary<string, int>();

        public void GenerateObjects(int seed)
        {

            //Random.InitState(seed);
            //foreach (MapVisualsSpecification spec in specs)
            //{
            //    int amountOfType = Random.Range(spec.minAmount, spec.maxAmount);
            //    for (int i = 0; i < amountOfType; i++)
            //    {
            //        GenerateObject(GetRandomObject(spec.type, amountOfType), GetRandomTile(spec.minDistanceToShortSide, spec.minDistanceToLongSide, spec));
            //    }
            //}
        }

        void GenerateObject(OnTileObject prefab, Tile tile)
        {
            if (tile == null)
            {
                Debug.Log("Didn't spawn: " + prefab.name + " cause of lack of space");
                return;
            }
            OnTileObject spawnedObject = Object.Instantiate(prefab, tile.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), tile.transform);
            spawnedObject.OnSpawn(tile);
            AddToDictionary(prefab.name);
        }

        OnTileObject GetRandomObject(TileObjectType type, int amountOfType)
        {
            List<OnTileObject> returnList = GetAllObjectsOfType(type);
            OnTileObject answer = returnList[Random.Range(0, returnList.Count - 1)];
            while (IsObjectAllowed(answer, amountOfType) == false)
            {
                returnList.Remove(answer);
                answer = returnList[Random.Range(0, returnList.Count - 1)];
            }
            return answer;
        }

        Tile GetRandomTile(int minDistanceShort, int minDistanceLong, MapVisualsSpecification spec)
        {
            int terminator = 0;
            Tile tile = Global.instance.currentMap.board[Random.Range(minDistanceShort, Global.instance.currentMap.mapWidth - minDistanceShort), Random.Range(minDistanceLong, Global.instance.currentMap.mapHeight - minDistanceLong)];
            while (IsTileFullyLegal(tile, spec) == false)
            {
                terminator++;
                if (terminator == 100)
                {
                    terminator = 0;
                    while (IsTileBarelyLegal(tile, spec))
                    {
                        terminator++;
                        tile = Global.instance.currentMap.board[Random.Range(0, Global.instance.currentMap.mapWidth), Random.Range(0, Global.instance.currentMap.mapHeight)];
                        if (terminator == 100)
                        {
                            Debug.Log("Terminated.");
                            return null;
                        }
                    }
                }
                tile = Global.instance.currentMap.board[Random.Range(minDistanceShort, Global.instance.currentMap.mapWidth - minDistanceShort), Random.Range(minDistanceLong, Global.instance.currentMap.mapHeight - minDistanceLong)];
            }
            return tile;
        }

        bool IsTileFullyLegal(Tile tile, MapVisualsSpecification spec)
        {
            return tile.hasObstacle == false && (HasExtraSpace(tile));
        }
        bool IsTileBarelyLegal(Tile tile, MapVisualsSpecification spec)
        {
            return tile.hasObstacle == false && (spec.needsExtraSpace == false || HasExtraSpace(tile));
        }

        bool HasExtraSpace(Tile tile)
        {
            foreach (Tile neighbour in tile.neighbours)
            {
                if (neighbour.hasObstacle)
                {
                    return false;
                }
            }
            return true;
        }

        List<OnTileObject> GetAllObjectsOfType(TileObjectType type)
        {
            List<OnTileObject> returnList = new List<OnTileObject>();
            foreach (OnTileObject prefab in allLegalObjects)
            {
                if (prefab.type == type)
                {
                    returnList.Add(prefab);
                }
            }
            return returnList;
        }

        bool IsObjectAllowed(OnTileObject prefab, int amountOfType)
        {
            if (alreadyGeneratedStuff.ContainsKey(prefab.name) == false)
            {
                return true;
            }
            else
            {
                return alreadyGeneratedStuff[prefab.name] <= CalculateAllowedAmount(amountOfType, GetAllObjectsOfType(prefab.type).Count);
            }
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

        void AddToDictionary(string name)
        {
            if (alreadyGeneratedStuff.ContainsKey(name))
            {
                alreadyGeneratedStuff[name]++;
            }
            else
            {
                alreadyGeneratedStuff.Add(name, 1);
            }
        }

    }
    [System.Serializable]
    public class MapVisualsSpecification
    {
        [SerializeField] TileObjectType _type;
        public TileObjectType type
        {
            get
            {
                return _type;
            }
            private set
            {
                _type = value;
            }
        }
        [SerializeField] int _minAmount;
        public int minAmount
        {
            get
            {
                return _minAmount;
            }
            private set
            {
                _minAmount = value;
            }
        }
        [SerializeField] int _maxAmount;
        public int maxAmount
        {
            get
            {
                return _maxAmount;
            }
            private set
            {
                _maxAmount = value;
            }
        }
        [SerializeField] int _minDistanceToShortSide;
        public int minDistanceToShortSide
        {
            get
            {
                return _minDistanceToShortSide;
            }
            private set
            {
                _minDistanceToShortSide = value;
            }
        }
        [SerializeField] int _minDistanceToLongSide;
        public int minDistanceToLongSide
        {
            get
            {
                return _minDistanceToLongSide;
            }
            private set
            {
                _minDistanceToLongSide = value;
            }
        }

        [SerializeField] bool _needsExtraSpace;
        public bool needsExtraSpace
        {
            get
            {
                return _needsExtraSpace;
            }
            private set
            {
                _needsExtraSpace = value;
            }
        }
    }
}