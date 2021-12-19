using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    [SerializeField] private Collider2D player;
    private List<Room> rooms;

    // Registers a new room to 
    public void RegisterRoom(Room room)
    {
        rooms.Add(room);
        // Physics2D.IgnoreCollision(room.GetComponent<PolygonCollider2D>(), player);
    }

    // Resets the current level
    public void ResetLevel()
    {
        rooms.Clear();
        // TODO: implement
    }
}