using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{

    public static SkyboxChanger instance { get; private set; }
    public Material[] skyboxes;
    public Material PregameSkyboxDefault;
    public Material currSkybox;
    int currentSkyboxNumber;
    public int realSkyboxNumber;
    public bool isSkyboxRandom;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        SetSkyboxTo(PregameSkyboxDefault);
    }


    public void SetSkyboxToRandom()
    {
        currentSkyboxNumber = Random.Range(0, skyboxes.Length);
        RenderSettings.skybox = skyboxes[currentSkyboxNumber];
        realSkyboxNumber = currentSkyboxNumber;
        currSkybox = RenderSettings.skybox;
    }

    public void SetSkyboxTo(int skyboxCounter)
    {
        if (skyboxCounter == -1)
        {
            SetSkyboxToRandom();
        }
        else
        {
            currentSkyboxNumber = skyboxCounter;
            RenderSettings.skybox = skyboxes[currentSkyboxNumber];
            currSkybox = RenderSettings.skybox;
        }     
    }
    public void SetSkyboxTo(Material skybox)
    {
        RenderSettings.skybox = skybox;
        currSkybox = RenderSettings.skybox;
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Debug.Log(currSkybox.name);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSkyboxToRandom();
            }
        }        
    }
}
