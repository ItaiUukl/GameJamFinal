using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    private bool _wasActivated = false;
    public GameObject[] doors_array;
    private GlobalsSO globalSO;
   
    private void Start()
    {
        int currLevel = PlayerPrefs.GetInt("currLevel", 1);
        for(int i = 0; i < doors_array.Length; i++){
            //if(i + 2 > int.Parse(GlobalsSO.AdvanceLevel(i))
                doors_array[i].SetActive(false);
        }
    }
    // Called when level is completed. Switches to next level with UI, etc.
    private void SelectLevel(string stringScene) => SceneManager.LoadScene(stringScene);

}