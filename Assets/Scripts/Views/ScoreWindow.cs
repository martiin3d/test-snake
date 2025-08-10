/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using TMPro;
using UnityEngine;

public class ScoreWindow : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;

    private void Awake()
    {
        GameHandler.Instance.OnGameRestart += OnGameRestartEvent;
    }

    private void OnDestroy()
    {
        GameHandler.Instance.OnGameRestart -= OnGameRestartEvent;
    }

    private void Start()
    {
        SetInitialValues();
    }

    private void OnEnable()
    {
        GameHandler.Instance.ScoreManager.OnHighscoreChanged += Score_OnHighscoreChanged;
        GameHandler.Instance.ScoreManager.OnScoreChanged += Score_OnScoreChanged;
        GameHandler.Instance.OnSnakeDied += HideUI;
    }

    private void OnDisable()
    {
        GameHandler.Instance.ScoreManager.OnHighscoreChanged -= Score_OnHighscoreChanged;
        GameHandler.Instance.ScoreManager.OnScoreChanged -= Score_OnScoreChanged;
        GameHandler.Instance.OnSnakeDied -= HideUI;
    }

    private void SetInitialValues()
    {
        _scoreText.text = "0";
        _highscoreText.SetText("{0}", GameHandler.Instance.ScoreManager.highscore);
    }

    private void Score_OnScoreChanged(int score)
    {
        _scoreText.SetText("{0}", score);
    }

    private void Score_OnHighscoreChanged(int highscore)
    {
        _highscoreText.SetText("{0}", highscore);
    }

    private void HideUI() 
    {
        gameObject.SetActive(false);
    }

    private void OnGameRestartEvent()
    {
        SetInitialValues();
        gameObject.SetActive(true);
    }
}
