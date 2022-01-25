using System;
using UnityEngine;

public class Directional : MonoBehaviour
{
    [SerializeField] protected MoveDirection _side;
    [SerializeField] protected bool _isBorder;

    private void Awake()
    {
        if (_isBorder)
        {
            _side = MoveDirectionUtils.ToOppositeDir(_side);
        }
    }

    public MoveDirection GetSide()
    {
        return _side;
    }

    public BoxCollider2D GenerateCollider(Transform parent, MoveDirection side, Vector2 dir, Vector2 dimensions, float breadth)
    {
        transform.SetParent(parent, false);
        _side = side;
        float collOffset = 2f * breadth + 4f * Physics2D.defaultContactOffset + .08f;
        BoxCollider2D resCollider = gameObject.AddComponent<BoxCollider2D>();
        resCollider.size = dir.x == 0
            ? new Vector2(dimensions.x - collOffset, breadth)
            : new Vector2(breadth, dimensions.y - collOffset);
        resCollider.offset = dir * dimensions / 2 - dir * (breadth + .08f) / 2;
        resCollider.isTrigger = true;

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.bodyType = RigidbodyType2D.Kinematic;

        return resCollider;
    }
}