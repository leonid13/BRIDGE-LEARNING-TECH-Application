using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float[] _sphereScoreByLevel;
    [SerializeField] private float[] _capsuleScoreByLevel;
    [SerializeField] private float _levelEndScore;
    [SerializeField] private float _gameWinScore;

    [SerializeField] private TextMeshProUGUI _levelScoreText;
    [SerializeField] private TextMeshProUGUI _gameScoreText;

    private float _currentLevelScore;
    private float _currentGameScore;
    private int _numberOfPushedObjects;
    private Scoreable.ScoreableType _lastPushedType;

    public float CurrentGameScore
    {
        get
        {
            return _currentGameScore;
        }
        private set { }
    }

    public int NumberOfPushedObjects
    {
        get
        {
            return _numberOfPushedObjects;
        }
        private set { }
    }

    private void Awake()
    {
        UpdateUIValues();
    }

    public void NullLastPushedType()
    {
        _lastPushedType = Scoreable.ScoreableType.None;
    }

    public void ResetScoreManager()
    {
        _currentLevelScore = 0;
        _currentGameScore = 0;
        _numberOfPushedObjects = 0;
        _lastPushedType = Scoreable.ScoreableType.None;
        UpdateUIValues();
    }

    public void ChangeScore(Scoreable.ScoreableType type)
    {
        float scoreMultiplier = 1; 
        if (_lastPushedType == type) scoreMultiplier = -2; // decrease by double the value if the last pushed type is the same

        switch (type)
        {
            case Scoreable.ScoreableType.Sphere:
                _currentLevelScore += _sphereScoreByLevel[_levelManager.CurrentLevel] * scoreMultiplier;
                _currentGameScore += _sphereScoreByLevel[_levelManager.CurrentLevel] * scoreMultiplier;
                break;

            case Scoreable.ScoreableType.Capsule:
                _currentLevelScore += _capsuleScoreByLevel[_levelManager.CurrentLevel] * scoreMultiplier;
                _currentGameScore += _capsuleScoreByLevel[_levelManager.CurrentLevel] * scoreMultiplier;
                break;

            default:
                Debug.LogError("Not specified scoreable type!");
                break;
        }
        _lastPushedType = type;
        _numberOfPushedObjects++;

        UpdateUIValues();
        _spawner.SpawnNewObject(type);
        CheckWinGameConditions();
    }

    private void UpdateUIValues()
    {
        _levelScoreText.text = _currentLevelScore.ToString();
        _gameScoreText.text = _currentGameScore.ToString();
    }

    private void CheckWinGameConditions()
    {
        if(_currentLevelScore >= _levelEndScore)
        {
            _levelManager.NextLevel();

            _currentLevelScore -= _levelEndScore;
            _levelScoreText.text = _currentLevelScore.ToString();
        }

        if(_currentGameScore >= _gameWinScore)
        {
            _levelManager.WinTheGame();
        }
    }
}
