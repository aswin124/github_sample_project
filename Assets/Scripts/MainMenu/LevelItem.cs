using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    public class LevelItem : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI levelName;

        private int m_levelIndex;
        private Action<int> m_onLevelSelected;

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// Initilizes the level item.
        /// </summary>
        /// <param name="_levelIdex"></param>
        /// <param name="_levelName"></param>
        public void Initialize(int _levelIdex, string _levelName, Action<int> _onLevelSelected)
        {
            //Check whether the properties are assigned from inspecter, if not then fetch it.
            if(button ==null)
            {
                button = this.GetComponentInChildren<Button>();
            }
            if(levelName == null)
            {
                levelName = this.GetComponentInChildren<TextMeshProUGUI>();
            }

            m_levelIndex = _levelIdex;
            m_onLevelSelected = _onLevelSelected;
            levelName.text = _levelName;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnLevelSelectd);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked when a player select a level
        /// </summary>
        private void OnLevelSelectd()
        {
            //Call an event and pass the level idex
            m_onLevelSelected?.Invoke(m_levelIndex);
        }

        #endregion

        #endregion
    }
}
