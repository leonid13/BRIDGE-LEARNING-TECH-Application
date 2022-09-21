using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameEvent _restartGame, _newLevel;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private int _maxLevel;
    [SerializeField] private GameObject _levelCanvas;
    [SerializeField] private GameObject _endGameCanvas;
    [SerializeField] private TextMeshProUGUI _winGameText;
    [SerializeField] private TextMeshProUGUI _loseGameText;
    [SerializeField] private TextMeshProUGUI _currentLevelText;

    private int _currentLevel;
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        private set { }
    }

    private void Awake()
    {
        _levelCanvas.SetActive(true);
        _endGameCanvas.SetActive(false);
        _winGameText.gameObject.SetActive(false);
        _loseGameText.gameObject.SetActive(false);
        _currentLevelText.text = (CurrentLevel + 1).ToString();
    }

    public void NextLevel()
    {
        _currentLevel++;
        if (_currentLevel > _maxLevel)
        {
            _currentLevel = _maxLevel;
        }
        else
        {
            _newLevel.Raise();
        }

        _currentLevelText.text = (CurrentLevel + 1).ToString();
    }

    public void RestartTheGame()
    {
        Time.timeScale = 1;

        _levelCanvas.SetActive(true);
        _endGameCanvas.SetActive(false);
        _winGameText.gameObject.SetActive(false);
        _loseGameText.gameObject.SetActive(false);
        _currentLevel = 0;
        _currentLevelText.text = (CurrentLevel + 1).ToString();

        _restartGame.Raise();
    }

    public void WinTheGame()
    {
        Time.timeScale = 0;
        _winGameText.gameObject.SetActive(true);
        SaveGameStats();
    }

    public void LoseTheGame()
    {
        Time.timeScale = 0;
        _loseGameText.gameObject.SetActive(true);
        SaveGameStats();
    }

    private void SaveGameStats()
    {
        _levelCanvas.SetActive(false);
        _endGameCanvas.SetActive(true);

        SaveData saveData = new SaveData();
        saveData.timeOfAttempt = System.DateTime.Now.ToString();
        saveData.score = _scoreManager.CurrentGameScore;
        saveData.score = _scoreManager.NumberOfPushedObjects;

        string save = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", save);

        // file is saved to C:\Users\username\AppData\LocalLow\DefaultCompany\BRIDGE LEARNING TECH Application
    }

    [System.Serializable]
    public class SaveData
    {
        public string timeOfAttempt;
        public float score;
        public float numberOfPushedObjects;
    }
}
