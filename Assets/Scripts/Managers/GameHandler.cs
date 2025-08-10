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
using System;

public class GameHandler : MonoBehaviour 
{
    public static GameHandler Instance;

    [field: SerializeField]
    public Transform SnakePoolPosition { get; private set; }
    public Action OnSnakeDied;
    public Action<int, bool, int> OnGameOver;
    public Action<bool> OnPauseGame;
    internal Action OnGameRestart;

    public IScoreManager ScoreManager => _scoreManager;
    private ScoreManager _scoreManager;

    private LevelGrid _levelGrid;
    private Food _food;
    public Snake _snake;

    private void Awake() 
    {
        Instance = this;

        _levelGrid = new LevelGrid(20, 20);
        _scoreManager = new ScoreManager();
        _food = new Food();

        Time.timeScale = 1f;
    }

    private void Start() 
    {
        Debug.Log("GameHandler.Start");

        _snake = Instantiate(GameAssets.Instance.snakePrefab);
        _snake.Setup(_levelGrid);

        _snake.OnSnakeMove += OnSnakeMovePosition;
        _food.OnEatFood += OnEatFoodEvent;

        SpawnFood();
    }

    private void OnDestroy()
    {
        _food.OnEatFood -= OnEatFoodEvent;
        _snake.OnSnakeMove -= OnSnakeMovePosition;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (IsGamePaused()) 
            {
                ResumeGame();
            } 
            else 
            {
                PauseGame();
            }
        }
    }

    private void OnSnakeMovePosition(Vector2Int position)
    {
        _food.TrySnakeEatFood(position);
    }

    private Vector2Int GetFoodPosition()
    {
        Vector2Int foodGridPosition;
        do
        {
            foodGridPosition = _levelGrid.GetRandomPosition();
        } while (_snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        return foodGridPosition;
    }

    private void SpawnFood()
    {
        _food.SetPosition(GetFoodPosition())
            .WithSprite(GameAssets.Instance.foodSprite)
            .Build();
    }

    private void OnEatFoodEvent()
    {
        _scoreManager.AddScore();
        _snake.Grow();
        SpawnFood();
    }

    public void SnakeDied() 
    {
        bool isNewHighscore = _scoreManager.TrySetNewHighscore();
        OnGameOver.Invoke(_scoreManager.score, isNewHighscore, _scoreManager.highscore);
        OnSnakeDied?.Invoke();
    }

    public void ResumeGame()
    {
        OnPauseGame?.Invoke(false);
        Time.timeScale = 1f;
    }

    private void PauseGame() 
    {
        Time.timeScale = 0f;
        OnPauseGame?.Invoke(true);
    }

    public static bool IsGamePaused() 
    {
        return Time.timeScale == 0f;
    }

    public void GoToMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void LoadLevel()
    {
        OnGameRestart?.Invoke();
        _scoreManager.SetInitialValues();
        _snake.ResetBodyParts();
        _snake.SetInitialValues();
        ResumeGame();
    }
}
