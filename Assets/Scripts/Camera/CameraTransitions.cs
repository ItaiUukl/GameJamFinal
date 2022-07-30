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
        GameManager.Instance.Init();
        GameManager.Cam = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _player.IsPaused = true;
    }

    public void OnFinishedEnter()
    {
        _player.EnterLevel();
    }

    public void OnStartedExit()
    {
        _player.IsActive = false;
    }

    public void OnFinishedExit()
    {
        GameManager.Instance.LoadLevel();
    }

    public void ShakeCamera()
    {
        if (GameManager.Instance.IsInMainMenu) return;
        Handheld.Vibrate();
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