using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    [FormerlySerializedAs("moveSpeed")] [SerializeField]
    private float maxSpeed = 20;

    [SerializeField] private float acceleration = 5;

    private const float SideSize = .2f;

    private Player _player;

    private PolygonCollider2D _collider;

    private SpriteRenderer _sprite;
    private Vector2 _moveDir = Vector2.zero;
    private float _velocity = 0;
    private readonly Dictionary<MoveDirection, List<Lever>> _levers = new Dictionary<MoveDirection, List<Lever>>();
    private readonly Dictionary<MoveDirection, bool> _blockedSides = new Dictionary<MoveDirection, bool>();

    public bool IsMoving => _moveDir.magnitude != 0;
    private bool IsSlow => maxSpeed < GameManager.Globals.maxSlowRoomSpeed;
    private bool _isShakingScreen;

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

    private void Update()
    {
        if (!IsSlow) return;
        if (IsMoving)
        {
            AudioManager.Instance.Play("Slow Room");
            _player._rumbler.RumbleLinear(0.01f, 0.3f, 0.01f, 0.5f, 0.2f);
            if (!_isShakingScreen)
            {
                GameManager.Cam.SetSlowShake(true);
                _isShakingScreen = true;
            }
        }
        else
        {
            _player._rumbler.StopRumble();
            if (_isShakingScreen)
            {
                GameManager.Cam.SetSlowShake(false);
                _isShakingScreen = false;
            }
        }
    }

    private void FixedUpdate()
    {
        _velocity = Mathf.Min(Time.fixedDeltaTime * acceleration + _velocity, maxSpeed) * _moveDir.magnitude;
        transform.position += (Vector3) _moveDir * _velocity * Time.fixedDeltaTime;
    }

    // Moves room until collision
    public void Move(MoveDirection dir)
    {
        if (_blockedSides[dir]) return;
        _moveDir = MoveDirectionUtils.ToVector2(dir);
        AudioManager.Instance.Play("Room Move");
        // if (_collider.OverlapPoint(_player.transform.position))
        // {
        _player.transform.SetParent(transform);
        // }
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
        SetBlocked(side, true);
        if (_moveDir != MoveDirectionUtils.ToVector2(side)) return;
        GameManager.Cam.ShakeCamera();
        _player._rumbler.RumbleConstant(0.1f, 0.3f, 0.1f);
        _velocity = 0;
        FixPosition(side, other);
        AudioManager.Instance.Play("Room Hit");
        _moveDir = Vector2.zero;
    }

    private void SideTriggerExit(MoveDirection side)
    {
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

    public void SetBlocked(MoveDirection side, bool state)
    {
        _blockedSides[side] = state;

        foreach (MoveDirection key in _levers.Keys)
        {
            foreach (Lever l in _levers[key])
            {
                if (key == side)
                {
                    l.SetActivation(!state);
                }

                if (IsMoving)
                {
                    l.Moving(!state);
                }
            }
        }
    }
}