using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSelector : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] RectTransform levelEntryParent;
    [SerializeField] LevelEntryGui levelEntryPrefab;
    [Space]
    [SerializeField] LevelDescription[] levels;

    public void Initialize(Action<LevelDescription> OnSelect)
    {
        foreach(var level in levels)
        {
            CreateLevelEntry(level, OnSelect);
        }
    }

    private void CreateLevelEntry(LevelDescription level, Action<LevelDescription> OnSelect)
    {
        var newLevel = Instantiate(levelEntryPrefab, levelEntryParent);
        newLevel.AssignLevel(level, OnSelect);
    }
}
