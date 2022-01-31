using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private static readonly int AnimatorShake = Animator.StringToHash("Shake");
    private static readonly int AnimatorShakeSlow = Animator.StringToHash("ShakeSlow");
    private static readonly int AnimatorExit = Animator.StringToHash("Exit");
    private static readonly int AnimatorFinish = Animator.StringToHash("Finish");

    private void Awake()
    {
        GameManager.Cam = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        if (_player)
        {
            _player.Freeze();
        }
    }

    public void OnFinishedEnter()
    {
        if (_player)
        {
            _player.EnterLevel();
        }
    }

    public void OnStartedExit()
    {
        if (_player)
        {
            _player.Freeze();
        }
    }

    public void OnFinishedExit()
    {
        GameManager.Instance.LoadLevel();
    }

    public void ShakeCamera()
    {
        _animator.SetTrigger(AnimatorShake);
    }

    public void SetSlowShake(bool value)
    {
        _animator.SetBool(AnimatorShakeSlow, value);
    }

    public void ExitTransition(bool isFinish)
    {
        _animator.SetTrigger(isFinish ? AnimatorFinish : AnimatorExit);
    }
}