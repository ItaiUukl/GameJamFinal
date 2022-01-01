using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Player player;
    private HashSet<LayerMask> _groundLayers;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _groundLayers ??= new HashSet<LayerMask>{LayerMask.NameToLayer(GlobalsSO.BorderLayer), 
                                                LayerMask.NameToLayer(GlobalsSO.OutlinesLayer), 
                                                LayerMask.NameToLayer("Default")};
        player ??= GetComponentInParent<Player>();
        _collider = GetComponent<BoxCollider2D>();
        // _collider.size = new Vector2(.95f, .05f);
        // _collider.offset = new Vector2(0f, .5f);
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
