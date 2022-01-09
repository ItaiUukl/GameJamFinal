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
    private GameObject line;
    private LineRenderer lr;
    private Transform[] transforms;



    // Registers a new room to Int
    public void RegisterRoom(Room room)
    {
        Debug.Log(room);
        if ((!_compositeHolder) && (!line))
        {    
            GenerateComposite();
        }
        room.transform.SetParent(_compositeHolder.transform);
        _roomPoly.Add(room._collider);

        if(!line){
            GenerateLineObject();
        }
    }

    void ConvertPointToWorldSpace()
    {
//        transforms = _roomCols[1].points.Select(t => (Vector2)_roomCols.transform.TransformPoint(t)).ToArray();

    }
        private void GenerateLineObject()
    {
        line = new GameObject
        {
            name = "LineRenderer",
            layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
        };
        lr = line.AddComponent<LineRenderer>();
        line.AddComponent<Lines>();
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
    private void Update() {
        MatchPath();
    }
    private void Awake(){
        
    }
    // converting vector2 to transform positions for line renderer points
    public void SetUpLine(){
        // From Itai line:
        //transforms = _roomCols[0].points.Select(t => (Vector2)_roomCols[0].transform.TransformPoint(t)).ToArray();
        for(int i = 0; i < transforms.Length; i++){
            lr.SetPosition(i,transforms[i].position);
        }
        lr.positionCount = transforms.Length;
    }
    //input: vector2 array and settings up the line renderer object
    private void MatchPath()
    {
        foreach(PolygonCollider2D p in _roomPoly){
            for(int p_in=0; p_in < p.points.Length; p_in++){
                vectorarr.Add(p.points[p_in]);
                Debug.Log("Polygon foreach" + p.points[p_in]);

            }
            foreach (CompositeCollider2D cp in FindObjectsOfType<CompositeCollider2D>())
            {
                for (int i = 0; i < cp.pathCount; i++)
                {
                    Vector2[] points = new Vector2[cp.GetPathPointCount(i)];
                    cp.GetPath(i, points);
                    bool colliderHasMatchingPaths = true;
                    vectorarr.Add(p.points[i]);

                    Debug.Log("CompositeCollider path for loop: " + points[i]);

                    foreach (Vector2 vector2 in points)
                    {
                        Debug.Log("CompositeCollider points: " + vector2);
                        if (!vectorarr.Contains(vector2))
                        {           
                            colliderHasMatchingPaths = false;
                        }
                    }
                    if (colliderHasMatchingPaths)
                    {
                            Debug.Log(colliderHasMatchingPaths);
                        Vector3 v3 = p.transform.TransformPoint(vectorarr[i]);
                        transforms[i].position = v3;
                        Debug.Log(i);
                        SetUpLine();
                    }
                }
            }
        }
    }
    
    // Resets the current level
    public void ResetLevel()
    {
        _compositeHolder = null;
        // TODO: might need more
    }
}