using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    [SerializeField] private Collider2D player;
    private List<Room> rooms;
    private HashSet<Room> sharedRooms = new HashSet<Room>();

    // Registers a new room to Int
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
    
    public void RoomConnection(Room room)
    {
        sharedRooms.Add(room);
        RecalculateRoomsCollider();
    }
    
    public void RoomDisconnection(Room room)
    {
        sharedRooms.Remove(room);
        RecalculateRoomsCollider();
    }

    private void RecalculateRoomsCollider()
    {
        // TODO: implement
    }
}