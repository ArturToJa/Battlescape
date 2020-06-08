using System;
using System.Collections;
using UnityEngine;


namespace BattlescapeLogic
{
    public class Obstacle : OnTileObject, IMouseTargetable
    {
        float deathSpeed = .5f;

        Animator animator;

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

        public void Start()
        {
            turnChanger = new TurnChanger(OnNewRound, OnNewTurn, OnNewPhase);
        }

        public void Destruct(Unit source)
        {
            //Tu mna być animacja destrukcji.
            foreach (Tile myTile in currentPosition)
            {
                myTile.SetMyObjectTo(null);
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

        public virtual void OnMouseHoverEnter(Vector3 exactMousePosition)
        {
            return;
        }

        public virtual void OnMouseHoverExit()
        {
            return;
        }        

        public int GetDistanceTo(MultiTile target)
        {
            int distance = 9999;
            Tile targetClosestTile = target.bottomLeftCorner;
            Tile thisClosestTile = currentPosition.bottomLeftCorner;

            foreach (Tile tile in currentPosition)
            {
                int possibleDistance = tile.position.DistanceTo(targetClosestTile.position);
                if (possibleDistance < distance)
                {
                    distance = possibleDistance;
                    thisClosestTile = tile;
                }
            }

            foreach (Tile tile in target)
            {
                int possibleDistance = tile.position.DistanceTo(thisClosestTile.position);
                if (possibleDistance < distance)
                {
                    distance = possibleDistance;
                    targetClosestTile = tile;
                }
            }
            return distance;
        }
        
        public void OnNewRound()
        {
            return;
        }

        public void OnNewTurn()
        {
            return;
        }

        public void OnNewPhase()
        {
            return;
        }
    }
}
