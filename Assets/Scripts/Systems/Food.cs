using UnityEngine;
using System;

public class Food
{
    public Action OnEatFood;
    public Vector2 position { private set; get; }

    private SpriteRenderer _food;
    private Sprite _sprite;

    public Food SetPosition(Vector2 position)
    {
        this.position = position;
        return this;
    }

    public Food WithSprite(Sprite sprite)
    {
        _sprite = sprite;
        return this;
    }

    public Food Build()
    {
        if (_food == null)
        {
            GameObject foodGo = new GameObject("Food", typeof(SpriteRenderer));
            _food = foodGo.GetComponent<SpriteRenderer>();
            _food.sortingOrder = 1;
        }
        _food.gameObject.transform.position = position;
        _food.sprite = _sprite;
        _food.gameObject.SetActive(true);

        return this;
    }

    public void Hide()
    {
        _food.gameObject.SetActive(false);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == position)
        {
            Hide();
            OnEatFood?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
}