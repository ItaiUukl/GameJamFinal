using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Move player in 2D space
    [SerializeField] private Room roomToMove;
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    private Room _currRoom = null;
    float cooldown = 0;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
        //ps = t.GetChild(1).transform.gameObject;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currRoom)
        {
            return;
        }
        // Movement controls
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (isGrounded || Mathf.Abs(r2d.velocity.x) > 0.01f))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }

        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }

            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
         if (Input.GetKeyDown(KeyCode.Space) && isGrounded && cooldown <= 0)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            cooldown = 0.7f;
            // Destroy(t.GetChild(t.childCount-1).transform.gameObject);

        }
        
            cooldown -= Time.deltaTime;


        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Reload scene
            GameManager.Instance.ReloadLevel();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            //Exit the game
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos =
            colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            foreach (var t1 in colliders)
            {
                if (t1 != mainCollider)
                {
                    isGrounded = true;
                    /* ParticleSystem
                    if(!particleActive){
                    // checking if the particle system flag is active and create new object if it isn't
                        //Instantiate(ps, t, true);
                       // particleActive = true;
                    }
                    */
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

        // Simple debug
        // Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        // Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }

    public void RoomMoving(Room room)
    {
        if (Physics2D.OverlapCircleAll(transform.position, 1f).Contains(room.GetComponent<Collider2D>()))
        {
            _currRoom = room;
            r2d.isKinematic = true;
            mainCollider.enabled = false;
            transform.SetParent(room.transform);
        }
    }

    public void RoomStopping(Room room)
    {
        if (room == _currRoom)
        {
            transform.SetParent(null);
            r2d.isKinematic = false;
            mainCollider.enabled = true;
            _currRoom = null;
        }
    }
}