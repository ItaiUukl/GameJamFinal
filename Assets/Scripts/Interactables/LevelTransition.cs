using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour
{
    private CameraTransitions _camera;
    
    private bool _wasActivated = false;
    
    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _camera = FindObjectOfType<CameraTransitions>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_wasActivated && other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            CompleteLevel();
        }
    }

    // Called when level is completed. Switches to next level with UI, etc.
    private void CompleteLevel()
    {
        Debug.Log("next level");
        _wasActivated = true;
        _camera.ExitTransition(false);
    }
}
