using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private static readonly int AnimatorShake = Animator.StringToHash("Shake");
    private static readonly int AnimatorShakeSlow = Animator.StringToHash("ShakeSlow");
    private static readonly int AnimatorExit = Animator.StringToHash("Exit");
    
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

    public void ShakeCamera(Vector2 dir, float strength)
    {
        Debug.Log("shake strength = " + strength + " is slow == "+ (strength < .4f));
        _animator.SetBool(AnimatorShakeSlow, strength < .4f);
        _animator.SetTrigger(AnimatorShake);
    }

    public void ExitTransition()
    {
        _animator.SetTrigger(AnimatorExit);
    }
}