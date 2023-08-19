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
        [SerializeField] private GamePlayHandler playHandler;

        private GameState m_gameState;
        private GameData m_gameData;
        private DataSaveLoader dataSaveLoader;

        #endregion

        #region Methods

        #region Mono Methods

        private void Start()
        {
            if(gameConfig == null)
            {
                Debug.LogError("GameConfig is  not available, please check the refrence!");
            }
            else
            {
                UpdateState(GameState.Initialize);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the game state to specified one, and carried out the funtionalities of that state
        /// </summary>
        /// <param name="_gameState"></param>
        private void UpdateState(GameState _gameState)
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
                    StartGame();
                    break;
            }

        }

        /// <summary>
        /// Load the saved data from memory and setup the game
        /// </summary>
        private void InitializeTheGame()
        {
            //Play bg music at start
            audioManager.Initiaze();
            audioManager.PlayBgMusic(gameConfig.Audios.BgMusic);

            //Intialize game data
            m_gameData = new GameData();

            //REgister all event listners
            menuHandler.LevelSelected += OnGameLevelSelected;
            menuHandler.PlaySfxAudio += PlaySfx;
            menuHandler.ContinuePreviousButton += ContinueProressSelected;
            playHandler.PlaySfxAudio += PlaySfx;
            playHandler.SaveGameData += SaveGameData;
            playHandler.GoBackToMenu += EndGameplayState;
            //Try to fetch the local data from memory
            //If the data  is not avilable, then initialize a new load/save class
            dataSaveLoader = new DataSaveLoader();
            string data = dataSaveLoader.LoadData();
            if (string.IsNullOrEmpty(data) == false)
            {
                m_gameData = JsonUtility.FromJson<GameData>(data);
            }
            
            UpdateState(GameState.MainMenu);
        }        

        /// <summary>
        /// Display mein Menu screen
        /// </summary>
        private void ShowMainMenu()
        {
            menuHandler.Initialize(gameConfig.Levels,m_gameData.isPreviousGameInProgress);
        }

        /// <summary>
        /// Invokes when a level is selected.
        /// </summary>
        /// <param name="_levelIndex"></param>
        private void OnGameLevelSelected(int _levelIndex)
        {
            //Set level index
            m_gameData.levelIndex = _levelIndex;
            m_gameData.isPreviousGameInProgress = false;
            SaveGameData(m_gameData);
            audioManager.PlaySfx(gameConfig.Audios.ButtonClick);
            //Move to next state
            UpdateState(GameState.GamePlay);
        }

        /// <summary>
        /// Handles the user resposnse when the continues user progress on restart popup will be handled here
        /// </summary>
        /// <param name="_previousGame"></param>
        private void ContinueProressSelected(bool _previousGame)
        {
            if(_previousGame)
            {
                UpdateState(GameState.GamePlay);
            }
            else
            {
                m_gameData.isPreviousGameInProgress = false;
                SaveGameData(m_gameData);
            }
        }

        /// <summary>
        /// Start The Game
        /// </summary>
        private void StartGame()
        {
            var levelData = gameConfig.Levels[m_gameData.levelIndex];
            if(m_gameData.isPreviousGameInProgress)
            {
                playHandler.Initialiaze(m_gameData, levelData, gameConfig.Themes[m_gameData.themeIndex].CardSpeites);
            }
            else
            {
                playHandler.Initialiaze(levelData,gameConfig.Themes[m_gameData.themeIndex].CardSpeites);
            }
        }

        /// <summary>
        /// PLay given SFX audio
        /// </summary>
        /// <param name="_sfxType">The audio type need to be played</param>
        private void PlaySfx(SfxType _sfxType)
        {
            switch(_sfxType)
            {
                case SfxType.ButtonClick:
                    audioManager.PlaySfx(gameConfig.Audios.ButtonClick);
                    break;
                case SfxType.CardFlip:
                    audioManager.PlaySfx(gameConfig.Audios.CardFlip);
                    break;
                case SfxType.CardPaired:
                    audioManager.PlaySfx(gameConfig.Audios.CombinationSucess);
                    break;
                case SfxType.CardNotPAired:
                    audioManager.PlaySfx(gameConfig.Audios.CombinationError);
                    break;
                case SfxType.GameWin:
                    audioManager.PlaySfx(gameConfig.Audios.GameWin);
                    break;
            }
        }

        /// <summary>
        /// Save the data in json file
        /// </summary>
        /// <param name="_gamedata"></param>
        private void SaveGameData(GameData _gamedata)
        {
            //Update the level index from from local game data to the passed gamedata object
            _gamedata.levelIndex = m_gameData.levelIndex;
            string data = JsonUtility.ToJson(_gamedata);
            dataSaveLoader.SaveData(data);
        }

        private void EndGameplayState()
        {
            //No thing to do here
        }

        #endregion

        #endregion




    }
}
