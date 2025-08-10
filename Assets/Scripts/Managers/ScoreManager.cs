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
using UnityEngine;

public class ScoreManager : IScoreManager
{
    public event Action<int> OnHighscoreChanged;
    public event Action<int> OnScoreChanged;

    public int score { private set; get; }
    public int highscore => GetHighscore();

    public ScoreManager()
    {
        SetInitialValues();
    }

    public void SetInitialValues()
    {
        score = 0;
        OnHighscoreChanged?.Invoke(highscore);
    }

    public void AddScore()
    {
        score += 100;
        OnScoreChanged?.Invoke(score);
    }

    public bool TrySetNewHighscore()
    {
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            OnHighscoreChanged?.Invoke(score);
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }
}
