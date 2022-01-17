using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private bool _reloadOnExit;

    private static readonly int Shake = Animator.StringToHash("Shake");
    private static readonly int Exit = Animator.StringToHash("Exit");


    private void Awake()
    {
        GameManager.Cam = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _player.IsActive = false;
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

    public void ShakeCamera()
    {
        _animator.SetTrigger(Shake);
    }

    public void ExitTransition(bool reload)
    {
        _reloadOnExit = reload;

        _animator.SetTrigger(Exit);
    }
}