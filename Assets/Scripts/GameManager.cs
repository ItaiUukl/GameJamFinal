using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] public Scene[] levels;
    
    // Advances game to the next level
    public void NextLevel()
    {
        // TODO: implement
    }

    // Resets the current level
    public void ResetLevel()
    {
        // TODO: implement
    }
}
