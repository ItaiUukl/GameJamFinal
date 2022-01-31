using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    [SerializeField] public int levelNumber;

    [SerializeField] private GameObject tutorButton,
        unlockedSprite,
        lockedSprite,
        rightMask,
        leftMask;

    private MainMenuManager _manager;
    private bool _isLocked = true;
    private GameObject _mask;
    private bool _wasActivated = false;
    public float PlayerDest => _mask.transform.position.x;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _manager = FindObjectOfType<MainMenuManager>();
        tutorButton.SetActive(false);
        _isLocked = GameManager.Instance.maxUnlockedLevel < levelNumber;
        unlockedSprite.SetActive(!_isLocked);
        lockedSprite.SetActive(_isLocked);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_wasActivated || _isLocked ||
            other.gameObject.layer != LayerMask.NameToLayer(GlobalsSO.PlayerLayer)) return;
        // Activating the sprite above menu door;
        tutorButton.SetActive(true);
        _manager.doorPlayerAt = this;
    }

    // Deactivating the sprite above menu door;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_wasActivated) return;
        tutorButton.SetActive(false);
        _manager.doorPlayerAt = null;
    }

    public void OnDoorEnter(Transform player)
    {
        _mask = player.position.x > transform.position.x ? leftMask : rightMask;
        _wasActivated = true;
        _mask.SetActive(true);
    }
}