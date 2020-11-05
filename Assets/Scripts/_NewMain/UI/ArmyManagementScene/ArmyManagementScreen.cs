using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BattlescapeUI
{
    public abstract class ArmyManagementScreen : MonoBehaviour
    {
        bool isSetup = false;
        [SerializeField] Button backButton;
        [SerializeField] Button _forwardButton;
        public Button forwardButton
        {
            get
            {
                return _forwardButton;
            }
            private set
            {
                _forwardButton = value;
            }
        }

        public virtual void OnSetup()
        {
            isSetup = true;
            backButton.onClick.AddListener(ArmyManagementScreens.instance.GoBack);
            forwardButton.onClick.AddListener(ArmyManagementScreens.instance.GoForward);
        }

        public virtual void OnChoice()
        {
            if (isSetup == false)
            {
                OnSetup();
            }
            transform.SetAsLastSibling();
        }
    }
}