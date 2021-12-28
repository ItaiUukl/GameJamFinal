using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private LevelsSO _levels;
    [SerializeField] public Player player;
    
    private void Awake()
    {
        _levels = Resources.LoadAll<LevelsSO>("Globals")[0];
    }

    public void NextLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(_levels.NextLevel());
    }

    // Resets the current level
   public void ReloadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(_levels.CurrentLevel());
    }
}
