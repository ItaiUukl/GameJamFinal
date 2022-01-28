using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static GlobalsSO Globals;
    private int _currLevelIdx = 0;
    public static CameraTransitions Cam = null;

    private PlayerInput _inputSystem;
    public static bool IsGamepadConnected { get; private set; }

    public int maxUnlockedLevel; // first level is 1 (not an index)

    private void Awake()
    {
        Globals = Resources.LoadAll<GlobalsSO>("Globals")[0];
        maxUnlockedLevel = PlayerPrefs.GetInt("currLevel", 1);
        Debug.Log("max lvl is " + maxUnlockedLevel);
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

        Debug.Log("Keyboard connected = " + (InputSystem.GetDevice(typeof(Keyboard)) is Keyboard));
        Debug.Log("Gamepad connected = " + (InputSystem.GetDevice(typeof(Gamepad)) is Gamepad));
        Debug.Log("Joystick connected = " + (InputSystem.GetDevice(typeof(Gamepad)) is Joystick));
        IsGamepadConnected = InputSystem.GetDevice(typeof(Gamepad)) is Gamepad;

        AudioManager.Instance.Play("Music");
    }

    private void OnReset(InputValue value)
    {
        Debug.Log("reset");
        if (!value.isPressed) return;
        ReloadLevel();
        AudioManager.Instance.Play("Restart Level");
    }

    private void OnExit(InputValue value)
    {
        if (!value.isPressed) return;
        if (SceneManager.GetActiveScene().name == Globals.mainMenuSceneName)
        {
            Application.Quit();
        }
        else
        {
            RoomsManager.Instance.ResetLevel();
            SceneManager.LoadScene(Globals.mainMenuSceneName);
        }
    }

    private void OnSwitchLevel(InputValue value)
    {
        SetLevel(_currLevelIdx + Math.Sign(value.Get<float>()));
    }

    // advances game to the next level
    public void NextLevel() => SetLevel(_currLevelIdx + 1);

    // Resets the current level
    private void ReloadLevel() => SetLevel(_currLevelIdx);

    public void SetLevel(int lvl)
    {
        Debug.Log("starting lvl " + (lvl + 1));
        _currLevelIdx = Math.Max(0, lvl) % Globals.levelAdvancement.Count;
        if (maxUnlockedLevel < _currLevelIdx + 1)
        {
            maxUnlockedLevel = _currLevelIdx + 1;
            PlayerPrefs.SetInt("currLevel", _currLevelIdx + 1);
        }

        Cam.ExitTransition();
    }

    // Resets the current level
    public void LoadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(Globals.AdvanceLevel(_currLevelIdx));
    }
}