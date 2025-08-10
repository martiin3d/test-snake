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

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _howToPlayPanel;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _howToButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _backButton;

    private enum Panel 
    {
        Main,
        HowToPlay,
    }

    private void Awake() 
    {
        ShowPanel(Panel.Main);
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _howToButton.onClick.AddListener(OnHowToButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
        _howToButton.onClick.RemoveListener(OnHowToButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnPlayButtonClick()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    private void OnHowToButtonClick()
    {
        ShowPanel(Panel.HowToPlay);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void OnBackButtonClick()
    {
        ShowPanel(Panel.Main);
    }

    private void ShowPanel(Panel sub) 
    {
        _mainPanel.SetActive(sub == Panel.Main);
        _howToPlayPanel.SetActive(sub == Panel.HowToPlay);
    }
}
