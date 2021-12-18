using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Lever : MonoBehaviour
{
    [SerializeField] private Vector2 dir;
    [SerializeField] private Room room;

    // Start is called before the first frame update
    void Start()
    {
        room = room == null ? GetComponentInParent<Room>() : room;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Activates lever
    public void Activate()
    {
        room.Move(dir);
        // TODO: Trigger lever animation
    }
}