﻿using System;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Obstacle : OnTileObject, IMouseTargetable
    {
        private Animator animator;

        

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
            Destroy(gameObject);
        }

        public void OnMouseHoverEnter()
        {
            return;
        }

        public void OnMouseHoverExit()
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

        public int GetDistanceTo(Tile target)
        {
            int distance = 9999;
            foreach (Tile tile in currentPosition)
            {
                int possibleDistance = tile.position.DistanceTo(target.position);
                if (possibleDistance < distance)
                {
                    distance = possibleDistance;
                }
            }
            return distance;
        }
    }
}
