using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20;

    private const int MaxMass = 1000000;
    private const float CollisionThreshold = .005f;
    private const float sideSize = .2f;

    private Player _player;
    private Rigidbody2D _body;
    private PolygonCollider2D _collider, _outlineCollider;
    private SpriteRenderer _sprite;
    private Vector2 _moveDir = Vector2.zero;
    private Dictionary<MoveDirection, List<Lever>> _levers = new Dictionary<MoveDirection, List<Lever>>();
    private Dictionary<MoveDirection, bool> _blockedSides = new Dictionary<MoveDirection, bool>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer);
        _collider = GetComponent<PolygonCollider2D>();
        _collider.usedByComposite = true;
        RoomsManager.Instance.RegisterRoom(this);

        _sprite = GetComponent<SpriteRenderer>();

        foreach (MoveDirection dir in Enum.GetValues(typeof(MoveDirection)))
        {
            _blockedSides[dir] = false;
            new GameObject().AddComponent<SideDetector>().InitSide(transform, dir, _sprite.size, sideSize, 
                SideTriggerEnter, SideTriggerExit);
        }

        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)_moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    // Moves room until collision
    public void Move(MoveDirection dir)
    {
        if (_blockedSides[dir]) return;
        _moveDir = GameManager.GetDirection(dir);
        _player.RoomMoving(this);
    }

    public void AddLever(Lever lever)
    {
        if (!_levers.ContainsKey(lever.direction))
        {
            _levers[lever.direction] = new List<Lever>();
        }
        
        _levers[lever.direction].Add(lever);
    }

    private void SideTriggerEnter(MoveDirection side)
    {
        // if (_moveDir.magnitude < .1f) return;
        SetBlocked(side, true);
        if (_moveDir != GameManager.GetDirection(side)) return;
        _player.RoomStopping(this);
        _moveDir = Vector2.zero;
    }
    
    private void SideTriggerExit(MoveDirection side)
    {
        SetBlocked(side, false);
    }

    private void SetBlocked(MoveDirection side, bool state)
    {
        _blockedSides[side] = state;
        
        if (!_levers.ContainsKey(side)) return;
        
        foreach (Lever l in _levers[side])
        {
            l.SetActivation(!state);
        }
    }
}