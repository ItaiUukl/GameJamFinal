using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Player player;
    private HashSet<LayerMask> _groundLayers;
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _groundLayers ??= new HashSet<LayerMask>
        {
            // LayerMask.NameToLayer(GlobalsSO.BorderLayer),
            LayerMask.NameToLayer(GlobalsSO.OutlinesLayer),
            LayerMask.NameToLayer(GlobalsSO.DefaultLayer)
        };
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_groundLayers.Contains(other.gameObject.layer)) return;
        player.IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_groundLayers.Contains(other.gameObject.layer)) return;
        player.IsGrounded = false;
    }
}