using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card.Data
{
    /// <summary>
    /// Keep all level specific related data
    /// Handles difficuly level will be based on this level data
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelDataSO : ScriptableObject
    {
        [SerializeField] private string levelName;
        public string LevelName { get { return levelName; } }

        [SerializeField] private Vector2 gridSize;
        public Vector2 GridSize { get { return gridSize; } }

        [SerializeField] private List<int> emptyCellIndexes;
        public List<int> EmptyCellIndexes { get { return emptyCellIndexes; } }

        #region Methods

        public int GetTotalNumberOfCards()
        {
            int totalNumberOfCards = (int)(gridSize.x * gridSize.y);
            return totalNumberOfCards;
        }

        #endregion

    }
}
