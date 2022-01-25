using System;
using UnityEngine;

public class SideDetector : Directional
{
    private Action<MoveDirection, Collider2D> _triggerEnter;
    private Action<MoveDirection> _triggerExit;
    private BoxCollider2D _collider;

    public void InitSide(Transform parent, MoveDirection side, Vector2 dimensions, float breadth,
        Action<MoveDirection, Collider2D> enter, Action<MoveDirection> exit)
    {
        name = side + " Side Detector " + parent.name;
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.RoomsLayer);
        _triggerEnter = enter;
        _triggerExit = exit;
        _collider = GenerateCollider(parent, side, GameManager.GetDirection(side), dimensions, breadth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.RoomsLayer) ||
            other.gameObject.layer == LayerMask.NameToLayer(GlobalsSO.BorderLayer))
        {
            Directional directional = other.GetComponent<Directional>();
            if (directional && directional.GetSide() == GameManager.OppositeDirection(_side))
            {
                _triggerEnter(_side, other);
            }
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
            Directional directional = other.GetComponent<Directional>();
            if (directional && directional.GetSide() == GameManager.OppositeDirection(_side))
            {
                _triggerExit(_side);
            }
        }
    }
}