using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    private bool _wasActivated = false;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string sceneString = gameObject.name;
        Debug.Log(sceneString);
        if (!_wasActivated && other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            string stringindex = sceneString.Substring(sceneString.Length - 1);
            GameManager.Instance.SetLevel(int.Parse(stringindex));
            SelectLevel(sceneString);
        }
    }

    // Called when level is completed. Switches to next level with UI, etc.
    private void SelectLevel(string sceneString) => SceneManager.LoadScene(sceneString);

}