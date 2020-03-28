using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BattlescapeUI
{
    public abstract class AbstractHealthbar : MonoBehaviour
    {
        protected Transform myObject;
        Camera mainCamera;
        protected Image healthBarFill;
        [SerializeField] float offset;
        [SerializeField] float barAnimationTime;

        void Start()
        {
            OnStart();
            mainCamera = Camera.main;
        }

        void Update()
        {
            OnUpdate();            
        }

        void LateUpdate()
        {
            transform.position = mainCamera.WorldToScreenPoint(myObject.position + Vector3.up * offset);
        }

        protected abstract void OnStart();        

        protected virtual void OnUpdate()
        {
            FillHealthbar();
        }

        public void TurnOn()
        {
            UIManager.InstantlyTransitionActivity(gameObject, true);
        }

        public void TurnOff()
        {
            UIManager.InstantlyTransitionActivity(gameObject, false);
        }

        void FillHealthbar()
        {
            float velocity = 0;
            healthBarFill.fillAmount = Mathf.SmoothDamp(healthBarFill.fillAmount, GetPercent(), ref velocity, barAnimationTime);
        }

        protected abstract float GetPercent();       
    }
}