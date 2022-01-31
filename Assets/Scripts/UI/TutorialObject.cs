using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private GameObject keyboardSprite;
    [SerializeField] private GameObject controllerSprite;

    // Start is called before the first frame update
    void Start()
    {
        keyboardSprite.SetActive(!GameManager.IsGamepadConnected);
        controllerSprite.SetActive(GameManager.IsGamepadConnected);
    }
}