using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static GlobalsSO Globals;
    private int _currLevel = 0;
    private int _selectedLevel = 0;
    public static CameraTransitions Cam = null;
    private PlayerInput _inputSystem;

    private bool _menu = false;

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
        if (_menu) SetLevel(_selectedLevel - 1);
        Cam.ExitTransition(true);
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
        SetLevel(_currLevel + (int) value.Get<float>());
    }

    public void NextLevel() => SetLevel(_currLevel + 1);

    public void SetLevel(int lvl)
    {
        RoomsManager.Instance.ResetLevel();
        _currLevel = Math.Max(0, lvl) % Globals.levelAdvancement.Count;
        SceneManager.LoadScene(Globals.AdvanceLevel(_currLevel));
        PlayerPrefs.SetInt("currLevel", _currLevel);
    }

    // Resets the current level
    public void ReloadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(Globals.AdvanceLevel(_currLevel));
    }

    public void SetMenuLevel(string index)
    {
        _menu = int.TryParse(index, out _selectedLevel);
        Debug.Log(_menu ? "Convertion done" : "Convertion failed");
    }
}