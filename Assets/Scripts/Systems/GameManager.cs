using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] public Scene[] levels;
    private int CurLevel = 0;
    
    // Advances game to the next level
    public void NextLevel()
    {
        SceneManager.LoadScene(levels[++CurLevel].buildIndex);
        
        // TODO: implement
    }

    // Resets the current level
   public void ReloadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(_levels.CurrentLevel());
    }
}
