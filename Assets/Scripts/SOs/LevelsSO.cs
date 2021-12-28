using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class LevelsSO : ScriptableObject
{
    public List<UnityEngine.Object> levelAdvancement;
    [NonSerialized] public int currLevel = 0;
    //[NonSerialized] public int nextLevel = 0;


    public string NextLevel()
    {
        currLevel = (currLevel + 1) % levelAdvancement.Count;
        return levelAdvancement[currLevel].name;
    }
    
     public string CurrentLevel()
    {
        return levelAdvancement[currLevel].name;
    }
    /*
    public Scene GetNextScene(){
        nextLevel = currLevel + 1;
        int nextSceneI = SceneManager.GetSceneByBuildIndex(nextLevel).buildIndex;
        Scene nextScene = GetSceneByBuildIndex(nextSceneI);
        return nextScene;
    }
     public Scene GetCurrentScene(){
        int currSceneI = SceneManager.GetSceneByBuildIndex(currLevel).buildIndex;
        Scene currnentScene = GetSceneByBuildIndex(currSceneI);
        return currnentScene;
    }*/
}
