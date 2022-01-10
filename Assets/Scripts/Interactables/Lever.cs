using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Lever : MonoBehaviour
{
    [SerializeField] public MoveDirection direction;
    [SerializeField] private Room room;

    private Collider2D _collider;
    private SpriteRenderer _sprite;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        room = room ? room : GetComponentInParent<Room>();
        room.AddLever(this);

        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetActivation(bool state)
    {
        _collider.enabled = state;
        Color tempColor = _sprite.color;
        tempColor.a = state ? 1f : .3f;
        _sprite.color = tempColor;
        SetAnimator(!state);
    }

    public void SetAnimator(bool state)
    {
        _animator.SetBool("Pressed", state);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            SetAnimator(true);
            room.Move(direction);
        }
    }
}