#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Room elevator;
    public LevelSelection? doorPlayerAt = null;
    private Animator _animator;
    private Vector3 elevatorInitPos;

    private static readonly int AnimatorOnPress = Animator.StringToHash("On Press"),
        AnimatorEsc = Animator.StringToHash("Esc"),
        AnimatorIdle = Animator.StringToHash("Idle");

    private bool _onLevelSelectScreen = false;
    private Player _player;

    private void Start()
    {
        elevatorInitPos = elevator.transform.localPosition;
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        bool hasGameStarted = GameManager.Instance.HasGameStarted;
        _onLevelSelectScreen = hasGameStarted;
        if (_onLevelSelectScreen)
        {
            _animator.SetTrigger(AnimatorIdle);
        }
        else
        {
            _player.IsPaused = true;
        }
    }

    private void OnPlay(InputValue value)
    {
        if (!value.isPressed || _onLevelSelectScreen) return;
        _onLevelSelectScreen = true;
        _animator.SetBool(AnimatorOnPress, true);
    }

    private void OnStartLevel(InputValue value)
    {
        if (!value.isPressed || !_onLevelSelectScreen || doorPlayerAt == null) return;
        doorPlayerAt.OnDoorEnter(_player.transform);
        _player.MoveTowards(doorPlayerAt.PlayerDest, OnPlayerEnteredDoor);
    }

    private void OnExit(InputValue value)
    {
        if (!value.isPressed) return;
        if (_onLevelSelectScreen)
        {
            _animator.SetTrigger(AnimatorEsc);
            _onLevelSelectScreen = false;
        }
        else
        {
            Application.Quit();
        }
    }

    private void OnPlayerEnteredDoor()
    {
        if (doorPlayerAt == null) return;
        GameManager.Instance.SetLevel(doorPlayerAt.levelNumber - 1);
    }
    
    private void OnTransitionToOpenScreenFinished()
    {
        _player.Vanish();
        elevator.transform.localPosition = elevatorInitPos;
        // elevator.Move(MoveDirection.Down);
    }
    
    private void OnTransitionToLevelSelectionFinished()
    {
        _player.EnterLevel();
    }
}