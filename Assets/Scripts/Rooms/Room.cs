using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoomsManager.Instance.RegisterRoom(this);
        // TODO: implement. Generates colliders
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // TODO: implement. Shuts off appropriate colliders
    }

    // Moves room until collision
    public void Move(Vector2 dir)
    {
        // TODO: implement
    }
    
}
