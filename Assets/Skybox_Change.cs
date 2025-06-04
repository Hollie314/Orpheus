using System;
using Orpheus.Core;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] skyboxes; // Assigne-les dans l'inspecteur ou charge-les dynamiquement
    private int currentIndex = 0;

    void Start()
    {
    
        GameManager.Instance.ChangeBiome += ChangeSkybox;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ChangeBiome -= ChangeSkybox;
    }

    void Update()
    {
      
    }

    void ChangeSkybox(BiomeName biome)
    {
        switch (biome)
        {
            case BiomeName.Elysee : currentIndex = 0;
                break;
            case BiomeName.Styx :currentIndex = 1;
                break;
            case BiomeName.Tartare :currentIndex = 3;
                break;
            case BiomeName.ChampsDesChatiments :currentIndex = 2;
                break;
            default: currentIndex = 0;
                break;
        }
        RenderSettings.skybox = skyboxes[currentIndex];
        DynamicGI.UpdateEnvironment(); // Pour recharger l’éclairage global (optionnel mais conseillé)
    }
}