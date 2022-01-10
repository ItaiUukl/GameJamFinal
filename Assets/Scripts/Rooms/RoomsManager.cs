using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoomsManager : Singleton<RoomsManager>
{
    private GameObject _compositeHolder;
    public List<GameObject> LinesList = new List<GameObject>();
    // list for every room collider
    private List<PolygonCollider2D> _roomPoly = new List<PolygonCollider2D>();
    // list of vector2 pollider points 
    private List<Vector2[]> collPoints = new List<Vector2[]>();

    private CompositeCollider2D compColl;
    //room index for ref
    private int r = 0;
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

    }

    private void GenerateComposite()
    {
        _compositeHolder = new GameObject
        {
            name = "RoomsEdgeCollider",
            layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
        };
        _compositeHolder.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        compColl = _compositeHolder.AddComponent<CompositeCollider2D>();
        compColl.geometryType = CompositeCollider2D.GeometryType.Outlines;
        compColl.vertexDistance = .05f;
        compColl.offsetDistance = .05f;

    }

    // Calling the funcation on Update, might need to find a better place for that, maybe on collision with other room
    private void Update() {
            Calculatelines();
       
    }
        
    // Input: settings up the line renderers points
    private void Calculatelines()
    {
            for (int i = 0; i < compColl.pathCount; i++)
            {
                Vector2[] points = new Vector2[compColl.GetPathPointCount(i)];
                compColl.GetPath(i, points);
                // fill the list with transform converted points
                Vector3[] points3 = points.Select(p => (Vector3) compColl.transform.TransformPoint(p)).ToArray();
                LineInsert(points3, i);
            }
            if(compColl.pathCount < LinesList.Count()){
                for(int i = compColl.pathCount; i < LinesList.Count(); i++)
                    LineInsert(new Vector3[]{}, i);
            }
        
    }
    //points = collPoints.Select(t => (Vector2)polyColl.transform.TransformPoint(t)).ToArray();
    //Input: point, index in the composite, room index create a new line renderer object and return the game object
    private void LineInsert(Vector3[] points, int idx){
        //create new line object
        LineRenderer l;
        if(idx >= LinesList.Count()){
            LinesList.Add(new GameObject
            {
                name = "LineRenderer " + idx,
                layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
            });
            l = LinesList[idx].AddComponent<LineRenderer>();
            l.loop = true;
            l.startWidth = 0.2f;
            l.textureMode = LineTextureMode.Tile;
            l.sortingLayerName  = "Room";
            l.material = Resources.Load("Materials/LineRenderRoom", typeof(Material)) as Material;

        } 
        else
        {
            l = LinesList[idx].GetComponent<LineRenderer>();
        }
            l.positionCount = points.Count();
            l.SetPositions(points);
 
            

    
    }
    
    // Resets the current level
    public void ResetLevel()
    {
        _compositeHolder = null;
        LinesList = new List<GameObject>();
        compColl = null;
        
        // TODO: might need more
    }
}