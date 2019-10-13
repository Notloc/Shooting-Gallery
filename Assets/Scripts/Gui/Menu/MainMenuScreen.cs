using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MenuScreen
{
    [SerializeField] MenuScreen levelSelectScreen;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;


    private void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnPlay()
    {
        this.gameObject.SetActive(false);
        levelSelectScreen.gameObject.SetActive(true);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
