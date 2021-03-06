using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour
{
    private static readonly int AnimatorOpen = Animator.StringToHash("Open");

    [SerializeField] private GameObject rightMask, leftMask;

    private bool _wasActivated = false;
    private Animator _animator;
    private GameObject _mask;
    private Player _player;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
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
        _mask = _player.transform.position.x > transform.position.x ? leftMask : rightMask;
        _mask.SetActive(true);
        _player.InTransition = true;
        _animator.SetTrigger(AnimatorOpen);

        _wasActivated = true;
    }

    public void OnDoorOpened()
    {
        _player.MoveTowards(_mask.transform.position.x, OnPlayerEnteredDoor);
    }

    private void OnPlayerEnteredDoor()
    {
        GameManager.Instance.NextLevel();
    }
}