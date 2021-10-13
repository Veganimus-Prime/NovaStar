using System;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Veganimus.NovaStar
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private int _playerScore;
        [SerializeField] private AudioClip _pauseSound;
        [SerializeField] private AudioClip _gameOverSound;
        [SerializeField] private Canvas _hUD_Canvas, pauseMenu, levelSummary, gameOverScreen, endOfGameScreen;
        [Header("Static UI")]

        [Header("Dynamic UI")]
        [SerializeField] private GameObject _incomingWaveTextGO, _powerUpTimer, _bossHealthUI, _shieldImage;
        [SerializeField] private Image _bossHealthBar;
        [SerializeField] private TMP_Text _levelText, _scoreText, _weaponText, _incomingWaveText;
        [Header("Listening To")]
        [SerializeField] private BoolEventSO _shieldUIEvent;
        [SerializeField] private CoRoutineEvent _startCoolDownTimer;
        [SerializeField] private GameEvent _trackBossWave;
        [SerializeField] private intEventSO _updateScoreChannel;
        [SerializeField] private intEventSO _trackWaveEvent;
        [SerializeField] private intEventSO _bossHealthUIEvent;
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private PlayerWeaponEvent _playerWeaponEvent;
        [SerializeField] private TrackLevelEventSO _trackLevelEvent;
        [Header("Broadcasting On")]
        [SerializeField] private LoadSceneEventSO _loadSceneEvent;
        [SerializeField] private PlaySFXEvent _playSFXEvent;

        private bool  _bossWave, _gamePaused;
        private int _currentWave, _currentLevel, _bossMaxHealth = 100, _bossCurrentHealth = 100;
        public static Action<Canvas> continueButton;

        private void OnEnable()
        {
            _shieldUIEvent.OnBoolEventRaised += UpdateShieldUI;
            _startCoolDownTimer.OnRoutineStart += StartTimer;
            _trackBossWave.OnEventRaised += TrackBossWave;
            _updateScoreChannel.OnEventRaised += UpdateScore;
            _trackWaveEvent.OnEventRaised += TrackWave;
            _bossHealthUIEvent.OnEventRaised += UpdateBossHealth;
            _inputReader.pauseEvent += OnPauseInput;
            _playerWeaponEvent.OnPlayerWeaponNameEventRaised += UpdateWeaponName;
            _trackLevelEvent.OnEventRaised += TrackLevel;
            GameManager.gameOver += GameOver;
        }
        private void OnDisable()
        {
            _shieldUIEvent.OnBoolEventRaised -= UpdateShieldUI;
            _startCoolDownTimer.OnRoutineStart -= StartTimer;
            _trackBossWave.OnEventRaised -= TrackBossWave;
            _updateScoreChannel.OnEventRaised -= UpdateScore;
            _trackWaveEvent.OnEventRaised -= TrackWave;
            _bossHealthUIEvent.OnEventRaised -= UpdateBossHealth;
            _inputReader.pauseEvent -= OnPauseInput;
            _playerWeaponEvent.OnPlayerWeaponNameEventRaised -= UpdateWeaponName;
            _trackLevelEvent.OnEventRaised -= TrackLevel;
            GameManager.gameOver -= GameOver;
        }
        
        private void Start()
        {
            _playerScore = 0;
            UpdateScore(0);
        }

        private void ContinueButton(Canvas activeCanvas)
        {
            activeCanvas.gameObject.SetActive(false);
            continueButton(activeCanvas);
        }

        private void GameOver(bool isOver)
        {
            _playSFXEvent.RaiseSFXEvent(_gameOverSound);
            gameOverScreen.gameObject.SetActive(isOver);
            _hUD_Canvas.gameObject.SetActive(false);
            _playerScore = 0;
            UpdateScore(0);
        }
        private void OnPauseInput()
        {
            _playSFXEvent.RaiseSFXEvent(_pauseSound);
            switch (_gamePaused)
            {
                case true:
                    pauseMenu.gameObject.SetActive(false);
                    _gamePaused = false;
                    Time.timeScale = 1;
                    break;
                case false:
                    pauseMenu.gameObject.SetActive(true);
                    _gamePaused = true;
                    Time.timeScale = 0;
                    break;
            }
        }
        private void QuitGame()
        {
#if UNITY_STANDALONE

            Application.Quit();
#endif
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif

        }

        private void RestartGame() => _loadSceneEvent.RaiseEvent("Main_Menu");

        private void StartTimer() => _powerUpTimer.gameObject.SetActive(true);

        private void TrackBossWave()
        {
            _levelText.text = $"Level: {_currentLevel} - Boss";
            _bossWave = true;
            _bossHealthUI.SetActive(true);
            StartCoroutine(IncomingWaveText());
        }

        private void TrackLevel(int level)
        {
            _currentLevel = level;
            _levelText.text = $"Level:  { _currentLevel } -  {_currentWave}";
        }

        private void TrackWave(int wave)
        {
            _currentWave = wave;
            _levelText.text = $"Level:  { _currentLevel} - { _currentWave}";
            StartCoroutine(IncomingWaveText());
        }

        private void UpdateBossHealth(int bossHealth)
        {
            _bossCurrentHealth = bossHealth;
            float normalizedValue = Mathf.Clamp((float)_bossCurrentHealth / (float)_bossMaxHealth, 0.0f, 1.0f);
            _bossHealthBar.fillAmount = normalizedValue;
        }

        private void UpdateScore(int amount)
        {
            _playerScore += amount;
            _scoreText.text = $"Score: { _playerScore}";
        }

        private void UpdateShieldUI(bool shieldOn) => _shieldImage.SetActive(shieldOn);

        private void UpdateWeaponName(string name)=> _weaponText.text = $"Weapon: {name}";

        private IEnumerator IncomingWaveText()
        {
            if(!_bossWave)
             _incomingWaveText.text = $"Wave: {_currentWave} Incoming!";
            
            else if(_bossWave)
                _incomingWaveText.text = $"Enemy Boss Incoming!";
           
            _incomingWaveTextGO.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            _incomingWaveTextGO.SetActive(false);
        }
    }
}
