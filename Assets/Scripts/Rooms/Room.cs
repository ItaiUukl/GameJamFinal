using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    private GameObject child;
    // Start is called before the first frame update
    void Start()
    {
        // RoomsManager.Instance.RegisterRoom(this);
        // TODO: implement. Generates colliders
        CreateEdgeCollider();
    }

    private void CreateEdgeCollider()
    {
        var poly = GetComponent<PolygonCollider2D>();
        var position = transform.position;
        child = new GameObject
        {
            // TODO: Add layers references/string names to Utils
            layer = LayerMask.NameToLayer("Default"),
            transform =
            {
                position = position,
                rotation = transform.rotation,
                localScale = transform.localScale
            }
        };
        // var worldPointsList = poly.points.Select(t => poly.transform.TransformPoint(t))
        //     .Select(worldSpacePoint => (Vector2) worldSpacePoint).ToList();
        var edgeCol = child.AddComponent<EdgeCollider2D>();
        var listOfPoints = poly.GetPath(0).ToList();
        listOfPoints.Add(listOfPoints[0]);
        edgeCol.SetPoints(listOfPoints);
        child.transform.parent = transform;
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