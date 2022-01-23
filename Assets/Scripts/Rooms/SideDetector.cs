using System;
using UnityEngine;

public class SideDetector : MonoBehaviour
{
    private MoveDirection _side;
    private Action<MoveDirection, Collider2D> _triggerEnter;
    private Action<MoveDirection> _triggerExit;
    private BoxCollider2D _collider;

    public void InitSide(Transform parent, MoveDirection side, Vector2 dimensions, float breadth,
        Action<MoveDirection, Collider2D> enter, Action<MoveDirection> exit)
    {
        name = side + "SideDetector" + parent.name;
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer);
        transform.SetParent(parent, false);
        _side = side;
        _triggerEnter = enter;
        _triggerExit = exit;

        Vector2 dir = MoveDirectionFunctions.ToVector2(side);

        float collOffset = 2f * breadth + 4f * Physics2D.defaultContactOffset + .08f;
        _collider = gameObject.AddComponent<BoxCollider2D>();
        _collider.size = dir.x == 0
            ? new Vector2(dimensions.x - collOffset, breadth)
            : new Vector2(breadth, dimensions.y - collOffset);
        _collider.offset = dir * dimensions / 2 - dir * (breadth + .08f) / 2;
        _collider.isTrigger = true;

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.RoomsLayer) ||
            other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.BorderLayer))
        {
            _triggerEnter(_side, other);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.PlayerLayer) &&
                 other.gameObject.transform.parent != transform.parent)
        {
            other.gameObject.transform.SetParent(transform.parent);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LayerMask roomsLayer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer),
            borderLayer = LayerMask.NameToLayer(GlobalsSO.BorderLayer);
        if ((other.gameObject.layer == borderLayer || other.gameObject.layer == roomsLayer) &&
            !(_collider.IsTouchingLayers(borderLayer) || _collider.IsTouchingLayers(roomsLayer)))
        {
            _triggerExit(_side);
        }
    }
}