using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fireworks : MonoBehaviour
{
    [SerializeField] private GameObject firework;
    [SerializeField] private float secondsBetweenShots = .5f;
    private Collider2D _collider;
    private bool _isIn;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(GlobalsSO.PlayerLayer) && !_isIn) return;
        _isIn = true;
        StartCoroutine(Shoot());
        AudioManager.Instance.Play("EndingNote");
    }

    private IEnumerator Shoot()
    {
        Vector2 boxMax = _collider.bounds.max, boxMin = _collider.bounds.min;
        while (true)
        {
            var position = new Vector2(Random.Range(boxMin.x, boxMax.x), Random.Range(boxMin.y, boxMax.y));
            Instantiate(firework, position, Quaternion.identity);
            yield return new WaitForSeconds(secondsBetweenShots);
        }
    }
}