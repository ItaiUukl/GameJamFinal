using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    private GameObject _compositeHolder;
    private CompositeCollider2D _compColl;
    private HashSet<Room> _rooms = new HashSet<Room>();

    private List<GameObject> _linesList = new List<GameObject>();
    private List<Coroutine> _materialsRoutines = new List<Coroutine>();

    // Calling the funcation on Update, might need to find a better place for that, maybe on collision with other room
    private void Update()
    {
        CalculateLines();
    }

    // Registers a new room to Int
    public void RegisterRoom(Room room)
    {
        if (!_compositeHolder)
        {
            GenerateComposite();
            _compColl.transform.SetParent(room.transform.parent);
        }

        _rooms.Add(room);
        room.transform.SetParent(_compositeHolder.transform);
    }

    private IEnumerator ChangeMaterial(LineRenderer line)
    {
        int idx = 0;

        while (true)
        {
            line.material = GameManager.Globals.outlineMaterials[idx];
            idx = (idx + 1) % GameManager.Globals.outlineMaterials.Count;
            yield return new WaitForSeconds(GameManager.Globals.outlineAnimationSpeed);
        }
    }

    private void GenerateComposite()
    {
        _compositeHolder = new GameObject
        {
            name = "RoomsEdgeCollider",
            layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
        };
        _compositeHolder.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        _compColl = _compositeHolder.AddComponent<CompositeCollider2D>();
        _compColl.geometryType = CompositeCollider2D.GeometryType.Outlines;
        _compColl.vertexDistance = .005f;
        _compColl.offsetDistance = .08f;
    }

    // Input: settings up the line renderers points
    private void CalculateLines()
    {
        if (!_compColl) return;
        for (int i = 0; i < _compColl.pathCount; i++)
        {
            Vector2[] points = new Vector2[_compColl.GetPathPointCount(i)];
            _compColl.GetPath(i, points);
            // fill the list with transform converted points
            Vector3[] points3 = points.Select(p => (Vector3) _compColl.transform.TransformPoint(p)).ToArray();
            LineInsert(points3, i);
        }

        if (_compColl.pathCount < _linesList.Count)
        {
            for (int i = _compColl.pathCount; i < _linesList.Count; i++)
                LineInsert(new Vector3[] { }, i);
        }
    }

    //Input: point, index in the composite, room index create a new line renderer object and return the game object
    private void LineInsert(Vector3[] points, int idx)
    {
        //create new line object
        LineRenderer l;
        if (idx >= _linesList.Count)
        {
            _linesList.Add(new GameObject
            {
                name = "LineRenderer " + idx,
                layer = LayerMask.NameToLayer(GlobalsSO.OutlinesLayer)
            });
            l = _linesList[idx].AddComponent<LineRenderer>();
            l.loop = true;
            l.startWidth = GameManager.Globals.outlineWidth;
            l.textureMode = GameManager.Globals.outlineTextureMode;
            l.sortingOrder = GameManager.Globals.outlineSortingOrder;
            /* if you would want different line color in the menu
            if(GameManager._menu)
            {
                l.endColor = GameManager.Globals.MenuColor;
                l.startColor = GameManager.Globals.MenuColor;
            } */
            //new Color(60, 104, 140);
            l.sortingLayerName = "Room";
            _materialsRoutines.Add(StartCoroutine(ChangeMaterial(l)));
        }
        else
        {
            l = _linesList[idx].GetComponent<LineRenderer>();
        }

        l.positionCount = points.Count();
        l.SetPositions(points);
    }

    // Resets the current level
    public void ResetLevel()
    {
        _linesList.Clear();
        _rooms.Clear();
        _compositeHolder = null;
        _compColl = null;
        foreach (Coroutine routine in _materialsRoutines)
        {
            StopCoroutine(routine);
        }

        _materialsRoutines.Clear();
    }
}