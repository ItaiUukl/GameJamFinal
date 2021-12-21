using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class LevelsSO : ScriptableObject
{
    public List<UnityEngine.Object> levelAdvancement;
    [NonSerialized] public int currLevel = 0;

    public string NextLevel()
    {
        currLevel = (currLevel + 1) % levelAdvancement.Count;
        return levelAdvancement[currLevel].name;
    }
}
