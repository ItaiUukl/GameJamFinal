using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoomsManager : Singleton<RoomsManager>
{
    struct RoomsGroup
    {
        public GameObject GroupParent { get; set; }
        public HashSet<Room> Rooms { get; set; }
    }
    
    struct Line
    {
        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; }

        public float Len() => Vector2.Distance(PointA, PointB);
    }
    
    [SerializeField] private Collider2D player;
    
    private List<RoomsGroup> _roomsGroups = new List<RoomsGroup> ();

    // Registers a new room to Int
    public void RegisterRoom(Room room)
    {
        // TODO maybe: Check if room overlaps other rooms and add it to appropriate group
        int groupNum = _roomsGroups.Count;
        
        GameObject groupObject = new GameObject();
        
        _roomsGroups.Add(new RoomsGroup{GroupParent = groupObject, Rooms = new HashSet<Room> {room}});

        groupObject.layer = LayerMask.NameToLayer("Rooms");
        groupObject.name = "RoomGroup" + groupNum;

        room.RoomGroup = groupNum;
        
        CreateEdgeCollider(groupNum);
        
        room.transform.SetParent(groupObject.transform);
        // Physics2D.IgnoreCollision(room.GetComponent<PolygonCollider2D>(), player);
    }

    // Resets the current level
    public void ResetLevel()
    {
        _roomsGroups.Clear();
        // TODO: implement
    }
    
    public void RoomConnection(Room room1, Room room2)
    {
        // TODO: combine both groups
        _roomsGroups[room2.RoomGroup].Rooms.Add(room1);
        room1.RoomGroup = room2.RoomGroup;
        CreateEdgeCollider(room2.RoomGroup);
    }
    
    public void RoomDisconnection(Room room)
    {
        // TODO: implement
        RecalculateRoomsCollider();
    }

    private void CreateEdgeCollider(int group)
    {
        PolygonCollider2D tempColl;
        
        List<Vector2> edgePoints = new List<Vector2>();
        
        foreach (Room room in _roomsGroups[group].Rooms)
        {
            tempColl = room.GetComponent<PolygonCollider2D>();
            edgePoints.AddRange(tempColl.points.Select(t => (Vector2)tempColl.transform.TransformPoint(t)));
        }
        edgePoints.Add(edgePoints[0]);
        
        //edgePoints = ConvexHull.ComputeConvexHull(edgePoints);
        
        _roomsGroups[group].GroupParent.SetActive(false);
        
        _roomsGroups[group].GroupParent.AddComponent<EdgeCollider2D>().SetPoints(edgePoints);
        
        _roomsGroups[group].GroupParent.SetActive(true);
    }
    
    private void RecalculateRoomsCollider()
    {
        // TODO: implement
    }

    private List<Line> PointsToLines(List<Vector2> points)
    {
        List<Line> lines = new List<Line>(points.Count - 1);
        
        for (int i = 0; i < points.Count - 1; i++)
        {
            lines.Add(new Line{PointA = points[i], PointB = points[i+1]});
        }

        return lines;
    }
    
    // TODO: make normal
    private List<Vector2> CombineEdges(List<Vector2> edges1, List<Vector2> edges2)
    {
        List<Line> lines1 = PointsToLines(edges1), lines2 = PointsToLines(edges2);
        List<Vector2> newEdges = new List<Vector2>();
        List<int> intersections = new List<int>(lines1.Count);

        for (int i = 0; i < lines1.Count; i++)
        {
            intersections.Add(-1);
            
            for (int j = 0; j < lines2.Count; j++)
            {
                if (LinesIntersect(lines1[i], lines2[j]))
                {
                    intersections[i] = j;
                    break;
                }
            }
        }

        bool completed2 = false;
        
        for (int i = 0; i < lines1.Count; i++)
        {
            if (completed2 || intersections[i] == -1)
            {
                newEdges.Add(lines1[i].PointA);
                break;
            }

            Line interLine = lines2[intersections[i]];

            int startPoint = Vector2.Distance(lines1[i].PointB, interLine.PointA) > 
                             Vector2.Distance(lines1[i].PointB, interLine.PointB) ? 
                             intersections[i] : intersections[i] + 1;
            newEdges.Add(edges2[startPoint]);

            int index, inter = -1;
            
            for (int j = 0; j < lines2.Count; j++)
            {
                index = (startPoint + j) % lines2.Count;
                for (int k = 0; k < intersections.Count; k++)
                {
                    if (index == intersections[k])
                    {
                        inter = k;
                        break;
                    }
                }
                if (inter != -1)
                {
                    Line interLine2 = lines1[inter];
                    i = Vector2.Distance(lines2[j].PointB, interLine2.PointA) > 
                        Vector2.Distance(lines2[j].PointB, interLine2.PointB) ? 
                        inter : inter + 1;
                    break;
                }

                newEdges.Add(lines2[index].PointA);
            }
            
            completed2 = true;

        }

        newEdges.Add(newEdges[0]);
        return newEdges;
    }

    private bool LinesIntersect(Line line1, Line line2)
    {
        // TODO: make more efficient
        Line shorter, longer;

        if (line1.Len() < line2.Len())
        {
            shorter = line1;
            longer = line2;
        }
        else
        {
            shorter = line2;
            longer = line1;
        }

        return IsPointOnLine(shorter.PointA, longer) || IsPointOnLine(shorter.PointB, longer);
    }

    private bool IsPointOnLine(Vector2 point, Line line)
    {
        float pointDistance = Vector2.Distance(point, line.PointA) + Vector2.Distance(point, line.PointB);
        return pointDistance - Vector2.Distance(line.PointA, line.PointB) <= .01f;
    }
}