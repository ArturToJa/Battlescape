using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        static readonly float minDistance = 0.1f;
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
            
            float distanceDelta = CalculateNew2DPosition();
            float heightDelta = CalculateNewHeight();
            //CalculateNewPitch(distanceDelta, heightDelta);
        }

        // i'm doin these half asleep, need heavy testing
        private float CalculateNew2DPosition(/*Vector3 newPosition*/)
        {
            float distanceToMove = speedPerFrame * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, distanceToMove);
            return distanceToMove;
        }

        private float CalculateNewHeight()
        {
            float currentDistance = Mathf.Sqrt(Mathf.Pow(this.transform.position.x, 2) + Mathf.Pow(this.transform.position.z, 2));
            float startDistance = Mathf.Sqrt(Mathf.Pow(startingPoint.x, 2) + Mathf.Pow(startingPoint.z, 2));
            float destinationDistance = Mathf.Sqrt(Mathf.Pow(target.transform.position.x, 2) + Mathf.Pow(target.transform.position.z, 2));
            float newHeight = (-1.0f) * (maxHeight + startingPoint.y) * (currentDistance - startDistance) * (currentDistance - destinationDistance);
            float oldHeight = this.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, newHeight, this.transform.position.z);
            return newHeight - oldHeight;
        }

        private void CalculateNewPitch(float distanceDelta, float heightDelta)
        {
            //Debug.Log("Before: " + this.transform.eulerAngles);
            float angle = Mathf.Atan2(heightDelta, distanceDelta);
            Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
            Maths.SetObjectPitch(this.gameObject, angle * Mathf.Rad2Deg);
            //Debug.Log("After: " + this.transform.eulerAngles);
        }
    }
}
