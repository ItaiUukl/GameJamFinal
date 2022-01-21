using UnityEngine;

public class Directional : MonoBehaviour
{
    [SerializeField] protected MoveDirection _side;

    public MoveDirection GetSide()
    {
        return _side;
    }
}
