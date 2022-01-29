using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public int? DoorPlayerAt = null;
    private Animator _animator;
    private static readonly int AnimatorOnPress = Animator.StringToHash("On Press");

    private void Start()
    {
        GameManager.Instance.Init();
        _animator = GetComponent<Animator>();
    }
    
    private void OnPlay(InputValue value)
    {
        if (!value.isPressed) return;
        Debug.Log("play");
        _animator.SetBool(AnimatorOnPress, true);
    }

    private void OnStartLevel(InputValue value)
    {
        if (!value.isPressed) return;
        if (DoorPlayerAt != null)
        {
            GameManager.Instance.SetLevel((int) DoorPlayerAt - 1);
        }
    }
}