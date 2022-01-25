using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private const float SideSize = .2f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(GlobalsSO.BorderLayer);
        _sprite = GetComponent<SpriteRenderer>();
        GameObject temp;
        MoveDirection side;
        Vector2 dir;
        Array directions = Enum.GetValues(typeof(MoveDirection));
        int offset = (int) transform.eulerAngles.z / 90;
        for (int i = 0; i < directions.Length; ++i)
        {
            side = (MoveDirection) directions.GetValue(i);
            dir = MoveDirectionUtils.ToVector2((MoveDirection) directions.GetValue((i + offset) % directions.Length));
            temp = new GameObject();
            temp.AddComponent<Directional>().GenerateCollider(transform, side, dir, _sprite.size, SideSize);
            temp.layer = LayerMask.NameToLayer(GlobalsSO.BorderLayer);
            temp.name = side + " wall side " + name;
        }
    }
}