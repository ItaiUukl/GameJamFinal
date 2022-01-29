using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public int? DoorPlayerAt = null;
    private Animator _animator;
    private static readonly int AnimatorOnPress = Animator.StringToHash("On Press");
    private bool _onLevelSelectScreen = false;
    private Player _player;

    private void Start()
    {
        GameManager.Instance.Init();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        if (_player)
        {
            _player.IsActive = false;
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
        if (!value.isPressed || !_onLevelSelectScreen) return;
        if (DoorPlayerAt != null)
        {
            GameManager.Instance.SetLevel((int) DoorPlayerAt - 1);
        }
    }

    private void OnTransitionFinished()
    {
        
        if (_player)
        {
            _player.EnterLevel();
        }
    }
}