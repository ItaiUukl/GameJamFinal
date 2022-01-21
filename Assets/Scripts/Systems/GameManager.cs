using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum MoveDirection
{
    Up,
    Right,
    Down,
    Left
}

public class GameManager : Singleton<GameManager>
{
    public static GlobalsSO Globals;
    private int _currLevel = 0;
    public static CameraTransitions Cam = null;
    private PlayerInput _inputSystem;

    private void Awake()
    {
        Globals = Resources.LoadAll<GlobalsSO>("Globals")[0];
    }

    public void DoNothing()
    {
    } // TODO: delete

    private void Start()
    {
        _inputSystem = gameObject.AddComponent<PlayerInput>();
        _inputSystem.actions = Globals.inputAction;
        _inputSystem.defaultActionMap = "General";
        _inputSystem.notificationBehavior = PlayerNotifications.SendMessages;
        _inputSystem.actions.Enable();
    }

    private void OnReset(InputValue value)
    {
        Debug.Log("reset");
        if (!value.isPressed) return;
        Cam.ExitTransition(true);
    }

    private void OnExit(InputValue value)
    {
        if (!value.isPressed) return;
        Application.Quit();
    }

    public static Vector2 GetDirection(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => Vector2.up,
            MoveDirection.Right => Vector2.right,
            MoveDirection.Down => Vector2.down,
            MoveDirection.Left => Vector2.left,
            _ => Vector2.zero
        };
    }
    
    public static MoveDirection OppositeDirection(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => MoveDirection.Down,
            MoveDirection.Right => MoveDirection.Left,
            MoveDirection.Down => MoveDirection.Up,
            MoveDirection.Left => MoveDirection.Right,
            _ => MoveDirection.Up
        };
    }

    private void OnSwitchLevel(InputValue value)
    {
        SetLevel(_currLevel + (int) value.Get<float>());
    }

    public void NextLevel() => SetLevel(_currLevel + 1);

    public void SetLevel(int lvl)
    {
        RoomsManager.Instance.ResetLevel();
        _currLevel = Math.Max(0, lvl) % Globals.levelAdvancement.Count;
        SceneManager.LoadScene(Globals.AdvanceLevel(_currLevel));
    }

    // Resets the current level
    public void ReloadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(Globals.AdvanceLevel(_currLevel));
    }
}