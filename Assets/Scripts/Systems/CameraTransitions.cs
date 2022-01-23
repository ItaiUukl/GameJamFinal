using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private bool _reloadOnExit;

    private static readonly int Shake = Animator.StringToHash("Shake");
    private static readonly int Exit = Animator.StringToHash("Exit");

    // shake fields

    private bool _isShaking;
    private Vector2 _shakeDir;
    private Transform _camTransform;
    private Vector2 _originalPos;

    // How long the object should shake for.
    private float _curShakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private float decreaseFactor = 1.0f;
    [SerializeField] private float shakeDuration = 1f;

    private void Awake()
    {
        if (_camTransform == null)
        {
            _camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        GameManager.Cam = this;
    }

    private void OnEnable()
    {
        _originalPos = _camTransform.localPosition;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _player.IsActive = false;
        _camTransform.localPosition = new Vector2(10, 10);
    }

    private void Update()
    {
        if (_isShaking)
        {
            if (_curShakeDuration > 0)
            {
                _camTransform.localPosition = _originalPos + _shakeDir * shakeAmount;
        
                _curShakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                _curShakeDuration = 0f;
                _camTransform.localPosition = _originalPos;
                _isShaking = false;
            }
        }
    }

    public void OnFinishedEnter()
    {
        _player.IsActive = true;
    }

    public void OnStartedExit()
    {
        _player.IsActive = false;
    }

    public void OnFinishedExit()
    {
        if (_reloadOnExit)
        {
            GameManager.Instance.ReloadLevel();
        }
        else
        {
            GameManager.Instance.NextLevel();
        }
    }

    public void ShakeCamera(Vector2 dir)
    {
        _isShaking = true;
        _shakeDir = dir;
        _curShakeDuration = shakeDuration;
        // _animator.SetTrigger(Shake);
    }

    public void ExitTransition(bool reload)
    {
        _reloadOnExit = reload;

        _animator.SetTrigger(Exit);
    }
}