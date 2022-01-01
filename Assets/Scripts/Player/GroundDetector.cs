using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HashSet<LayerMask> groundLayers;
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        groundLayers ??= new HashSet<LayerMask>{LayerMask.NameToLayer(GlobalsSO.BorderLayer), 
                                                LayerMask.NameToLayer(GlobalsSO.OutlinesLayer), 
                                                LayerMask.NameToLayer("Default")};
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!groundLayers.Contains(other.gameObject.layer)) return;
        
        player.IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!groundLayers.Contains(other.gameObject.layer)) return;

        player.IsGrounded = false;
    }
}
