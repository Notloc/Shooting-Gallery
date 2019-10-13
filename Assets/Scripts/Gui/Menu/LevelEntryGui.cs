using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelEntryGui : MonoBehaviour
{
    [SerializeField] Text levelName;
    [SerializeField] Button button;

    private Action<LevelDescription> SelectionFunction;
    private LevelDescription level;

    public void AssignLevel(LevelDescription level, Action<LevelDescription> OnSelect)
    {
        this.level = level;

        levelName.text = level.LevelName;
        SelectionFunction = OnSelect;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SelectionFunction(level);
    }
}
