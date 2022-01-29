using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private float followSpeed = 1,
        leftBound = -.3f,
        rightBound = 47.1f;

    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var xPos = Mathf.Clamp(_player.position.x, leftBound, rightBound);
        Vector3 targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}