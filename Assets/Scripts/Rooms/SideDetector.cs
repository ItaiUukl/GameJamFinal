using System;
using UnityEngine;

public class SideDetector : MonoBehaviour
{
    private MoveDirection _side;
    private Action<MoveDirection, Collider2D> _triggerEnter;
    private Action<MoveDirection> _triggerExit;

    public void InitSide(Transform parent, MoveDirection side, Vector2 dimensions, float breadth,
        Action<MoveDirection, Collider2D> enter, Action<MoveDirection> exit)
    {
        name = side + "SideDetector" + parent.name;
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer);
        transform.SetParent(parent, false);
        _side = side;
        _triggerEnter = enter;
        _triggerExit = exit;

        Vector2 dir = GameManager.GetDirection(side);

        float collOffset = 2f * breadth + 4f * Physics2D.defaultContactOffset + .08f;
        BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = dir.x == 0
            ? new Vector2(dimensions.x - collOffset, breadth)
            : new Vector2(breadth, dimensions.y - collOffset);
        coll.offset = dir * dimensions / 2 - dir * (breadth + .08f) / 2;
        coll.isTrigger = true;

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _triggerEnter(_side, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _triggerExit(_side);
    }
}