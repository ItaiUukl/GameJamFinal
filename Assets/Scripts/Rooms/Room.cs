using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float maxSpeed = 50;
    [SerializeField] private float acceleration = 5;

    private const float SideSize = .2f;

    private Player _player;
    private PolygonCollider2D _collider;
    private SpriteRenderer _sprite;
    private Vector2 _moveDir = Vector2.zero;
    // private float _velocity = 0;
    private readonly Dictionary<MoveDirection, List<Lever>> _levers = new Dictionary<MoveDirection, List<Lever>>();
    private readonly Dictionary<MoveDirection, bool> _blockedSides = new Dictionary<MoveDirection, bool>();

    private bool IsMoving => _moveDir.magnitude != 0;

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
            new GameObject().AddComponent<SideDetector>().InitSide(transform, dir, _sprite.size, SideSize,
                SideTriggerEnter, SideTriggerExit);
        }

        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        // _velocity = Mathf.Min(Time.fixedDeltaTime * acceleration + _velocity, maxSpeed) * _moveDir.magnitude;
        // transform.position += (Vector3) _moveDir * _velocity * Time.fixedDeltaTime;
        transform.position += (Vector3) _moveDir * moveSpeed * Time.fixedDeltaTime;
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

    private void SideTriggerEnter(MoveDirection side, Collider2D other)
    {
        // if (_moveDir.magnitude < .1f) return;
        // _velocity = 0;
        
        SetBlocked(side, true);
        if (_moveDir != GameManager.GetDirection(side)) return;
        FixPosition(side, other);
        _player.RoomStopping(this);
        _moveDir = Vector2.zero;
    }

    private void SideTriggerExit(MoveDirection side)
    {
        if (!IsMoving) return;
        SetBlocked(side, false);
    }

    private void FixPosition(MoveDirection side, Collider2D other)
    {
        if (!IsMoving) return;
        Vector2 offset = _sprite.size / 2 - Vector2.one * .08f;
        switch (side)
        {
            case MoveDirection.Left:
                transform.position = new Vector2(other.bounds.max.x + offset.x, transform.position.y);
                return;
            case MoveDirection.Right:
                transform.position = new Vector2(other.bounds.min.x - offset.x, transform.position.y);
                return;
            case MoveDirection.Down:
                transform.position = new Vector2(transform.position.x, other.bounds.max.y + offset.y);
                return;
            case MoveDirection.Up:
                transform.position = new Vector2(transform.position.x, other.bounds.min.y - offset.y);
                return;
            default: return;
        }
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