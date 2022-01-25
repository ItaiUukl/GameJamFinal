using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Lever : MonoBehaviour
{
    [SerializeField] public MoveDirection direction;
    [SerializeField] private Room room;

    private Collider2D _collider;
    private SpriteRenderer _sprite;
    private Animator _animator;

    private static readonly int Pressed = Animator.StringToHash("Pressed");
    private static readonly int Disabled = Animator.StringToHash("Disabled");

    private bool _pressable = true;

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
        _collider.enabled = _pressable = state;
        Color tempColor = _sprite.color;
        tempColor.a = state ? 1f : .7f;
        _animator.SetBool(Disabled, !state);
        _sprite.color = tempColor;
    }

    public void Moving(bool state)
    {
        Debug.Log(name + " pressable: " + _pressable);
        _collider.enabled = _pressable && !state;
    }

    private void OnPressed()
    {
        _collider.enabled = false;
        room.Move(direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer) && _pressable && !room.IsMoving)
        {
            _pressable = false;
            _animator.SetTrigger(Pressed);
        }
    }
}