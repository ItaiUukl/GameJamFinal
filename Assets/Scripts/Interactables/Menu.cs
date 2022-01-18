using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // Starts the game, load first level scene
    public void StartGame()
    {    
        RoomsManager.Instance.ResetLevel();
        Debug.Log(1);
        SceneManager.LoadScene(2);
    }
    public void LevelSelect()
    {    
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene("levelSelect");
    }
}