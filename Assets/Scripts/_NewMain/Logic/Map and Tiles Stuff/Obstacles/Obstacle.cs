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
        
        public override void OnNewRound()
        {
            return;
        }

        public override void OnNewTurn()
        {
            return;
        }

        public override void OnNewPhase()
        {
            return;
        }
    }
}
