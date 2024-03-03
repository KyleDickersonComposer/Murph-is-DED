using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace Game.Core
{
    public enum GamePauseStatus
    {
        Paused,
        UnPaused
    }

    public enum GameplayStatus
    {
        None,
        OnGoing,
        End
    }
    public class GameManager : GenericSingleton<GameManager>
    {

        public UnityEvent OnSceneStart;
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;

        public UnityEvent OnGameplayStart;
        public UnityEvent OnGameEnd;
        public UnityEvent OnBackToMain;

        private GamePauseStatus _gamePauseStatus = GamePauseStatus.UnPaused;

        private GameplayStatus _gameplayStatus = GameplayStatus.None;

        public GamePauseStatus GamePauseStatus { get=> _gamePauseStatus; }
        public GameplayStatus GameplayStatus { get=> _gameplayStatus;}

        private void PauseGame()
        {
            Time.timeScale = 0.0f;
            _gamePauseStatus = GamePauseStatus.Paused;
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1.0f;
            _gamePauseStatus = GamePauseStatus.UnPaused;
        }

        private void StartGame()
        {
            _gameplayStatus = GameplayStatus.OnGoing;
        }

        private void EndGame()
        {
            _gameplayStatus = GameplayStatus.End;
        }

        private void ExitGameplay()
        {
            _gameplayStatus = GameplayStatus.None;
        }
        private void Start() 
        {
            OnGamePaused.AddListener(PauseGame);
            OnGameResumed.AddListener(UnpauseGame);    
            OnGameplayStart.AddListener(StartGame);
            OnGameEnd.AddListener(EndGame);
            OnBackToMain.AddListener(ExitGameplay);
        }
        
    }

}
