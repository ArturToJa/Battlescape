using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeGraphics
{
    [System.Serializable]
    public class Skybox
    {
        [SerializeField] Material[] skyOptions;        
        Material currSkybox;
        int currentSkyboxIndex;        

        public void SetSkyboxToRandom()
        {
            currentSkyboxIndex = Random.Range(0, skyOptions.Length);
            RenderSettings.skybox = skyOptions[currentSkyboxIndex];
            currSkybox = RenderSettings.skybox;
        }

        public void SetSkyboxTo(int index)
        {
            currentSkyboxIndex = index;
            RenderSettings.skybox = skyOptions[currentSkyboxIndex];
            currSkybox = RenderSettings.skybox;
        }

    }
}
