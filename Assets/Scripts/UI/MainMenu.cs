using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _quitButton;

    [Space(4)]
    [SerializeField]
    private GameObject _mainMenuPanel;
    [SerializeField]
    private SettingsMenu _settings;

    void Awake()
    {
        _playButton.onClick.AddListener(StartGame);
        _settingsButton.onClick.AddListener(ShowOptions);
        _quitButton.onClick.AddListener(() => Application.Quit());
        _settings.SettingsSaved += HideOptions;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void ShowOptions()
    {
        _mainMenuPanel.SetActive(false);
        _settings.gameObject.SetActive(true);
    }

    private void HideOptions()
    {
        _mainMenuPanel.SetActive(true);
        _settings.gameObject.SetActive(false);
    }
}
