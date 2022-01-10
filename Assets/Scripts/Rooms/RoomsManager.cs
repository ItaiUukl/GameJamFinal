using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    private GameObject _compositeHolder;

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

        CompositeCollider2D compColl = _compositeHolder.AddComponent<CompositeCollider2D>();
        compColl.geometryType = CompositeCollider2D.GeometryType.Outlines;
        compColl.vertexDistance = .05f;
        compColl.offsetDistance = .05f;
    }

    // Resets the current level
    public void ResetLevel()
    {
        _compositeHolder = null;
        // TODO: might need more
    }
}