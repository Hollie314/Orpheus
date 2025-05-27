using UnityEngine;
using UnityEngine.SceneManagement;

public class HUB : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(2); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}