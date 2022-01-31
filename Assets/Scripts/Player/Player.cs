using System;
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

    [NonSerialized] public bool IsActive;
    [NonSerialized] public bool IsPaused = true;
    [NonSerialized] public bool InTransition;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private GroundDetector _groundDetector;

    private Vector3 _initPos;
    private Vector2 _velocity = Vector2.zero;
    private float _xInput, _jumpForce, _gravity;

    private float _coyote, _jumpBuffer;

    private float _height, _distance;

    private bool _isJumpPressed, _isJumpReleased, _animateJump;

    private Action _onReachedDestCallback = null;
    private float? _destX = null;
    private bool IsWalkingToDoor => _destX != null;

    [NonSerialized] public Transform RoomToEnter;

    private static readonly int AnimatorVelocityY = Animator.StringToHash("VelocityY"),
        AnimatorRun = Animator.StringToHash("Run"),
        AnimatorJump = Animator.StringToHash("Jump"),
        AnimatorGrounded = Animator.StringToHash("Grounded");

    // Use this for initialization
    void Start()
    {
        UpdateForces(maxJumpPeak, maxPeakDistance);

        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
        _rb.sharedMaterial = new PhysicsMaterial2D {friction = 0};

        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = false;

        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        _groundDetector = GetComponentInChildren<GroundDetector>();
    }

    private void Update()
    {
        if (IsPaused) return;
        if (IsWalkingToDoor && (transform.position.x * _xInput > _destX * _xInput || _rb.velocity.x == 0))
        {
            _onReachedDestCallback.Invoke();
            _rb.velocity = Vector2.zero;
            _xInput = 0;
            _destX = null;
            IsPaused = true;
            return;
        }
        
        if (InTransition)
        {
            return;
        }

        if (_xInput != 0 && _sprite.flipX != _xInput < 0)
        {
            _groundDetector.InstantiateDust();
            _sprite.flipX = _xInput < 0;
        }

        if (_groundDetector.IsGrounded())
        {
            if (_jumpBuffer <= 0)
            {
                if (_velocity.x != 0)
                {
                    AudioManager.Instance.Play("Run");
                }

                _coyote = _velocity.y < _jumpForce ? coyoteTime : _coyote;
            }
        }
        else
        {
            _coyote = Mathf.Max(_coyote - Time.deltaTime, 0);
        }

        _jumpBuffer = Mathf.Max(_jumpBuffer - Time.deltaTime, 0);
        
        _animator.SetBool(AnimatorGrounded, _groundDetector.IsGrounded());
        if (_animateJump)
        {
            _animator.SetTrigger(AnimatorJump);
            _animateJump = false;
        }
        _animator.SetFloat(AnimatorVelocityY, _rb.velocity.y);
        _animator.SetBool(AnimatorRun, Mathf.Abs(_rb.velocity.x) > 0.05);
    }

    private void FixedUpdate()
    {
        if (IsPaused) return;

        if (InTransition && !IsWalkingToDoor)
        {
            TransitionUpdate();
            return;
        }
        _velocity.x = _xInput * speed;
        if (_groundDetector.IsGrounded())
        {
            UpdateForces(maxJumpPeak, maxPeakDistance);

            if (_jumpBuffer > 0)
            {
                _velocity.y = _jumpForce;
            }
            else
            {
                if (_isJumpPressed)
                {
                    _isJumpPressed = false;
                    _animateJump = true;
                    _velocity.y = _jumpForce;
                    _coyote = 0;
                }
            }
        }
        else
        {
            _velocity.y = _rb.velocity.y;
            
            if (_velocity.y <= 0)
            {
                UpdateForces(maxJumpPeak, fallDistance);
            }
            else if (_isJumpReleased)
            {
                _isJumpReleased = false;
                UpdateForces(minJumpPeak, minPeakDistance);
                _velocity.y = Mathf.Min(_jumpForce, _velocity.y);
            }

            _velocity.y -= _gravity * Time.fixedDeltaTime;

            if (_isJumpPressed)
            {
                _isJumpPressed = false;
                if (_coyote > 0)
                {
                    _animateJump = true;
                    _velocity.y = _jumpForce;
                    _coyote = 0;
                }
                else
                {
                    _jumpBuffer = airBuffer;
                }
            }
        }
        
        _rb.velocity = _velocity;
    }

    public void Activate()
    {
        IsPaused = false;
        IsActive = true;
    }

    public void EnterLevel()
    {
        _initPos = transform.localPosition;
        _sprite.enabled = true;
        _animator.enabled = true;
        _rb.isKinematic = false;
        AudioManager.Instance.Play("Qube Enter Level");
    }

    public void Vanish()
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        IsPaused = true;
        transform.localPosition = _initPos;
        _animator.Rebind();
        _animator.Update(0f);
        _animator.enabled = false;
        _sprite.enabled = false;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!IsActive)
        {
            _isJumpPressed = _isJumpReleased = false;
        }
        else if (ctx.started)
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
        _xInput = IsActive ? Math.Sign(ctx.ReadValue<float>()) : 0;
    }

    public void MoveTowards(float destX, Action callback)
    {
        IsActive = false;
        _onReachedDestCallback = callback;
        _xInput = Math.Sign(destX - transform.position.x);
        _xInput /= 2;
        _rb.velocity = new Vector2(_xInput * 0.1f, 0);
        _destX = destX;
    }

    private void UpdateForces(float h, float d)
    {
        _height = h;
        _distance = d;
        _jumpForce = (2 * _height * speed) / _distance;
        _gravity = (2 * _height * speed * speed) / (_distance * _distance);
    }

    private void TransitionUpdate()
    {
        _velocity.x = 0;
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        _rb.velocity = _velocity;
        
        _animator.SetBool(AnimatorGrounded, _groundDetector.IsGrounded());
        _animator.SetFloat(AnimatorVelocityY, _rb.velocity.y);
        _animator.SetBool(AnimatorRun, Mathf.Abs(_rb.velocity.x) > 0.05);
    }
}