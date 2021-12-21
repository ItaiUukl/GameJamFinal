using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    private GameObject child;

    public int RoomGroup { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Rooms");
        RoomsManager.Instance.RegisterRoom(this);
        // TODO: implement. Generates colliders
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision happened");
        // var p1 = other.GetContact(0).point;
        // var p2 = other.GetContact(other.contactCount - 1).point;
        // var edgeColliders = child.GetComponents<EdgeCollider2D>();
        // var newPoints = new List<Vector2>();
        // bool firstEdgeFractionFound = false;
        // var collidersToAdd = new List<List<Vector2>>();
        // foreach (var edgeCol in edgeColliders)
        // {
        //     for (int i = 0; i < edgeCol.pointCount - 1; ++i)
        //     {
        //         newPoints.Add(edgeCol.points[i]);
        //         if (firstEdgeFractionFound) continue;
        //         if (IsOnAB(edgeCol.points[i], edgeCol.points[i + 1], p1))
        //         {
        //             SplitCollider(edgeCol, i, newPoints, p1, p2);
        //             firstEdgeFractionFound = true;
        //         }   
        //         if (IsOnAB(edgeCol.points[i], edgeCol.points[i + 1], p2))
        //         {
        //             SplitCollider(edgeCol, i, newPoints, p2, p1);
        //             firstEdgeFractionFound = true;
        //         }
        //     }
        //     collidersToAdd.Add(newPoints.ToList());
        //     firstEdgeFractionFound = false;
        //     newPoints.Clear();
        // }
        // foreach (var pts in collidersToAdd)
        // {
        //     child.AddComponent<EdgeCollider2D>().SetPoints(pts);
        // }
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