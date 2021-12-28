using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveDirection
{
    Up,
    Right,
    Down,
    Left
}

public class Lever : MonoBehaviour
{
    [SerializeField] private MoveDirection direction;
    [SerializeField] private Room room;

    private Collider2D _collider;
    private Vector2 _vecDir;
    private bool _active;

    // Start is called before the first frame update
    void Start()
    {
        room = room ? room : GetComponentInParent<Room>();
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        
        switch (direction)
        {
            case MoveDirection.Up:
                _vecDir = Vector2.up;
                break;
            case MoveDirection.Right:
                _vecDir = Vector2.right;
                break;
            case MoveDirection.Down:
                _vecDir = Vector2.down;
                break;
            case MoveDirection.Left:
                _vecDir = Vector2.left;
                break;
        }
    }

    private void Update()
    {
        // TODO: much more efficient to have the active levers register to an event and only game manager to check input.
        if (_active && Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Activate();
        }
    }

    // Activates lever
    public void Activate()
    {
        room.Move(_vecDir);
        // RoomsManager.Instance.MoveRoom(room, _vecDir);
        // TODO: Trigger lever animation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _active = false;
        }
    }
}