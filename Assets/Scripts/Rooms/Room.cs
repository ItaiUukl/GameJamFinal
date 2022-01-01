using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20;

    private const int MaxMass = 1000000;
    private const float CollisionThreshold = .005f;

    private Player _player;
    public Rigidbody2D _body;
    private PolygonCollider2D _collider, _outlineCollider;
    private Vector2 _moveDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer);
        _collider = GetComponent<PolygonCollider2D>();
        _outlineCollider = RoomsManager.Instance.RegisterRoom(this);

        _body = gameObject.AddComponent<Rigidbody2D>();
        _body.bodyType = RigidbodyType2D.Dynamic;
        _body.isKinematic = true;
        _body.gravityScale = 0;
        _body.mass = _body.drag = MaxMass;
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;

        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (!(_body.velocity.magnitude < CollisionThreshold))
        {
            _outlineCollider.points = _collider.points
                .Select(t => (Vector2) _collider.transform.TransformPoint(t)).ToArray();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision: " + gameObject.name + " with "+ other.gameObject.name);
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        if (layer == GlobalsSO.RoomsLayer || layer == GlobalsSO.BorderLayer)
        {
            if (_moveDir.magnitude == 0) return;
            StartCoroutine(Collide());
        }
    }

    // Moves room until collision
    public void Move(Vector2 dir)
    {
        _moveDir = dir;
        _body.isKinematic = false;
        _body.velocity = moveSpeed * dir;
        _player.RoomMoving(this);
        _body.mass = 1;
        _body.drag = 0;
    }

    private IEnumerator Collide()
    {
        yield return new WaitForSeconds(0.05f);
        if (!(_body.velocity.magnitude < CollisionThreshold)) yield break;
        _body.mass = _body.drag = MaxMass;
        _body.isKinematic = true;
        _moveDir = Vector2.zero;
        // _body.velocity = Vector2.zero;
        _outlineCollider.points = _collider.points.Select(t => (Vector2) _collider.transform.TransformPoint(t))
            .ToArray();
        _player.RoomStopping(this);
    }
}