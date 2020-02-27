using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        static readonly float minDistance = 0.2f;
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

        void Start()
        {
            maxHeight += startingPoint.y;
        }
        void Update()
        {
            UpdatePosition();
            if (Vector2.Distance(this.transform.position, target.transform.position) < minDistance)
            {
                if (sourceUnit.owner.type != PlayerType.Network)
                {
                    Networking.instance.SendCommandToHit(sourceUnit, target);
                }
                Destroy(this);
            }
        }

        private void UpdatePosition()
        {
            
            float distanceDelta = CalculateNew2DPosition();
            float heightDelta = CalculateNewHeight();
            CalculateNewPitch(distanceDelta, heightDelta);
        }

        // i'm doin these half asleep, need heavy testing
        private float CalculateNew2DPosition(/*Vector3 newPosition*/)
        {
            float distanceToMove = speedPerFrame * Time.deltaTime;
            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
            Vector2 newPosition = Vector2.MoveTowards(myPosition, targetPosition, distanceToMove);
            this.transform.position = new Vector3(newPosition.x, this.transform.position.y, newPosition.y);
            return distanceToMove;
        }

        private float CalculateNewHeight()
        {
            float x = Mathf.Sqrt(Mathf.Pow(this.transform.position.x, 2.0f) + Mathf.Pow(this.transform.position.z, 2.0f));
            float x1 = Mathf.Sqrt(Mathf.Pow(startingPoint.x, 2.0f) + Mathf.Pow(startingPoint.z, 2.0f));
            float x2 = Mathf.Sqrt(Mathf.Pow(target.transform.position.x, 2.0f) + Mathf.Pow(target.transform.position.z, 2.0f));
            float p = x1 + ((x2 - x1) / 2.0f);
            float q = maxHeight;
            float a = (0.0f - q) / Mathf.Pow((x1 - p), 2.0f);
            float newHeight = a * Mathf.Pow((x - p), 2.0f) + q;
            float oldHeight = this.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, newHeight, this.transform.position.z);
            return newHeight - oldHeight;
        }

        private void CalculateNewPitch(float distanceDelta, float heightDelta)
        {
            float angle = Mathf.Atan2(heightDelta, distanceDelta) * Mathf.Rad2Deg;
            //Debug.Log("angle: " + angle);
           // Maths.SetObjectPitch(this.gameObject, angle);
            this.transform.rotation *= Quaternion.AngleAxis((angle) - this.transform.rotation.eulerAngles.y, new Vector3(0, 1, 0));
        }
    }
}
