using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        static readonly float minDistance = 0.5f;
        public Vector3 startingPoint { get; set; }
        public Unit sourceUnit { get; set; }
        public Unit target { get; set; }
        [SerializeField] float _speedPerFrame;
        public float speedPerFrame
        {
            get
            {
                return _speedPerFrame;
            }
            private set
            {
                _speedPerFrame = value;
            }
        }
        [SerializeField] float _maxHeight;
        public float maxHeight
        {
            get
            {
                return _maxHeight;
            }
            private set
            {
                _maxHeight = value;
            }
        }

        void Update()
        {
            UpdatePosition();
            if (Vector2.Distance(this.transform.position, target.transform.position) < minDistance)
            {
                sourceUnit.HitTarget(target);
                Destroy(this);
            }
        }

        private void UpdatePosition()
        {
            Vector3 newPosition = this.transform.position;
            CalculateNew2DPosition(newPosition);
            CalculateNewHeight(newPosition);
            CalculateNewPitch(newPosition);
            this.transform.position = newPosition;
        }

        // i'm doin these half asleep, need heavy testing
        private void CalculateNew2DPosition(Vector3 newPosition)
        {
            float angle = Mathf.Atan((target.transform.position.z - startingPoint.z) / (target.transform.position.x - startingPoint.x));
            newPosition.x = speedPerFrame * Mathf.Sin(angle) + this.transform.position.x;
            newPosition.z = speedPerFrame * Mathf.Cos(angle) + this.transform.position.z;
        }

        private void CalculateNewHeight(Vector3 newPosition)
        {
            float currentDistance = Mathf.Sqrt(Mathf.Pow(newPosition.x, 2) + Mathf.Pow(newPosition.z, 2));
            float startDistance = Mathf.Sqrt(Mathf.Pow(startingPoint.x, 2) + Mathf.Pow(startingPoint.z, 2));
            float destinationDistance = Mathf.Sqrt(Mathf.Pow(target.transform.position.x, 2) + Mathf.Pow(target.transform.position.z, 2));
            newPosition.y = (maxHeight + startingPoint.y) * (currentDistance - startDistance) * (currentDistance - destinationDistance);
        }

        private void CalculateNewPitch(Vector3 newPosition)
        {
            float currentDistance = Mathf.Sqrt(Mathf.Pow(this.transform.position.x, 2) + Mathf.Pow(this.transform.position.z, 2));
            float newDistance = Mathf.Sqrt(Mathf.Pow(newPosition.x, 2) + Mathf.Pow(newPosition.z, 2));
            float angle = Mathf.Atan((this.transform.position.y - newPosition.y) / (newDistance - currentDistance));
            Maths.SetObjectLocalPitch(this.gameObject, angle);
        }
    }
}
