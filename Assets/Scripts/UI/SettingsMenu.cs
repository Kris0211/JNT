using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
struct SettingsData
{
    public int requiredCoins;
    public int spawnInterval;
}

public class SettingsMenu : MonoBehaviour
{
    public event Action SettingsSaved;

    [Tooltip("ScriptableObject file which contains gameplay settings data.")]
    [SerializeField]
    private GameplaySettings _gameplaySettings;

    [Space(4)]
    [SerializeField]
    private Button _returnButton;

    [Space(4)]
    [SerializeField]
    private TMP_InputField _requiredCoinsInputField;
    [SerializeField]
    private TMP_InputField _spawnIntervalInputField;

    private SettingsData _settingsData;

    private string _persistentDataPath;

    void Awake()
    {
        _persistentDataPath = Path.Combine(Application.persistentDataPath, "settings.json");

        Assert.IsNotNull(_requiredCoinsInputField);
        _requiredCoinsInputField.onValueChanged.AddListener(UpdateRequiredCoins);

        Assert.IsNotNull(_spawnIntervalInputField);
        _spawnIntervalInputField.onValueChanged.AddListener(UpdateSpawnInterval);

        Assert.IsNotNull(_returnButton);
        _returnButton.onClick.AddListener(OnReturnButtonPressed);

        _settingsData = new();
    }

    void OnEnable()
    {
        LoadSettings();
    }

    private void UpdateRequiredCoins(string value)
    {
        if (int.TryParse(value, out int i))
        {
            _settingsData.requiredCoins = i;
        }
    }

    private void UpdateSpawnInterval(string value)
    {
        if (int.TryParse(value, out int i))
        {
            _settingsData.spawnInterval = i;
        }
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(_settingsData);
        using StreamWriter file = new(File.Open(_persistentDataPath, FileMode.OpenOrCreate));
        file.Write(json);

        if (_gameplaySettings != null)
        {
            _gameplaySettings.SetValues(_settingsData.requiredCoins, _settingsData.spawnInterval);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(_gameplaySettings);
            AssetDatabase.SaveAssets();
            #endif
        }
    }

    private void LoadSettings() {
        if (File.Exists(_persistentDataPath))
        {
            string json = File.ReadAllText(_persistentDataPath);
            SettingsData settings = JsonUtility.FromJson<SettingsData>(json);

            int requiredCoinsValue = settings.requiredCoins;
            int spawnIntervalValue = settings.spawnInterval;

            _settingsData.requiredCoins = requiredCoinsValue;
            _settingsData.spawnInterval = spawnIntervalValue;

            _requiredCoinsInputField.SetTextWithoutNotify(settings.requiredCoins.ToString());
            _spawnIntervalInputField.SetTextWithoutNotify(settings.spawnInterval.ToString());

        }
        else // Restore default values
        {
            _settingsData.requiredCoins = 5;
            _settingsData.spawnInterval = 3;
            _requiredCoinsInputField.SetTextWithoutNotify("5");
            _spawnIntervalInputField.SetTextWithoutNotify("3");
        }
    }

    private void OnReturnButtonPressed()
    {
        SaveSettings();
        SettingsSaved.Invoke();
    } 
}
