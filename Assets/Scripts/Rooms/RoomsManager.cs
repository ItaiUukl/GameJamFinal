using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    private List<Room> rooms;

    // Registers a new room to 
    public void RegisterRoom(Room room)
    {
        rooms.Add(room);
    }

    // Resets the current level
    public void ResetLevel()
    {
        rooms.Clear();
        // TODO: implement
    }
}