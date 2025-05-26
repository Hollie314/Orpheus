using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelMenu: MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelButtons; 
    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevels; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelID)
    {
        string levelName = "level" + levelID;
        SceneManager.LoadScene(levelName);
    }

    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
}
