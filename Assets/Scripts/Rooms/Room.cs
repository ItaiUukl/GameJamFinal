using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 500;
    public int RoomGroup { get; set; }

    private Player _player;
    private Rigidbody2D _body;
    private PolygonCollider2D _collider;
    private PolygonCollider2D _outlineCollider;
    private Vector2 _moveDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Rooms");
        _collider = GetComponent<PolygonCollider2D>();
        _outlineCollider = RoomsManager.Instance.RegisterRoom(this);
        
        _body = gameObject.AddComponent<Rigidbody2D>();
        _body.bodyType = RigidbodyType2D.Dynamic;
        _body.isKinematic = true;
        _body.gravityScale = 0;
        _body.mass = 1000000;
        _body.drag = 1000000;
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;

        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        // transform.Translate(_moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    // private void Update()
    // {
    //     transform.Translate(_moveDir * moveSpeed * Time.deltaTime);
    //     // transform.position += (Vector3) _moveDir * moveSpeed * Time.deltaTime;
    // }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     switch (LayerMask.LayerToName(other.gameObject.layer))
    //     {
    //         case "Rooms":
    //             _body.isKinematic = true;
    //             PolygonCollider2D tempColl = GetComponent<PolygonCollider2D>();
    //             _outlineCollider.points = tempColl.points.Select(t => (Vector2) tempColl.transform.TransformPoint(t))
    //                 .ToArray();
    //             // Room room = other.collider.GetComponent<Room>();
    //             // if (room)
    //             // {
    //             //     RoomsManager.Instance.RoomConnection(this, room);
    //             // }
    //             break;
    //         case "Player":
    //             // other.transform.SetParent(transform);
    //             break;
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_moveDir.magnitude == 0) return;
        StartCoroutine(Collide());

        // switch (LayerMask.LayerToName(other.gameObject.layer))
        // {
        //     case "Default":
        //         break;
        //     case "Border":
        //         _player.RoomStopping(this);
        //         break;
        //     case "Rooms":
        //         _body.mass = 1000000;
        //         _body.drag = 1000000;
        //         Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
        //         _moveDir = Vector2.zero;
        //         // _body.velocity = Vector2.zero;
        //         PolygonCollider2D tempColl = GetComponent<PolygonCollider2D>();
        //         _outlineCollider.points = tempColl.points.Select(t => (Vector2) tempColl.transform.TransformPoint(t))
        //             .ToArray();
        //         // TODO: unfreeze player
        //         _player.RoomStopping(this);
        //         break;
        //     case "Player":
        //         // other.transform.SetParent(transform);
        //         break;
        // }
    }


    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Rooms"))
    //     {
    //         Room room = other.GetComponent<Room>();
    //         
    //         if (room)
    //         {
    //             RoomsManager.Instance.RoomDisconnection(this);
    //         }
    //     }
    // }

    // private void SplitCollider(EdgeCollider2D edgeCol, int i, List<Vector2> newPoints, Vector2 A, Vector2 B)
    // {
    //     newPoints.Add(A);
    //     edgeCol.points = newPoints.ToArray();
    //     newPoints.Clear();
    //     newPoints.Add(edgeCol.points[i+1]);
    //     newPoints.Add(B);
    // }

    // Moves room until collision
    public void Move(Vector2 dir)
    {
        // TODO: freeze player
        _moveDir = dir;
        _body.isKinematic = false;
        _player.RoomMoving(this);
        Debug.Log("Dynamic");
        _body.velocity = moveSpeed * dir;
        _body.mass = 1;
        _body.drag = 0;
    }

    private IEnumerator Collide()
    {
        yield return new WaitForSeconds(0.05f);
        if (!(_body.velocity.magnitude < .005f)) yield break;
        _body.mass = 1000000;
        _body.drag = 1000000;
        _body.isKinematic = true;
        _moveDir = Vector2.zero;
        // _body.velocity = Vector2.zero;
        _outlineCollider.points = _collider.points.Select(t => (Vector2) _collider.transform.TransformPoint(t))
            .ToArray();
        // TODO: unfreeze player
        _player.RoomStopping(this);
    }
}