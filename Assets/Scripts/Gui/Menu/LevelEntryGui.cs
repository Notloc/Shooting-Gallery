using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelEntryGui : MonoBehaviour
{
    [SerializeField] Text levelName;
    [SerializeField] Button button;
    [SerializeField] Image image;

    private Action<LevelDescription> SelectionFunction;
    private LevelDescription level;

    public void AssignLevel(LevelDescription level, Action<LevelDescription> OnSelect)
    {
        this.level = level;

        levelName.text = level.DisplayName;
        SelectionFunction = OnSelect;
        image.sprite = level.Image;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SelectionFunction(level);
    }
}
