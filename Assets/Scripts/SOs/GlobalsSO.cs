using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levels", menuName = "Levels Order")]
public class GlobalsSO : ScriptableObject
{
    public List<Object> levelAdvancement;

    public const string RoomsLayer = "Rooms", BorderLayer = "Border", OutlinesLayer = "Outline", PlayerLayer = "Player";

    public string AdvanceLevel(int currLevel)
    {
        return levelAdvancement[currLevel].name;
    }
}