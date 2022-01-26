using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem;

public class LevelSelection : MonoBehaviour
{
    private bool _wasActivated = false;
    public GameObject[] doors_array;
    private GlobalsSO globalSO;
   
    private void Start()
    {
        //PlayerPrefs.SetInt("currLevel", 1);
        int currLevel = PlayerPrefs.GetInt("currLevel", 1);
        for(int i = 0; i < currLevel; i++){
            //if(i + 2 > int.Parse(GlobalsSO.AdvanceLevel(i))
            doors_array[i].SetActive(true);
        }
    }
    // Called when level is completed. Switches to next level with UI, etc.
    private void SelectLevel(string stringScene) => SceneManager.LoadScene(stringScene);
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (!_wasActivated && other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

}