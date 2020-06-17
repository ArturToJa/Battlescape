using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class MapVisualsGenerator
    {
        [System.Serializable]
        class GameobjectList
        {
            [SerializeField] public List<GameObject> elements;
        }

        [SerializeField] List<MapVisualsSpecification> specs;
        [SerializeField] List<GameobjectList> allLegalObjects;
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
                    GameObject placeableObject = GenerateObject(GetRandomObject(spec.type, amountOfType), spec.canRotate);
                    IOnTilePlaceable placeable = placeableObject.GetComponent<IOnTilePlaceable>();
                    PlaceObject(placeableObject, GetRandomMultiTile(placeable.currentPosition.width, placeable.currentPosition.height, spec));
                }
            }
        }

        void PlaceObject(GameObject objectToPlace, MultiTile position)
        {
            if (position == null)
            {
                GameObject.Destroy(objectToPlace);
                return;
            }
            objectToPlace.GetComponent<IOnTilePlaceable>().OnSpawn(position.bottomLeftCorner);
            Vector3 oldScale = objectToPlace.transform.localScale;
            objectToPlace.transform.SetParent(position.bottomLeftCorner.transform);
            objectToPlace.transform.localScale = oldScale;
        }

        GameObject GenerateObject(GameObject prefab, bool canRotate)
        {
            Quaternion rotation = prefab.transform.rotation;
            if (canRotate)
            {
                rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
            GameObject spawnedObject = Object.Instantiate(prefab);
            IOnTilePlaceable placeable = spawnedObject.GetComponent<IOnTilePlaceable>();
            AddToDictionary(prefab.name);
            return spawnedObject;
        }

        GameObject GetRandomObject(TileObjectType type, int amountOfType)
        {
            List<GameObject> returnList = GetAllObjectsOfType(type);
            GameObject answer = returnList[Random.Range(0, returnList.Count - 1)];
            int terminator = 0;
            while (IsObjectAllowed(answer, amountOfType, type) == false)
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

        MultiTile GetRandomMultiTile(int width, int height, MapVisualsSpecification spec)
        {
            int terminator = 0;
            Tile tile = Global.instance.currentMap.board[Random.Range(spec.minDistanceToShortSide, Global.instance.currentMap.mapWidth - spec.minDistanceToShortSide), Random.Range(spec.minDistanceToLongSide, Global.instance.currentMap.mapHeight - spec.minDistanceToLongSide)];
            MultiTile position = MultiTile.Create(tile, width, height);
            while (IsMultiTileLegal(position, spec) == false)
            {
                terminator++;
                tile = Global.instance.currentMap.board[Random.Range(spec.minDistanceToShortSide, Global.instance.currentMap.mapWidth - spec.minDistanceToShortSide), Random.Range(spec.minDistanceToLongSide, Global.instance.currentMap.mapHeight - spec.minDistanceToLongSide)];
                position = MultiTile.Create(tile, width, height);
                if (terminator == 100)
                {
                    return null;
                }
            }

            return position;
        }

        bool IsMultiTileLegal(MultiTile position, MapVisualsSpecification spec)
        {
            return position.IsWalkable() && (spec.needsExtraSpace == false || HasExtraSpace(position));
        }

        bool HasExtraSpace(MultiTile position)
        {
            foreach (Tile neighbour in position.closeNeighbours)
            {
                foreach (Tile closeNeighbour in neighbour.neighbours)
                {
                    if(closeNeighbour.IsWalkable() == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        List<GameObject> GetAllObjectsOfType(TileObjectType type)
        {
            return allLegalObjects[(int)type].elements;
        }

        bool IsObjectAllowed(GameObject prefab, int amountOfType, TileObjectType type)
        {
            if (alreadyGeneratedStuff.ContainsKey(prefab.name) == false)
            {
                return true;
            }
            else
            {
                return alreadyGeneratedStuff[prefab.name] <= CalculateAllowedAmount(amountOfType, GetAllObjectsOfType(type).Count);
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