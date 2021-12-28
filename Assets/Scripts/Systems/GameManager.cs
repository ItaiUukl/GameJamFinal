using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GlobalsSO _globals;
    private int _currLevel = 0;
    
    private void Awake()
    {
        _globals = Resources.LoadAll<GlobalsSO>("Globals")[0];
    }

    public void NextLevel()
    {
        RoomsManager.Instance.ResetLevel();
        _currLevel = (_currLevel + 1) % _globals.levelAdvancement.Count;
        SceneManager.LoadScene(_globals.AdvanceLevel(_currLevel));
    }
}
