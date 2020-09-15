using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattlescapeLogic
{
    public class GameVersion : MonoBehaviour
    {
        [SerializeField] string gameVersion;
        [SerializeField] Text versionText;

        void Awake()
        {
            versionText.text = gameVersion;
            if (PlayerPrefs.HasKey("Version") && PlayerPrefs.GetString("Version") == gameVersion)
            {
                return;
            }
            else
            {
                OnUpdateVersion();
            }
        }

        void OnUpdateVersion()
        {
            //For now, nothing. But in the future, something ^ ^
            PlayerPrefs.SetString("Version", gameVersion);
        }
    }
}