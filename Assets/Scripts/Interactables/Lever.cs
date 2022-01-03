using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Lever : MonoBehaviour
{
    [SerializeField] private MoveDirection direction;
    [SerializeField] private Room room;

    private Collider2D _collider;
    private Vector2 _vecDir;
    private bool _active;

    // Start is called before the first frame update
    void Start()
    {
        room = room ? room : GetComponentInParent<Room>();
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _vecDir = direction switch
        {
            MoveDirection.Up => Vector2.up,
            MoveDirection.Right => Vector2.right,
            MoveDirection.Down => Vector2.down,
            MoveDirection.Left => Vector2.left,
            _ => _vecDir
        };
    }

    private void Update()
    {
        // TODO: much more efficient to have the active levers register to an event and only game manager to check input.
        if (_active && Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Activate();
        }
    }

    // Activates lever
    public void Activate()
    {
        room.Move(direction);
        // TODO: Trigger lever animation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            _active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer))
        {
            _active = false;
        }
    }
}