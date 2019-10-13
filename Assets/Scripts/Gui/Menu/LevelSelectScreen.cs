using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MenuScreen
{
    [SerializeField] LevelSelector levelSelector;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button backButton;

    private LevelDescription selectedLevel;
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
        backButton.onClick.AddListener(ReturnToPrevious);
        levelSelector.Initialize(OnSelect);
    }

    private void OnEnable()
    {
        playButton.interactable = false;
        selectedLevel = null;
    }

    private void OnSelect(LevelDescription level)
    {
        selectedLevel = level;
        playButton.interactable = (selectedLevel != null);
    }

    private void OnPlay()
    {
        menuController.Play(selectedLevel);
    }
}
