using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    [SerializeField] public int levelNumber;
    [SerializeField] private GameObject tutorButton;
    [SerializeField] private GameObject unlockedSprite, lockedSprite;
    private LevelSelectionManager _manager;
    private bool _isLocked = true;
    private SpriteMask _mask;
    public bool wasActivated = false;
    public float PlayerDest => _mask.transform.position.x;

    private void Start()
    {
        Debug.Log("created door " + name);
        GetComponent<Collider2D>().isTrigger = true;
        _manager = FindObjectOfType<LevelSelectionManager>();
        _mask = GetComponentInChildren<SpriteMask>();
        tutorButton.SetActive(false);
        _isLocked = GameManager.Instance.maxUnlockedLevel < levelNumber;
        unlockedSprite.SetActive(!_isLocked);
        lockedSprite.SetActive(_isLocked);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wasActivated || _isLocked || other.gameObject.layer != LayerMask.NameToLayer(GlobalsSO.PlayerLayer)) return;
        // Activating the sprite above menu door;
        tutorButton.SetActive(true);
        _manager.DoorPlayerAt = this;
    }

    // Deactivating the sprite above menu door;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (wasActivated) return;
        tutorButton.SetActive(false);
        _manager.DoorPlayerAt = null;
    }

    public void OnDoorEnter()
    {
        wasActivated = _mask.enabled = true;
    }
}