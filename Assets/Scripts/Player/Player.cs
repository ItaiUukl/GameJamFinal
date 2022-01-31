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

    public bool isActive;
    public bool isPaused = true;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private GroundDetector _groundDetector;

    private Vector3 _initPos;
    private Vector2 _velocity = Vector2.zero;
    private float _xInput, _jumpForce, _gravity;

    private float _coyote, _jumpBuffer;

    private float _height, _distance;

    private bool _isJumpPressed, _isJumpReleased;

    private Action _onReachedDestCallback = null;
    private float? _destX = null;
    private bool IsWalkingToDoor => _destX != null;

    public Transform roomToEnter;

    private static readonly int AnimatorVelocityY = Animator.StringToHash("VelocityY"),
        AnimatorRun = Animator.StringToHash("Run"),
        AnimatorJump = Animator.StringToHash("Jump"),
        AnimatorGrounded = Animator.StringToHash("Grounded");

    // Use this for initialization
    void Start()
    {
        _initPos = transform.position;
        _height = maxJumpPeak;
        _distance = maxPeakDistance;
        UpdateForces();

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

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        if (IsWalkingToDoor && (transform.position.x * _xInput > _destX * _xInput || _rb.velocity.x == 0))
        {
            _onReachedDestCallback.Invoke();
            _rb.velocity = Vector2.zero;
            _xInput = 0;
            _destX = null;
            isPaused = true;
            return;
        }

        _velocity.x = _xInput * speed;
        if (_xInput != 0) _sprite.flipX = _xInput < 0;
        // todo: particle system

        if (_groundDetector.IsGrounded() || IsWalkingToDoor)
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
                if (_velocity.x != 0)
                {
                    AudioManager.Instance.Play("Run");
                }

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
        isPaused = false;
        isActive = true;
    }

    public void EnterLevel()
    {
        _sprite.enabled = true;
        _animator.enabled = true;
        AudioManager.Instance.Play("Qube Enter Level");
    }

    public void Vanish()
    {
        Debug.Log("pos = " + _initPos);
        _sprite.enabled = false;
        _animator.enabled = false;
        isPaused = true;
        GetComponent<Collider2D>().enabled = false;
        transform.position = _initPos;
    }

    private void UpdateForces()
    {
        _jumpForce = (2 * _height * speed) / _distance;
        _gravity = (2 * _height * speed * speed) / (_distance * _distance);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isActive)
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
        _xInput = isActive ? Math.Sign(ctx.ReadValue<float>()) : 0;
    }

    public void MoveTowards(float destX, Action callback)
    {
        isActive = false;
        _onReachedDestCallback = callback;
        _xInput = Math.Sign(destX - transform.position.x);
        _xInput /= 2;
        _rb.velocity = new Vector2(_xInput * 0.1f, 0);
        _destX = destX;
    }
}