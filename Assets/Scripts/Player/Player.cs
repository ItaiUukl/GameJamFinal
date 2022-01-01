using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float airBuffer = .1f;
    [SerializeField] private float coyoteTime = .1f;
    [SerializeField] private float maxJumpPeak = 1.2f;
    [SerializeField] private float minJumpPeak = .4f;
    [SerializeField, Min(.0001f)] private float maxPeakDistance = 1.2f;
    [SerializeField, Min(.0001f)] private float minPeakDistance = .4f;
    [SerializeField, Min(.0001f)] private float fallDistance = .9f;
    
    public bool IsGrounded { set; get;}
    
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private SpriteRenderer _sprite;
    
    private Room _currRoom;
    private Vector2 _velocity = Vector2.zero;
    
    private float _jumpForce;
    private float _gravity;
    
    private float _coyote;
    private float _jumpBuffer;
    
    private float _height; 
    private float _distance;


    // Use this for initialization
    void Start()
    {
        _height = maxJumpPeak;
        _distance = maxPeakDistance;
        UpdateForces();
        
        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
        _rb.sharedMaterial = new PhysicsMaterial2D {friction = 0};
        
        _collider = GetComponent<Collider2D>();

        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currRoom)
        {
            return;
        }

        float xInput = Input.GetAxis("Horizontal");
        _velocity.x = xInput * speed;
        _sprite.flipX = xInput < 0;
        
        if (IsGrounded)
        {
            _distance = maxPeakDistance;
            _height = maxJumpPeak;
            UpdateForces();
            
            if (_jumpBuffer != 0)
            {
                _velocity.y = _jumpForce;
            }
            else
            {
                _coyote = _velocity.y < _jumpForce ? coyoteTime : _coyote;
                
                if (IsJumpPressed())
                {
                    _velocity.y = _jumpForce;
                    _coyote = 0;
                }
            }
        }
        else
        {
            _velocity.y = _rb.velocity.y;
            if (_rb.velocity.y <= 0)
            {
                _distance = fallDistance;
                _height = maxJumpPeak;
                UpdateForces();
            }
            else if (IsJumpReleased())
            {
                _height = minJumpPeak;
                _distance = minPeakDistance;
                UpdateForces();
                _velocity.y = Mathf.Min(_jumpForce, _velocity.y);
            }
            
            _velocity.y -= _gravity * Time.deltaTime;
             
            if (IsJumpPressed())
            {
                if (_coyote > 0)
                {
                    _velocity.y = _jumpForce;
                    _coyote = 0;
                }
                else
                {
                    _jumpBuffer = airBuffer;
                }
            }
            
            _coyote = Mathf.Max(_coyote - Time.deltaTime, 0);
        }
        _jumpBuffer = Mathf.Max(_jumpBuffer - Time.deltaTime, 0);
        _rb.velocity = _velocity;
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

    private void UpdateForces()
    {
        _jumpForce = (2 * _height * speed) / _distance;
        _gravity = (2 * _height * speed * speed) / (_distance * _distance);
    }

    private static bool IsJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
    }
    
    private static bool IsJumpReleased()
    {
        return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space);
    }
}