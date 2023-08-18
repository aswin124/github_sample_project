using Card.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// Manages all the game states and works as a bridge between other calsses.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Properties

        [SerializeField] private GameConfigSO gameConfig;

        [SerializeField] private MainMenuHandler menuHandler;
        [SerializeField] private AudioManager audioManager;

        private GameState m_gameState;

        #endregion

        #region Methods

        #region Mono Methods

        private void Start()
        {
            if(gameConfig == null)
            {
                Debug.LogError("GameConfig is  not avvilable, please check the refrence!");
            }
            else
            {
                UpdateState(GameState.Initialize);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the game state to specified one, and carried out the funtionalities of that state
        /// </summary>
        /// <param name="_gameState"></param>
        public void UpdateState(GameState _gameState)
        {
            m_gameState = _gameState;

            switch(m_gameState)
            {
                case GameState.Initialize:
                    InitializeTheGame();
                    break;
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameState.GamePlay:
                    break;
                case GameState.GameResult:
                    break;
            }

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Load the saved data from memory and setup the game
        /// </summary>
        private void InitializeTheGame()
        {
            //Play bg music at start
            audioManager.Initiaze();
            audioManager.PlayBgMusic(gameConfig.Audios.BgMusic);

            menuHandler.LevelSelected += OnGameLevelSelected;
            //Try to fetch the local data from memory
            //If the data  is not avilable, then initialize a new load/save class
            UpdateState(GameState.MainMenu);
        }


        /// <summary>
        /// Invokes when a level is selected.
        /// </summary>
        /// <param name="_levelIndex"></param>
        private void OnGameLevelSelected(int _levelIndex)
        {
            audioManager.PlaySfx(gameConfig.Audios.ButtonClick);
        }

        /// <summary>
        /// Display mein Menu screen
        /// </summary>
        private void ShowMainMenu()
        {
            menuHandler.Initialize(gameConfig.Levels);
        }

        /// <summary>
        /// Start The Game
        /// </summary>
        private void StartGame(string selectedLevel)
        {

        }

        /// <summary>
        /// Display game win screen
        /// </summary>
        private void ShowGameResultScreen()
        {

        }

        #endregion

        #endregion




    }
}
