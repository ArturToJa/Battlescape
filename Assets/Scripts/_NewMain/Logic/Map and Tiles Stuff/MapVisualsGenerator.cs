using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class MapVisualsGenerator
    {
        [SerializeField] List<MapVisualsSpecification> specs;
        [SerializeField] List<IOnTilePlaceable> allLegalObjects;
        [SerializeField] float allowedPercentage = 0.25f;
        List<Tile> tilesWithObstacle;

        Dictionary<string, int> alreadyGeneratedStuff = new Dictionary<string, int>();

        public void GenerateObjects(int seed)
        {
            Random.InitState(seed);
            foreach (MapVisualsSpecification spec in specs)
            {
                int amountOfType = Random.Range(spec.minAmount, spec.maxAmount + 1);
                for (int i = 0; i < amountOfType; i++)
                {
                    GenerateObject(GetRandomObject(spec.type, amountOfType), GetRandomTile(spec.minDistanceToShortSide, spec.minDistanceToLongSide, spec), spec.canRotate);
                }
            }
        }

        void GenerateObject(IOnTilePlaceable prefab, Tile tile, bool canRotate)
        {
            if (tile == null)
            {
                Debug.Log("Didn't spawn: " + prefab.name + " cause of lack of space");
                return;
            }
            var temp = prefab as MonoBehaviour;
            Quaternion rotation = temp.transform.rotation;
            if (canRotate)
            {
                rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
            IOnTilePlaceable spawnedObject = Object.Instantiate(temp, tile.transform.position, rotation, tile.transform).GetComponent<IOnTilePlaceable>();
            spawnedObject.OnSpawn(tile);
            AddToDictionary(prefab.name);
        }

        IOnTilePlaceable GetRandomObject(TileObjectType type, int amountOfType)
        {
            List<IOnTilePlaceable> returnList = GetAllObjectsOfType(type);
            IOnTilePlaceable answer = returnList[Random.Range(0, returnList.Count - 1)];
            int terminator = 0;
            while (IsObjectAllowed(answer, amountOfType) == false)
            {
                terminator++;
                returnList.Remove(answer);
                answer = returnList[Random.Range(0, returnList.Count - 1)];
                if (terminator == 100)
                {                    
                    return returnList[Random.Range(0, returnList.Count - 1)]; ;
                }
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
                tile = Global.instance.currentMap.board[Random.Range(minDistanceShort, Global.instance.currentMap.mapWidth - minDistanceShort), Random.Range(minDistanceLong, Global.instance.currentMap.mapHeight - minDistanceLong)];
                if (terminator == 100)
                {
                    Debug.Log("half-terminator");
                    break;
                }
            }
            if (terminator == 100)
            {
                terminator = 0;
                while (IsTileBarelyLegal(tile, spec) == false)
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
            return tile;
        }

        bool IsTileFullyLegal(Tile tile, MapVisualsSpecification spec)
        {
            return tile.IsWalkable() && (HasExtraSpace(tile));
        }
        bool IsTileBarelyLegal(Tile tile, MapVisualsSpecification spec)
        {
            return tile.IsWalkable() && (spec.needsExtraSpace == false || HasExtraSpace(tile));
        }

        bool HasExtraSpace(Tile tile)
        {
            foreach (Tile neighbour in tile.neighbours)
            {
                if (neighbour.IsWalkable())
                {
                    return false;
                }
            }
            return true;
        }

        List<IOnTilePlaceable> GetAllObjectsOfType(TileObjectType type)
        {
            List<IOnTilePlaceable> returnList = new List<IOnTilePlaceable>();
            foreach (IOnTilePlaceable prefab in allLegalObjects)
            {
                if (prefab.type == type)
                {
                    returnList.Add(prefab);
                }
            }
            return returnList;
        }

        bool IsObjectAllowed(IOnTilePlaceable prefab, int amountOfType)
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
        }
        [SerializeField] int _minAmount;
        public int minAmount
        {
            get
            {
                return _minAmount;
            }        
        }
        [SerializeField] int _maxAmount;
        public int maxAmount
        {
            get
            {
                return _maxAmount;
            }           
        }
        [SerializeField] int _minDistanceToShortSide;
        public int minDistanceToShortSide
        {
            get
            {
                return _minDistanceToShortSide;
            }
        }
        [SerializeField] int _minDistanceToLongSide;
        public int minDistanceToLongSide
        {
            get
            {
                return _minDistanceToLongSide;
            }
        }

        [SerializeField] bool _needsExtraSpace;
        public bool needsExtraSpace
        {
            get
            {
                return _needsExtraSpace;
            }            
        }

        [SerializeField] bool _canRotate;
        public bool canRotate
        {
            get
            {
                return _canRotate;
            }
        }
    }
}