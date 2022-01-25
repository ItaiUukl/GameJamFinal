using UnityEngine;

public enum MoveDirection
{
    Up,
    Right,
    Down,
    Left
}

public static class MoveDirectionUtils
{
    public static Vector2 ToVector2(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => Vector2.up,
            MoveDirection.Right => Vector2.right,
            MoveDirection.Down => Vector2.down,
            MoveDirection.Left => Vector2.left,
            _ => Vector2.zero
        };
    }

    public static MoveDirection ToOppositeDir(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => MoveDirection.Down,
            MoveDirection.Right => MoveDirection.Left,
            MoveDirection.Down => MoveDirection.Up,
            MoveDirection.Left => MoveDirection.Right,
            _ => dir
        };
    }

    public static Vector2 ToOppositeVector2(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => Vector2.down,
            MoveDirection.Right => Vector2.left,
            MoveDirection.Down => Vector2.up,
            MoveDirection.Left => Vector2.right,
            _ => Vector2.zero
        };
    }
}