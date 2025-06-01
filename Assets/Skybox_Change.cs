using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] skyboxes; // Assigne-les dans l'inspecteur ou charge-les dynamiquement
    private int currentIndex = 0;

    void Start()
    {
        if (skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxes[currentIndex];
        }
    }

    void Update()
    {
        // Exemple : appuyer sur la touche "S" pour changer
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSkybox();
        }
    }

    void ChangeSkybox()
    {
        currentIndex = (currentIndex + 1) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentIndex];
        DynamicGI.UpdateEnvironment(); // Pour recharger l’éclairage global (optionnel mais conseillé)
    }
}