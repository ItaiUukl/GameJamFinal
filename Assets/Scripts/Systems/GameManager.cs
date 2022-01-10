using UnityEngine;
using UnityEngine.SceneManagement;

public enum MoveDirection
{
    Up,
    Right,
    Down,
    Left
}

public class GameManager : Singleton<GameManager>
{
    private GlobalsSO _globals;
    private int _currLevel = 0;

    private void Awake()
    {
        _globals = Resources.LoadAll<GlobalsSO>("Globals")[0];
    }

    public static Vector2 GetDirection(MoveDirection dir)
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

    public void NextLevel()
    {
        RoomsManager.Instance.ResetLevel();
        _currLevel = (_currLevel + 1) % _globals.levelAdvancement.Count;
        SceneManager.LoadScene(_globals.AdvanceLevel(_currLevel));
    }

    // Resets the current level
    public void ReloadLevel()
    {
        RoomsManager.Instance.ResetLevel();
        SceneManager.LoadScene(_globals.AdvanceLevel(_currLevel));
    }
}