using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum State { Alive, Dead }

    [Tooltip("The prefab for a single snake body part.")]
    [SerializeField] private GameObject _snakeBodyPartPrefab;

    private const int SortingOrder = 2;
    private const int StartBodySize = 0;
    private State _state;
    private SnakeData.Direction _gridMoveDirection;
    private Vector2Int _gridPosition;
    private float _gridMoveTimer;
    private LevelGrid _levelGrid;
    private int _snakeBodySize;

    private List<SnakeMovePosition> _snakeMovePositionList;
    private List<SnakeBodyPart> _snakeBodyPartPool;

    private Transform _snakeHeadTransform;
    private SpriteRenderer _snakeHeadSpriteRenderer;
    private SnakeData _snakeData;
    private SnakeBodyPartData _snakeBodyPartData;

    public Action<Vector2Int> OnSnakeMove;
    private float _joystickDeadZone = 0.3f;

    private struct SnakeMovePosition
    {
        public Vector2Int GridPosition;
        public SnakeData.Direction Direction;
    }

    private void Awake()
    {
        _snakeData = GameAssets.Instance.SnakeData;
        _snakeBodyPartData = GameAssets.Instance.SnakeBodyPartData;

        _snakeHeadTransform = transform;
        _snakeHeadSpriteRenderer = GetComponent<SpriteRenderer>();
        if (_snakeHeadSpriteRenderer == null)
        {
            _snakeHeadSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        _snakeHeadSpriteRenderer.sprite = _snakeBodyPartData.HeadSprite;
        _snakeHeadSpriteRenderer.sortingOrder = SortingOrder;

        _snakeMovePositionList = new List<SnakeMovePosition>();

        InitializeBodyPartPool();

        SetInitialValues();
    }

    private void OnEnable()
    {
        DPadView.OnDirectionChanged += SetDirection;
    }

    private void OnDisable()
    {
        DPadView.OnDirectionChanged -= SetDirection;
    }

    private void Update()
    {
        if (_state == State.Alive)
        {
            HandleInput();
            HandleGridMovement();
        }
    }

    public void Setup(LevelGrid levelGrid)
    {
        _levelGrid = levelGrid;
    }

    public void SetInitialValues()
    {
        _snakeMovePositionList.Clear();
        _gridPosition = _snakeData.InitialPosition;
        _gridMoveDirection = _snakeData.InitialDirection;
        _gridMoveTimer = _snakeData.MoveTimerMax;

        for (int i = 0; i < StartBodySize; i++)
        {
            AddBodyPart();
            _snakeBodyPartPool[_snakeBodySize].SetPositionAndSprite(_gridPosition, _snakeBodyPartData, _gridMoveDirection);
        }

        _state = State.Alive;
    }

    private void AddBodyPart()
    {
        if (_snakeBodyPartPool.Count <= _snakeBodySize)
        {
            IncreaseBodyPartsPool();
        }

        _snakeBodyPartPool[_snakeBodySize].gameObject.SetActive(true);

        _snakeBodySize++;
    }

    public void Grow()
    {
        AddBodyPart();

        SoundManager.Instance.PlaySound(SoundManager.Sound.SnakeEat);
    }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int> { _gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in _snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GridPosition);
        }
        return gridPositionList;
    }

    public void ResetBodyParts()
    {
        for (int i = 0; i < _snakeBodyPartPool.Count; i++)
        {
            _snakeBodyPartPool[i].gameObject.SetActive(false);
        }

        _snakeBodySize = 0;
    }

    private void InitializeBodyPartPool()
    {
        _snakeBodyPartPool = new List<SnakeBodyPart>();
        for (int i = 0; i < _snakeData.InitialPoolSize; i++)
        {
            GameObject bodyPartGO = Instantiate(_snakeBodyPartPrefab, GameHandler.Instance.SnakePoolPosition);
            SnakeBodyPart bodyPart = bodyPartGO.GetComponent<SnakeBodyPart>();
            if (bodyPart != null)
            {
                _snakeBodyPartPool.Add(bodyPart);
                bodyPartGO.SetActive(false);
            }
            else
            {
                Destroy(bodyPartGO);
            }
        }
    }

    private void SetDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up && _gridMoveDirection != SnakeData.Direction.Down)
        {
            _gridMoveDirection = SnakeData.Direction.Up;
        }
        else if (direction == Vector2Int.down && _gridMoveDirection != SnakeData.Direction.Up)
        {
            _gridMoveDirection = SnakeData.Direction.Down;
        }
        else if (direction == Vector2Int.left && _gridMoveDirection != SnakeData.Direction.Right)
        {
            _gridMoveDirection = SnakeData.Direction.Left;
        }
        else if (direction == Vector2Int.right && _gridMoveDirection != SnakeData.Direction.Left)
        {
            _gridMoveDirection = SnakeData.Direction.Right;
        }
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2Int moveDirection = Vector2Int.zero;

        if (Mathf.Abs(horizontal) > _joystickDeadZone)
        {
            moveDirection.x = (horizontal > 0 ? 1 : -1);
        }
        else if (Mathf.Abs(vertical) > _joystickDeadZone)
        {
            moveDirection.y = (vertical > 0 ? 1 : -1);
        }

        if (moveDirection != Vector2Int.zero)
        {
            SetDirection(moveDirection);
        }
    }

    private void HandleGridMovement()
    {
        _gridMoveTimer += Time.deltaTime;
        if (_gridMoveTimer >= _snakeData.MoveTimerMax)
        {
            _gridMoveTimer = 0;

            SnakeMovePosition newMovePosition = new SnakeMovePosition
            {
                GridPosition = _gridPosition,
                Direction = _gridMoveDirection
            };
            _snakeMovePositionList.Insert(0, newMovePosition);

            Vector2Int gridMoveDirectionVector = GetVectorFromDirection(_gridMoveDirection);
            _gridPosition += gridMoveDirectionVector;

            _gridPosition = _levelGrid.ValidateGridPosition(_gridPosition);

            OnSnakeMove?.Invoke(_gridPosition);

            if (_snakeMovePositionList.Count > _snakeBodySize)
            {
                _snakeMovePositionList.RemoveAt(_snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyPartsMovement();
            CheckForSelfCollision();

            _snakeHeadTransform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            _snakeHeadTransform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            SoundManager.Instance.PlaySound(SoundManager.Sound.SnakeMove);
        }
    }

    private void CheckForSelfCollision()
    {
        for (int i = 0; i < _snakeMovePositionList.Count; i++)
        {
            if (_gridPosition == _snakeMovePositionList[i].GridPosition)
            {
                _state = State.Dead;
                GameHandler.Instance.SnakeDied();
                SoundManager.Instance.PlaySound(SoundManager.Sound.SnakeDie);
                return;
            }
        }
    }

    private void IncreaseBodyPartsPool()
    {
        GameObject bodyPartGO = Instantiate(_snakeBodyPartPrefab, transform.parent);
        SnakeBodyPart bodyPart = bodyPartGO.GetComponent<SnakeBodyPart>();
        _snakeBodyPartPool.Add(bodyPart);
    }

    private void UpdateSnakeBodyPartsMovement()
    {
        int count = _snakeBodySize < _snakeMovePositionList.Count ? _snakeBodySize : _snakeMovePositionList.Count;
        for (int i = 0; i < count; i++)
        {
            _snakeBodyPartPool[i].SetPositionAndSprite(_snakeMovePositionList[i].GridPosition, _snakeBodyPartData, _snakeMovePositionList[i].Direction);
        }
    }

    private Vector2Int GetVectorFromDirection(SnakeData.Direction dir)
    {
        switch (dir)
        {
            case SnakeData.Direction.Right: return Vector2Int.right;
            case SnakeData.Direction.Left: return Vector2Int.left;
            case SnakeData.Direction.Up: return Vector2Int.up;
            case SnakeData.Direction.Down: return Vector2Int.down;
            default: return Vector2Int.zero;
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return n < 0 ? n + 360 : n;
    }
}