using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] private float acceleration = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravity = 6;
    [SerializeField] private float airBuffer = .5f;
    [SerializeField] private float coyoteTime = .5f;
    
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private SpriteRenderer _sprite;
    
    private Room _currRoom = null;
    private Vector2 _velocity = Vector2.zero;
    private float _coyote;
    private float _jumpBuffer;
    private bool _isGrounded;
    
    private readonly Vector2 _groundDetectionOffset = new Vector2(0.1f, 0.1f);

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
        
        _collider = GetComponent<Collider2D>();

        _sprite = GetComponent<SpriteRenderer>();
        // _rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currRoom)
        {
            return;
        }

        float xInput = Input.GetAxis("Horizontal");
        _velocity.x += xInput * acceleration * Time.deltaTime;
        _velocity.x = Mathf.Clamp(_velocity.x, -maxSpeed, maxSpeed);
        _sprite.flipX = xInput < 0;
        
        if (_isGrounded)
        {
            if (_jumpBuffer != 0)
            {
                _velocity.y = jumpForce;
            }
            else
            {
                _coyote = coyoteTime;

                _velocity.x = Mathf.Lerp(_velocity.x, xInput, Time.deltaTime * 15);
                
                if (IsJumpPressed())
                {
                    _velocity.y = jumpForce;
                    _coyote = 0;
                }
            }
        }
        else
        {
            _velocity.y -= gravity * Time.deltaTime;
            
            Debug.Log(_coyote);
            if (IsJumpPressed())
            {
                if (_coyote > 0)
                {
                    _velocity.y = jumpForce;
                    _coyote = 0;
                }
                else
                {
                    _jumpBuffer = airBuffer;
                }
            }

            if (xInput == 0)
            {
                _coyote = Mathf.Max(_coyote - Time.deltaTime, 0);
            }

            if (IsJumpReleased())
            {
                _velocity.y = jumpForce / 2;
            }
            

            _velocity.x = Mathf.Lerp(_velocity.x, xInput, Time.deltaTime * 2);
            
        }
        _jumpBuffer = Mathf.Max(_jumpBuffer - Time.deltaTime, 0);

    }

    void FixedUpdate()
    {
        Debug.Log(_velocity);
        _rb.MovePosition(_rb.position + _velocity * Time.deltaTime);
        _isGrounded = IsGrounded();
        Debug.Log(_isGrounded);
    }

    public void RoomMoving(Room room)
    {
        if (Physics2D.OverlapCircleAll(transform.position, 1f).Contains(room.GetComponent<Collider2D>()))
        {
            _currRoom = room;
            _rb.isKinematic = true;
            _collider.enabled = false;
            transform.SetParent(room.transform);
        }
    }

    public void RoomStopping(Room room)
    {
        if (room == _currRoom)
        {
            transform.SetParent(null);
            _rb.isKinematic = false;
            _collider.enabled = true;
            _currRoom = null;
        }
    }

    private static bool IsJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
    }
    
    private static bool IsJumpReleased()
    {
        return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space);
    }
    
    private Vector2 GetBottomLeft()
    {
        Bounds colliderBounds = _collider.bounds;
        return new Vector2(colliderBounds.center.x - colliderBounds.extents.x, 
            colliderBounds.center.y - colliderBounds.extents.y);
    }
    
    private Vector2 GetBottomRight()
    {
        Bounds colliderBounds = _collider.bounds;
        return new Vector2(colliderBounds.center.x + colliderBounds.extents.x, 
            colliderBounds.center.y - colliderBounds.extents.y);
    }
    
    private Vector2 GetBottom()
    {
        Bounds colliderBounds = _collider.bounds;
        return new Vector2(colliderBounds.center.x, 
            colliderBounds.center.y - colliderBounds.extents.y - 1);
    }

    private bool IsGrounded()
    {
        Vector2 areaTL = GetBottomLeft() + _groundDetectionOffset;
        Vector2 areaBR = GetBottomRight() - _groundDetectionOffset;
        return Physics2D.BoxCast(GetBottom(), _collider.bounds.size, 0f, Vector2.down, .1f);
    }
}