using System;
using System.Collections;
using UnityEngine;


namespace BattlescapeLogic
{
    public class Obstacle : OnTileObject, IMouseTargetable
    {
        float deathSpeed = .5f;

        Animator animator;


        [SerializeField] Position[] shape;

        public Tile[] currentPosition { get; private set; }

        [SerializeField]
        private bool _isTall = false;
        public bool isTall
        {
            get
            {
                return _isTall;
            }
            private set
            {
                _isTall = value;
            }
        }


        public override void OnSpawn(Tile spawningTile)
        {
            base.OnSpawn(spawningTile);
            currentPosition = GetTiles(spawningTile, shape);
            foreach (Tile tile in currentPosition)
            {
                tile.myObstacle = this;
            }
        }


        public virtual void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Destruct(Unit source)
        {
            //Tu mna być animacja destrukcji.
            foreach (Tile myTile in currentPosition)
            {
                myTile.myObstacle = null;
            }
            StartCoroutine(DestructionRoutine());
        }

        IEnumerator DestructionRoutine()
        {
            Vector3 finalPosition = new Vector3(transform.position.x, -1, transform.position.z);
            Renderer renderer = GetComponent<Renderer>();
            Color finalColour = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);
            while (transform.position.y > -1)
            {
                transform.position = (Vector3.Lerp(transform.position, finalPosition, deathSpeed * Time.deltaTime));
                renderer.material.color = (Color.Lerp(renderer.material.color, finalColour, 5*deathSpeed * Time.deltaTime));
                yield return null;
            }
            Destroy(gameObject);
        }

        public virtual void OnMouseHoverEnter()
        {
            return;
        }

        public virtual void OnMouseHoverExit()
        {
            return;
        }

        Tile[] GetTiles(Tile tile, Position[] myShape)
        {
            Tile[] list = new Tile[shape.Length];
            for (int i = 0; i < shape.Length; i++)
            {
                int tileX = tile.position.x + myShape[i].x;
                int tileZ = tile.position.z + myShape[i].z;
                if (tileX <= Global.instance.currentMap.mapWidth && tileX >= 0 && tileZ <= Global.instance.currentMap.mapHeight && tileZ >= 0)
                {
                    list[i] = Global.instance.currentMap.board[tileX, tileZ];
                }
            }
            return list;
        }

        public int GetDistanceTo(Position target)
        {
            int distance = 9999;
            foreach (Tile tile in currentPosition)
            {
                int possibleDistance = tile.position.DistanceTo(target);
                if (possibleDistance < distance)
                {
                    distance = possibleDistance;
                }
            }
            return distance;
        }
    }
}
