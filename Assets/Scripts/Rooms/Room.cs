using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Room : MonoBehaviour
{
    public int RoomGroup { get; set; }

    private Rigidbody2D _body;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Rooms");
        RoomsManager.Instance.RegisterRoom(this);
        GetComponent<PolygonCollider2D>().isTrigger = true;
        //
        // _body = gameObject.AddComponent<Rigidbody2D>();
        // _body.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case "Rooms":
                Room room = other.GetComponent<Room>();
            
                if (room)
                {
                    RoomsManager.Instance.RoomConnection(this, room);
                }
                break;
            case "Player":
                other.transform.SetParent(transform);
                break;
        }
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
        _body.velocity = 3 * dir;
    }
}