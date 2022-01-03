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
    private const float sideSize = .5f;

    private Player _player;
    private Rigidbody2D _body;
    private PolygonCollider2D _collider, _outlineCollider;
    private SpriteRenderer _sprite;
    private Vector2 _moveDir = Vector2.zero;
    private Dictionary<MoveDirection, List<Lever>> _sideColliders = new Dictionary<MoveDirection, List<Lever>>();
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
            
        // _body = gameObject.AddComponent<Rigidbody2D>();
        // _body.bodyType = RigidbodyType2D.Dynamic;
        // _body.isKinematic = true;
        // _body.gravityScale = 0;
        // _body.mass = _body.drag = MaxMass;
        // _body.constraints = RigidbodyConstraints2D.FreezeRotation;

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

    private void SideTriggerEnter(MoveDirection side)
    {
        if (_moveDir.magnitude < .1f) return;
        _player.RoomStopping(this);
        _moveDir = Vector2.zero;
        _blockedSides[side] = true;
    }
    
    private void SideTriggerExit(MoveDirection side)
    {
        _blockedSides[side] = false;
    }

    private IEnumerator Collide()
    {
        yield return new WaitForSeconds(0.05f);
        // if (!(_body.velocity.magnitude < CollisionThreshold)) yield break;
        // _body.mass = _body.drag = MaxMass;
        // _body.isKinematic = true;
        _moveDir = Vector2.zero;
        // _body.velocity = Vector2.zero;
        // _outlineCollider.points = _collider.points.Select(t => (Vector2) _collider.transform.TransformPoint(t))
        //     .ToArray();
        _player.RoomStopping(this);
    }
}