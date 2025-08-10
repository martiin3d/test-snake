using UnityEngine;
using static SnakeData;

public class SnakeBodyPart : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetPositionAndSprite(Vector2Int gridPosition, SnakeBodyPartData partsData, Direction currentDir)
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        _spriteRenderer.sprite = partsData.BodySprite;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromDirection(currentDir));
    }

    private float GetAngleFromDirection(Direction direction)
    {
        switch (direction)
        {
            default:
            case Direction.Up: return 0;
            case Direction.Right: return -90;
            case Direction.Down: return 180;
            case Direction.Left: return 90;
        }
    }
}