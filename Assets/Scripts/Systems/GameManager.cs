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

    private bool _hasGameStarted = false;
    public bool HasGameStarted
    {
        get
        {
            if (_hasGameStarted) return true;
            _hasGameStarted = true;
            return false;
        }
    }
    public static bool IsGamepadConnected => InputSystem.GetDevice(typeof(Gamepad)) is Gamepad;

    public bool IsInMainMenu => SceneManager.GetActiveScene().name == Globals.mainMenuSceneName;

    public int maxUnlockedLevel; // first level is 1 (not an index)

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void DisableCursor()
    {
        if (Application.isEditor) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        Globals = Resources.LoadAll<GlobalsSO>("Globals")[0];
        maxUnlockedLevel = PlayerPrefs.GetInt("currLevel", 1);
        Debug.Log("max lvl is " + maxUnlockedLevel);
    }

    /**
     * returns false if it is it's first call, else true.
     */
    public void Init()
    {
    }

    private void Start()
    {
        _inputSystem = gameObject.AddComponent<PlayerInput>();
        _inputSystem.actions = Globals.inputAction;
        _inputSystem.defaultActionMap = "General";
        _inputSystem.notificationBehavior = PlayerNotifications.SendMessages;
        _inputSystem.actions.Enable();

        AudioManager.Instance.Play("Music");
    }

    private void OnReset(InputValue value)
    {
        if (!value.isPressed || IsInMainMenu) return;
        ReloadLevel();
        AudioManager.Instance.Play("Restart Level");
    }

    private void OnResetMaxLevel(InputValue value)
    {
        if (!value.isPressed) return;
        PlayerPrefs.SetInt("currLevel", 1);
        maxUnlockedLevel = 1;
        _hasGameStarted = false;
        SetMainMenu(false);
    }

    private void OnExit(InputValue value)
    {
        if (!value.isPressed || IsInMainMenu) return;
        SetMainMenu(false);
    }

    private void OnSwitchLevel(InputValue value)
    {
        if (IsInMainMenu) return;
        SetLevel(_currLevelIdx + Math.Sign(value.Get<float>()));
    }

    public void SetMainMenu(bool isCalledFromEndScene)
    {
        _currLevelIdx = -1;
        Cam.ExitTransition(isCalledFromEndScene);
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

        Cam.ExitTransition(false);
    }

    // Resets the current level
    public void LoadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(_currLevelIdx >= 0 ? Globals.AdvanceLevel(_currLevelIdx) : Globals.mainMenuSceneName);
    }
}