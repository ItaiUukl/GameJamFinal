using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 1,
        leftBound = -.3f,
        rightBound = 44.5f;

    [SerializeField] private Room roomToFollow;
    [SerializeField] private bool followRoom = true;

    private bool _roomHasMoved;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomToFollow.IsMoving)
        {
            _roomHasMoved = true;
        }

        if (!_roomHasMoved) return;

        GameObject objToFollow = followRoom && roomToFollow.IsMoving ? roomToFollow.gameObject : _player.gameObject;
        var xPos = Mathf.Clamp(objToFollow.transform.position.x, leftBound, rightBound);
        Vector3 targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}