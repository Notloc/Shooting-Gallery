using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the string name of the next level
/// </summary>
public class NextLevel : MonoBehaviour
{
    [SerializeField] string nextLevelName;
    public string LevelName { get { return nextLevelName; } }
}
