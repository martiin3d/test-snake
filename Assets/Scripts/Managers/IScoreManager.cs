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

public interface IScoreManager
{
    int highscore { get; }

    event Action<int> OnHighscoreChanged;
    event Action<int> OnScoreChanged;
}