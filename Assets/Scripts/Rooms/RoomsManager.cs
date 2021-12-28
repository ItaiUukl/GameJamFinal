using System.Linq;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    private GameObject _compositeHolder;

    // Registers a new room to Int
    public PolygonCollider2D RegisterRoom(Room room)
    {
        if (!_compositeHolder)
        {
            GenerateComposite();
        }

        PolygonCollider2D polyColl = room.gameObject.AddComponent<PolygonCollider2D>(),
            tempColl = room.GetComponent<PolygonCollider2D>();
        polyColl.points = tempColl.points;
        // polyColl.points = tempColl.points
        //     .Select(t => (Vector2) tempColl.transform.TransformPoint(t)).ToArray();
        polyColl.usedByComposite = true;
        room.transform.SetParent(_compositeHolder.transform);
        // _compositeHolder.GetComponent<CompositeCollider2D>().GenerateGeometry();
        return polyColl;
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
        compColl.offsetDistance = .01f;
    }

    // Resets the current level
    public void ResetLevel()
    {
        _compositeHolder = null;
        // TODO: might need more
    }
}