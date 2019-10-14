using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Button nextLevel;
    [SerializeField] Button mainMenu;

    [SerializeField] string mainMenuSceneName;

    void Awake()
    {
        nextLevel.onClick.AddListener(NextLevel);
        mainMenu.onClick.AddListener(MainMenu);
    }


    private void NextLevel()
    {
        string sceneName = mainMenuSceneName;
        var obj = GameObject.FindGameObjectWithTag("NextLevel");
        if (obj)
            sceneName = obj.GetComponent<NextLevel>().LevelName;

        SceneManager.LoadScene(sceneName);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

}
