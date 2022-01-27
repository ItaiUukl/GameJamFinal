using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class GlobalsSO : ScriptableObject
{
    [FormerlySerializedAs("stringAdvancement")]
    public string mainMenuSceneName;

    public List<string> levelAdvancement;

    public InputActionAsset inputAction;

    public float maxSlowRoomSpeed = 10;

    [Header("Outline")] public float outlineWidth = .2f;
    public LineTextureMode outlineTextureMode = LineTextureMode.Tile;
    public float outlineAnimationSpeed = .15f;
    public Color MenuColor = Color.white;
    public List<Material> outlineMaterials;


    public const string RoomsLayer = "Rooms",
        BorderLayer = "Border",
        OutlinesLayer = "Outline",
        PlayerLayer = "Player",
        DefaultLayer = "Default";

    public string AdvanceLevel(int currLevel)
    {
        return levelAdvancement[currLevel];
    }
}