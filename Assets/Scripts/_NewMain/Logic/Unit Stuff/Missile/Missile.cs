using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        public IMissileLaucher myLauncher { get; set; }
        static readonly float minDistance = 0.2f;
        public Unit sourceUnit { get; set; }
        public Tile target { get; set; }
        [SerializeField] Sound onHitSound;
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

        private Vector2 targetPosition;
        private float distanceToTravel = 0.0f;
        private float distanceTraveled = 0.0f;
        private bool directionX = false;
        private bool directionZ = false;

        void Start()
        {
            maxHeight += this.transform.position.y;

            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
            distanceToTravel = Vector2.Distance(myPosition, targetPosition);

            directionX = myPosition.x <= targetPosition.x;
            directionZ = myPosition.y <= targetPosition.y;
        }
        void Update()
        {
            UpdatePosition();
            if (Vector3.Distance(this.transform.position, target.transform.position) < minDistance)
            {
                if (sourceUnit.GetMyOwner().type != PlayerType.Network)
                {
                    myLauncher.OnMissileHitTarget(target);                   
                }
                BattlescapeSound.SoundManager.instance.PlaySound(gameObject, onHitSound);
                Destroy(gameObject);
            }
        }

        private void UpdatePosition()
        {
            float distanceDelta = CalculateNew2DPosition();
            float heightDelta = CalculateNewHeight();
            CalculateNewPitch(distanceDelta, heightDelta);
        }

        private float CalculateNew2DPosition()
        {
            float distanceToMove = speedPerFrame * Time.deltaTime;
            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 newPosition = Vector2.MoveTowards(myPosition, targetPosition, distanceToMove);
            this.transform.position = new Vector3(newPosition.x, this.transform.position.y, newPosition.y);
            distanceTraveled += distanceToMove;
            return distanceToMove;
        }

        private float CalculateNewHeight()
        {
            float x = distanceTraveled;
            float x1 = 0.0f;
            float x2 = distanceToTravel;
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
            float angle = -90 + Mathf.Atan2(heightDelta, distanceDelta) * Mathf.Rad2Deg;
            
            if(directionX)
            {
                angle = -angle;
            }
            if(directionZ)
            {
                angle += 270;
            }
            Maths.SetObjectYaw(this.gameObject, angle);
        }
    }
}
