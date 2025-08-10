/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour 
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _newHighscoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;

    private void Awake()
    {
        GameHandler.Instance.OnGameRestart += OnGameRestartEvent;
        GameHandler.Instance.OnGameOver += OnGameOverEvent;
        Hide();
    }

    private void OnDestroy()
    {
        GameHandler.Instance.OnGameRestart -= OnGameRestartEvent;
        GameHandler.Instance.OnGameOver -= OnGameOverEvent;
    }

    private void OnEnable()
    {
        _retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    private void OnDisable()
    {
        _retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        GameHandler.Instance.LoadLevel();
    }

    private void Hide() 
    {
        gameObject.SetActive(false);
    }

    private void OnGameOverEvent(int score, bool isNewHighscore, int highscore)
    {
        gameObject.SetActive(true);

        _newHighscoreText.gameObject.SetActive(isNewHighscore);
        _scoreText.SetText("{0}", score);
        _highscoreText.SetText("{0}", highscore);
    }

    private void OnGameRestartEvent()
    {
        Hide();
    }
}
