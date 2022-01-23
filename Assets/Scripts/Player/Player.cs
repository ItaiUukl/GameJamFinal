using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool IsGrounded { set; get; }

    public bool IsActive { set; get; }

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _animator;

    private Vector2 _velocity = Vector2.zero;

    private float _xInput;
    private float _jumpForce;
    private float _gravity;

    private float _coyote;
    private float _jumpBuffer;

    private float _height;
    private float _distance;
    
    private bool _isJumpPressed;
    private bool _isJumpReleased;

    private static readonly int AnimatorVelocityY = Animator.StringToHash("VelocityY");
    private static readonly int AnimatorRun = Animator.StringToHash("Run");
    private static readonly int AnimatorJump = Animator.StringToHash("Jump");
    private static readonly int AnimatorGrounded = Animator.StringToHash("Grounded");


    // Use this for initialization
    void Start()
    {
        GameManager.Instance.DoNothing(); // TODO: delete
        _height = maxJumpPeak;
        _distance = maxPeakDistance;
        UpdateForces();

        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
        _rb.sharedMaterial = new PhysicsMaterial2D {friction = 0};

        _sprite = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity.x = _xInput * speed;
        if (_xInput != 0) _sprite.flipX = _xInput < 0;

        if (IsGrounded)
        {
            _animator.SetBool(AnimatorGrounded, true);
            _distance = maxPeakDistance;
            _height = maxJumpPeak;
            UpdateForces();

            if (_jumpBuffer > 0)
            {
                _velocity.y = _jumpForce;
            }
            else
            {
                _coyote = _velocity.y < _jumpForce ? coyoteTime : _coyote;

                if (_isJumpPressed)
                {
                    _isJumpPressed = false;
                    _animator.SetTrigger(AnimatorJump);
                    _velocity.y = _jumpForce;
                    _coyote = 0;
                }
            }
        }
        else
        {
            _animator.SetBool(AnimatorGrounded, false);
            _velocity.y = _rb.velocity.y;
            if (_rb.velocity.y <= 0)
            {
                _distance = fallDistance;
                _height = maxJumpPeak;
                UpdateForces();
            }
            else if (_isJumpReleased)
            {
                _isJumpReleased = false;
                _height = minJumpPeak;
                _distance = minPeakDistance;
                UpdateForces();
                _velocity.y = Mathf.Min(_jumpForce, _velocity.y);
            }

            _velocity.y -= _gravity * Time.deltaTime;

            if (_isJumpPressed)
            {
                _isJumpPressed = false;
                if (_coyote > 0)
                {
                    _animator.SetTrigger(AnimatorJump);
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

        _animator.SetFloat(AnimatorVelocityY, _rb.velocity.y);
        _animator.SetBool(AnimatorRun, Mathf.Abs(_rb.velocity.x) > 0.05);
    }
    public void Activate()
    {
        IsActive = true;
    }

    private void UpdateForces()
    {
        _jumpForce = (2 * _height * speed) / _distance;
        _gravity = (2 * _height * speed * speed) / (_distance * _distance);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isJumpPressed = true;
            _isJumpReleased = false;
        }
        else if (ctx.canceled)
        {
            _isJumpReleased = true;
            _isJumpPressed = false;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _xInput = IsActive ? ctx.ReadValue<float>() : 0;
    }
}