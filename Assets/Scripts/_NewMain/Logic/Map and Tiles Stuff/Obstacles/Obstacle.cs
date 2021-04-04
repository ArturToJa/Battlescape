using System;
using System.Collections;
using UnityEngine;


namespace BattlescapeLogic
{
    public class Obstacle : OnTileObject, IMouseTargetable, IVisuals
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

        public void Destruct(IDamageSource source)
        {
            //Tu mna być animacja destrukcji.
            currentPosition.SetMyObjectTo(null);
            StartCoroutine(DestructionRoutine());
        }

        IEnumerator DestructionRoutine()
        {
            Vector3 finalPosition = new Vector3(transform.position.x, -1, transform.position.z);
                     
            
            while (transform.position.y > -1)
            {
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    Color finalColour = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);
                    renderer.material.color = (Color.Lerp(renderer.material.color, finalColour, 5 * deathSpeed * Time.deltaTime));
                }
                transform.position = (Vector3.Lerp(transform.position, finalPosition, deathSpeed * Time.deltaTime));                
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
