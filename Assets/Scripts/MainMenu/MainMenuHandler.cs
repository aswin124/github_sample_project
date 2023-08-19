using Card.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{   
    /// <summary>
    /// Handles mainMenu
    /// </summary>
    public class MainMenuHandler : MonoBehaviour
    {
        #region Properties

        [SerializeField] private GameObject levelItemPrefab;
        [SerializeField] private Transform levelItemParent;
        [SerializeField] private GameObject previousGamePopUP;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button continueButton;


        private List<LevelDataSO> m_levelDataList;
        private List<LevelItem> m_allLevelItems;
        private bool m_isInitialized;
        private bool m_isPreviousGameInProgress;

        public Action<int> LevelSelected;
        public Action<bool> ContinuePreviousButton;
        public Action<SfxType> PlaySfxAudio;

        #endregion

        #region Methods

        #region public Methods

        /// <summary>
        /// Itnitialize the Main menu
        /// </summary>
        /// <param name="_levelDataList"></param>
        /// <param name="_isPreviousGameInProgress"></param>
        public void Initialize(List<LevelDataSO> _levelDataList, bool _isPreviousGameInProgress)
        {
            if (m_isInitialized)
            {
                //Menu is clready created moeve to next state
                UpdateState(MainMenuState.Idle);
            }
            else
            {
                //Since we are not unloading the scene, we dont need to set the UI every time
                if (_levelDataList == null || _levelDataList.Count == 0)
                {
                    Debug.LogError("Error : Level Data is empty, check the game config SO!");
                }
                else
                {
                    m_levelDataList = _levelDataList;
                    m_isPreviousGameInProgress = _isPreviousGameInProgress;
                    UpdateState(MainMenuState.Initialize);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the menu states
        /// </summary>
        /// <param name="_state"></param>
        private void UpdateState(MainMenuState _state)
        {
            switch(_state)
            {
                case MainMenuState.Initialize:
                        InitiazlizeLevelSelectionUI();
                    break;
                case MainMenuState.PreviousGamePopUp:
                    ShowPreviousGamePopUp();
                    break;
                case MainMenuState.Idle:
                    //Do  nothing, wait for the player to make selection.
                    break;
            }
        }
        /// <summary>
        /// Creates level selection UI asper the avilable levels SO
        /// </summary>
        private void InitiazlizeLevelSelectionUI()
        {
            //Check whether the list is empty or not, if it is not empty, 
            //Then destoy all the objects and clear the list.
            if(m_allLevelItems != null && m_allLevelItems.Count>0)
            {
                for (int i = 0; i < m_allLevelItems.Count; i++)
                {
                    Destroy(m_allLevelItems[i].gameObject);
                }
                m_allLevelItems.Clear();
            }
            else
            {
                m_allLevelItems = new List<LevelItem>();
            }

            //Create level item prefabs in UI
            for (int i = 0; i < m_levelDataList.Count; i++)
            {
                LevelItem levelItem = Instantiate(levelItemPrefab, levelItemParent).GetComponent<LevelItem>();
                levelItem.gameObject.name = m_levelDataList[i].LevelName;
                levelItem.Initialize(i, m_levelDataList[i].LevelName, OnLevelSelected);
                m_allLevelItems.Add(levelItem);
            }

            m_isInitialized = true;
            UpdateState(MainMenuState.PreviousGamePopUp);
        }

        /// <summary>
        /// Invokes when player makes level selection;
        /// </summary>
        /// <param name="_levelIndex"></param>
        private void OnLevelSelected(int _levelIndex)
        {
            LevelSelected?.Invoke(_levelIndex);
        }

        /// <summary>
        /// Shows previous game popup to user for continue progress
        /// </summary>
        private void ShowPreviousGamePopUp()
        {
            if(m_isPreviousGameInProgress)
            {
                //Show Popup
                previousGamePopUP.SetActive(true);

                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(OnClickMenuButton);

                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(OnClickContinueButton);
            }
            else
            {
                previousGamePopUP.SetActive(false);
                UpdateState(MainMenuState.Idle);
            }
        }

        private void OnClickMenuButton()
        {
            previousGamePopUP.SetActive(false);

            //Play Audio
            PlaySfxAudio?.Invoke(SfxType.ButtonClick);
            ContinuePreviousButton?.Invoke(false);
        }

        private void OnClickContinueButton()
        {
            previousGamePopUP.SetActive(false);
            //Play Audio
            PlaySfxAudio?.Invoke(SfxType.ButtonClick);
            ContinuePreviousButton?.Invoke(true);
        }

        #endregion

        #endregion
    }
}
