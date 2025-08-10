/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGrid
{
    private int _width;
    private int _height;

    public LevelGrid(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public Vector2Int GetRandomPosition()
    {
        return new Vector2Int(Random.Range(0, _width), Random.Range(0, _height));
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = _width - 1;
        }
        if (gridPosition.x > _width - 1)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = _height - 1;
        }
        if (gridPosition.y > _height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}