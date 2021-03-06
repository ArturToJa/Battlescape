﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeSound;

namespace BattlescapeLogic
{
    public class Missile : MonoBehaviour
    {
        public IMissileLaucher myLauncher { get; set; }
        static readonly float minDistance = 0.2f;
        public Unit sourceUnit { get; set; }
        public Vector3 target { get; set; }
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

        private Vector3 newPosition = new Vector3();

        void Start()
        {
            maxHeight += this.transform.position.y;

            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            targetPosition = new Vector2(target.x, target.z);
            distanceToTravel = Vector2.Distance(myPosition, targetPosition);
        }
        void Update()
        {
            CalculatePosition();
            UpdatePosition();
            if (Vector3.Distance(this.transform.position, target) < minDistance)
            {
                if (sourceUnit.GetMyOwner().type != PlayerType.Network)
                {
                    myLauncher.OnMissileHitTarget();                   
                }
                BattlescapeSound.SoundManager.instance.PlaySound(gameObject, onHitSound);
                Destroy(gameObject);
            }
        }

        private void CalculatePosition()
        {
            CalculateNew2DPosition();
            CalculateNewHeight();
            CalculateNewAngle();
        }

        private void CalculateNew2DPosition()
        {
            float distanceToMove = speedPerFrame * Time.deltaTime;
            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 new2DPosition = Vector2.MoveTowards(myPosition, targetPosition, distanceToMove);
            newPosition.x = new2DPosition.x;
            newPosition.z = new2DPosition.y;
            distanceTraveled += distanceToMove;
        }

        private void CalculateNewHeight()
        {
            float x = distanceTraveled;
            float x1 = 0.0f;
            float x2 = distanceToTravel;
            float p = x1 + ((x2 - x1) / 2.0f);
            float q = maxHeight;
            float a = (0.0f - q) / Mathf.Pow((x1 - p), 2.0f);
            float newHeight = a * Mathf.Pow((x - p), 2.0f) + q;
            newPosition.y = newHeight;
        }

        private void CalculateNewAngle()
        {
            this.transform.LookAt(newPosition);
        }

        private void UpdatePosition()
        {
            this.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        }
    }
}
