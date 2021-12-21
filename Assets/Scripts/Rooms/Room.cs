using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    public int RoomGroup { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Rooms");
        RoomsManager.Instance.RegisterRoom(this);
        GetComponent<PolygonCollider2D>().isTrigger = true;
        
        // TODO: implement. Generates colliders
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rooms"))
        {
            Room room = other.GetComponent<Room>();
            
            if (room)
            {
                RoomsManager.Instance.RoomConnection(this, room);
            }
        }
    }

    private void SplitCollider(EdgeCollider2D edgeCol, int i, List<Vector2> newPoints, Vector2 A, Vector2 B)
    {
        newPoints.Add(A);
        edgeCol.points = newPoints.ToArray();
        newPoints.Clear();
        newPoints.Add(edgeCol.points[i+1]);
        newPoints.Add(B);
    }

    private bool IsOnAB(Vector2 A, Vector2 B, Vector2 C)
    {
        return Vector2.Dot((B - A).normalized, (C - B).normalized) <= 0f &&
               Vector2.Dot((A - B).normalized, (C - A).normalized) <= 0f;
    }

    // Moves room until collision
    public void Move(Vector2 dir)
    {
        // TODO: implement
    }
}