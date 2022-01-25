using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour
{
    public bool menu = false;
    private bool _wasActivated = false;
    string stringIndex;
    string stringScene;

    private void Start()
    {
        stringScene = gameObject.name;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (!_wasActivated && other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            if(menu){
                //Activating the sprite above menu door;
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                // getting the last string char to set the level;
                stringIndex = stringScene.Substring(stringScene.Length - 1);
                GameManager.Instance.SetMenuLevel(stringIndex);
            }
            else
                CompleteLevel();
        }
    }
    //Deativating the sprite above menu door;
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(menu) gameObject.transform.GetChild(4).gameObject.SetActive(false);
    }

    // Called when level is completed. Switches to next level with UI, etc.
    private void CompleteLevel()
    {
        Debug.Log("next level");
        _wasActivated = true;
        GameManager.Cam.ExitTransition(false);
    }

}