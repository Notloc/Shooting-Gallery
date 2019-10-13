using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/New LevelDescription")]
public class LevelDescription : ScriptableObject
{
    [SerializeField] string levelName;
    public string LevelName { get { return levelName; } }
}
