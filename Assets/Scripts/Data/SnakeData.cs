using UnityEngine;

[CreateAssetMenu(fileName = "SnakeData", menuName = "Martin Luquet/Snake Data", order = 1)]
public class SnakeData : ScriptableObject
{
    [Tooltip("Initial position of the snake")]
    public Vector2Int InitialPosition = new Vector2Int(10, 10);
    [Tooltip("Initial movement direction of the snake")]
    public Direction InitialDirection = Direction.Right;
    [Tooltip("Time between grid movements")]
    public float MoveTimerMax = 0.2f;
    [Tooltip("Snake body parts to pre-create for the pool.")]
    public int InitialPoolSize = 30;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}