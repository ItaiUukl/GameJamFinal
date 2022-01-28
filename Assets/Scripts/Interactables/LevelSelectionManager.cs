using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public int? DoorPlayerAt = null;

    private void OnStartLevel(InputValue value)
    {
        if (!value.isPressed) return;
        if (DoorPlayerAt != null)
        {
            GameManager.Instance.SetLevel((int) DoorPlayerAt - 1);
            // GameManager.Cam.ExitTransition(false);
        }
    }
}