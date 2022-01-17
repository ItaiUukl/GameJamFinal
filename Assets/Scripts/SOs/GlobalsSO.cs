using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class GlobalsSO : ScriptableObject
{
    [FormerlySerializedAs("stringAdvancement")]
    public List<string> levelAdvancement;

    public InputActionAsset inputAction;

    [Header("Outline")] public float outlineWidth = .2f;
    public LineTextureMode outlineTextureMode = LineTextureMode.Tile;
    public float outlineAnimationSpeed = .15f;
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