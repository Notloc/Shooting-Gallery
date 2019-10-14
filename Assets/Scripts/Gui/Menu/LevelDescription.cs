using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/New LevelDescription")]
public class LevelDescription : ScriptableObject
{
    [SerializeField] string levelName;
    [SerializeField] string displayName;
    [SerializeField] Sprite levelImage;

    public string LevelName { get { return levelName; } }
    public string DisplayName { get { return displayName; } }
    public Sprite Image { get { return levelImage; } }
}
