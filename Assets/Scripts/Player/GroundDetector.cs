using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    // private HashSet<LayerMask> _groundLayers;
    [SerializeField] private ParticleSystem dust;
    private Player _player;
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        // _groundLayers ??= new HashSet<LayerMask>
        // {
        //     // LayerMask.NameToLayer(GlobalsSO.BorderLayer),
        //     LayerMask.NameToLayer(GlobalsSO.OutlinesLayer),
        //     LayerMask.NameToLayer(GlobalsSO.DefaultLayer)
        // };
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _player = GetComponentInParent<Player>();
    }

    public bool IsGrounded()
    {
        return _collider.IsTouchingLayers();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(dust, transform.position, transform.rotation, _player.transform.parent);
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!_groundLayers.Contains(other.gameObject.layer)) return;
    //     player.IsGrounded = _collider.IsTouchingLayers();
    // }
}