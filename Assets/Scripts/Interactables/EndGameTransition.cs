using UnityEngine;

public class EndGameTransition : MonoBehaviour
{
    private bool _wasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_wasActivated || other.gameObject.layer != LayerMask.NameToLayer(GlobalsSO.PlayerLayer)) return;
        _wasActivated = true;
        AudioManager.Instance.Stop("EndingNote");
        AudioManager.Instance.Play("Music");
        GameManager.Instance.SetMainMenu(true);
    }
}