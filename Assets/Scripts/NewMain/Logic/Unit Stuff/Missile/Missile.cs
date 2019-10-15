using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        static readonly float minDistance = 50;
        public Vector3 startingPoint { get; private set; }
        public Vector3 destinationPoint { get; private set; }
        public float speedPerFrame { get; private set; }
        public float maxHeight { get; private set; }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition();
            if (Vector2.Distance(this.transform.position, destinationPoint) < minDistance)
            {
                Destroy(this);
                //SendEventThatMissleHasCompletedItsTask();
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
            float angle = Mathf.Atan((destinationPoint.z - startingPoint.z) / (destinationPoint.x - startingPoint.x));
            newPosition.x = speedPerFrame * Mathf.Sin(angle) + this.transform.position.x;
            newPosition.z = speedPerFrame * Mathf.Cos(angle) + this.transform.position.z;
        }

        private void CalculateNewHeight(Vector3 newPosition)
        {
            float currentDistance = Mathf.Sqrt(Mathf.Pow(newPosition.x, 2) + Mathf.Pow(newPosition.z, 2));
            float startDistance = Mathf.Sqrt(Mathf.Pow(startingPoint.x, 2) + Mathf.Pow(startingPoint.z, 2));
            float destinationDistance = Mathf.Sqrt(Mathf.Pow(destinationPoint.x, 2) + Mathf.Pow(destinationPoint.z, 2));
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
