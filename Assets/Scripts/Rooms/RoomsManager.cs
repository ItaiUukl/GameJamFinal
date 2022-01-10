using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoomsManager : Singleton<RoomsManager>
{
    private GameObject _compositeHolder;
    public GameObject Line;
    private List<PolygonCollider2D> _roomPoly = new List<PolygonCollider2D>();
    private List<Vector2> vectorarr = new List<Vector2>();

    private int index = 0;
    private GameObject _line;
    private GameObject[] lr;
    private GameObject lineobj;


    // Registers a new room to Int
    public void RegisterRoom(Room room)
    {
        if (!_compositeHolder)
        {    
            GenerateComposite();
            
        }
        room.transform.SetParent(_compositeHolder.transform);
        _roomPoly.Add(room._collider);
        GenerateLineObject(room.name);

    }

    private void GenerateComposite()
    {
        _compositeHolder = new GameObject
        {
            name = "RoomsEdgeCollider",
            layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
        };
        _compositeHolder.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        CompositeCollider2D compColl = _compositeHolder.AddComponent<CompositeCollider2D>();
        compColl.geometryType = CompositeCollider2D.GeometryType.Outlines;
        compColl.vertexDistance = .05f;
        compColl.offsetDistance = .05f;
    }
    //Input: index, create a new line renderer object and return the game object
    private void GenerateLineObject(string roomindex)
    {
        _line = new GameObject
        {
            name = "LineRenderer " + roomindex,
            layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
        };
        _line.AddComponent<LineRenderer>();
        _line.AddComponent<DrawLine>();
    }
    // Calling the funcation on Start, might need to call on Update aswell
    private void Update() {
    
        DrawPath();
    }
        
    // Input: settings up the line renderers points
    private void DrawPath()
    {
        int j = 0;
        foreach (CompositeCollider2D cc in FindObjectsOfType<CompositeCollider2D>())
        {
            for (int i = 0; i < cc.pathCount; i++)
            {
                Vector2[] points = new Vector2[cc.GetPathPointCount(i)];
                cc.GetPath(i, points);          
                foreach (Vector2 vector2 in points)
                {
                    LineInsert(vector2, j , points.Length);
                    j++;
                }
            }
        }
    }

    private void LineInsert(Vector2 v2, int index, int pointl){
        foreach (LineRenderer line in FindObjectsOfType<LineRenderer>())
        {
            Vector3[] positions = new Vector3[8];
            positions[index] = v2;
            line.SetPosition(index , positions[index]);
            line.GetComponent<LineRenderer>().positionCount = positions.Length; 
            Debug.Log("line " + line);
            Debug.Log("Point " + v2);
            Debug.Log("index " + index);
        }
    }
    
    // Resets the current level
    public void ResetLevel()
    {
        _compositeHolder = null;
        // TODO: might need more
    }
}