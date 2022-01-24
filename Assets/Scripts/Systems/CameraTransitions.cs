using DG.Tweening;
using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    private Player _player;

    // private Animator _animator;
    private Transform _levelTransition;
    private Transform _camTransform;

    private bool _reloadOnExit;

    [Header("Transition Animation Settings")]
    [SerializeField] private float transitionDuration = .5f;
    private const Ease TransitionEase = Ease.OutCubic;

    // private static readonly int Shake = Animator.StringToHash("Shake");
    // private static readonly int Exit = Animator.StringToHash("Exit");

    [Header("Camera Shake Animation Settings")]
    [SerializeField] private float shakeStrength = .5f;
    [SerializeField] private float shakeDuration = .35f;
    [SerializeField] private int shakeVibrato = 15;

    private void Awake()
    {
        if (_camTransform == null)
        {
            _camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        GameManager.Cam = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _levelTransition = transform.GetChild(0);
        // _animator = GetComponent<Animator>();
        _player.IsActive = false;
        PlayEnterAnimation();
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

    public void ShakeCamera(Vector2 dir, float strength)
    {
        _camTransform.DOShakePosition(shakeDuration, dir * shakeStrength * strength, (int) (shakeVibrato * strength));

        // _animator.SetTrigger(Shake);
    }

    public void ExitTransition(bool reload)
    {
        _reloadOnExit = reload;
        PlayExitAnimation();
        // _animator.SetTrigger(Exit);
    }

    private void PlayEnterAnimation()
    {
        SpriteRenderer lTSprite = _levelTransition.GetComponent<SpriteRenderer>();
        var lTCol = lTSprite.color;
        lTCol.a = 255;
        lTSprite.color = lTCol;
        _camTransform.DOMoveX(-1.5f, transitionDuration)
            .From()
            .SetEase(TransitionEase);
        _levelTransition.transform.DOMoveX(23, transitionDuration)
            .SetEase(TransitionEase)
            .OnComplete(OnFinishedEnter);
    }

    private void PlayExitAnimation()
    {
        _levelTransition.transform.localPosition = new Vector3(-23, 0, 20);
        _camTransform.DOMoveX(1.5f, transitionDuration)
            .SetEase(TransitionEase);
        _levelTransition.transform.DOMoveX(0, transitionDuration)
            .SetEase(TransitionEase)
            .OnPlay(OnStartedExit)
            .OnComplete(OnFinishedExit);
    }
}