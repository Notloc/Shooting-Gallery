using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Button playAgain;
    [SerializeField] Button mainMenu;

    [SerializeField] string mainMenuSceneName;

    void Awake()
    {
        playAgain.onClick.AddListener(PlayAgain);
        mainMenu.onClick.AddListener(MainMenu);
    }


    private void PlayAgain()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

}
