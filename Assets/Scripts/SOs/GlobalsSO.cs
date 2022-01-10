using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class GlobalsSO : ScriptableObject
{
    public List<Object> levelAdvancement;
    public List<string> stringAdvancement;


    public const string RoomsLayer = "Rooms",
        BorderLayer = "Border",
        OutlinesLayer = "Outline",
        PlayerLayer = "Player",
        DefaultLayer = "Default";

    public string AdvanceLevel(int currLevel)
    {
        return stringAdvancement[currLevel];
    }
}