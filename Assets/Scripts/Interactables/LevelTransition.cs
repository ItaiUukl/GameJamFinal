using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            CompleteLevel();
        }
    }

    // Called when level is completed. Switches to next level with UI, etc.
    public void CompleteLevel()
    {
        // TODO: exit animation (maybe should be in game manager)
        GameManager.Instance.NextLevel();
    }
}
