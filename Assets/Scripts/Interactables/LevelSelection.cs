using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private GameObject tutorButton;
    [SerializeField] private Sprite unlockedSprite, lockedSprite;
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private SpriteRenderer[] numberSprites;
    private LevelSelectionManager _manager;
    private bool _isLocked = true;

    private void Start()
    {
        Debug.Log("created door " + name);
        GetComponent<Collider2D>().isTrigger = true;
        _manager = FindObjectOfType<LevelSelectionManager>();
        tutorButton.SetActive(false);
        _isLocked = GameManager.Instance.maxUnlockedLevel < levelNumber;

        // set door sprite
        doorSprite.sprite = _isLocked ? lockedSprite : unlockedSprite;
        foreach (var sr in numberSprites)
        {
            sr.color = _isLocked ? GameManager.Globals.doorBlueColor : GameManager.Globals.doorOrangeColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isLocked || other.gameObject.layer != LayerMask.NameToLayer(GlobalsSO.PlayerLayer)) return;
        // Activating the sprite above menu door;
        tutorButton.SetActive(true);
        _manager.DoorPlayerAt = levelNumber;
    }

    // Deactivating the sprite above menu door;
    private void OnTriggerExit2D(Collider2D other)
    {
        tutorButton.SetActive(false);
        _manager.DoorPlayerAt = null;
    }
}