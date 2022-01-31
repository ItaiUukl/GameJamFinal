#nullable enable
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public LevelSelection? doorPlayerAt = null;
    private Animator _animator;

    private static readonly int AnimatorOnPress = Animator.StringToHash("On Press"),
        AnimatorEsc = Animator.StringToHash("Esc"),
        AnimatorIdle = Animator.StringToHash("Idle");

    private bool _onLevelSelectScreen = false;
    private Player _player;

    private void Start()
    {
        bool firstInit = GameManager.Instance.Init();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _onLevelSelectScreen = !firstInit;
        if (!firstInit)
        {
            _animator.SetTrigger(AnimatorIdle);
        }
        else if (_player)
        {
            _player.Freeze();
        }
    }

    private void OnPlay(InputValue value)
    {
        if (!value.isPressed || _onLevelSelectScreen) return;
        _onLevelSelectScreen = true;
        Debug.Log("play");
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
        Debug.Log("esc from lvl select" + _onLevelSelectScreen);
        if (!value.isPressed) return;
        if (_onLevelSelectScreen)
        {
            _animator.SetTrigger(AnimatorEsc);
            _onLevelSelectScreen = false;
            // todo: reset player
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

    private void OnTransitionFinished()
    {
        if (_player)
        {
            _player.EnterLevel();
        }
    }
}