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
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour 
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake() 
    {
        GameHandler.Instance.OnPauseGame += OnPauseGameEvent;
        Hide();
    }
    private void OnDestroy()
    {
        GameHandler.Instance.OnPauseGame -= OnPauseGameEvent;
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(OnResumeButtonClick);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
    }

    private void OnResumeButtonClick()
    {
        GameHandler.Instance.ResumeGame();
    }

    private void OnMainMenuButtonClick()
    {
        GameHandler.Instance.GoToMainMenu();
    }

    private void OnPauseGameEvent(bool paused)
    {
        gameObject.SetActive(paused);
    }

    private void Hide() 
    {
        gameObject.SetActive(false);
    }
}
