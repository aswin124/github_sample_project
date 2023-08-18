using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// Carries all the relevent data for the data, the same class will be saved in local to keep track 
    /// of user game progress.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public int levelIndex;
        public int themeIndex;
        public List<CardStatus> allCardStatus;
        public int score;
        public int noOfTurns;
        public int timeTaken;
        public bool isPreviousGameInProgress;
        public int comboPairingCount;

        public GameData()
        {
            levelIndex = 0;
            allCardStatus = new List<CardStatus>();
            score = 0;
            noOfTurns = 0;
            timeTaken = 0;
            themeIndex = 0;
            isPreviousGameInProgress = false;
            comboPairingCount = 0;
        }
    }

    /// <summary>
    /// Hold the card data for recovery
    /// </summary>
    [Serializable]
    public class CardStatus
    {
        public int cardIndex;
        public bool isEmpty;
        public int imageIndex;
        public bool isPaired;

        public CardStatus()
        {
            cardIndex = 0;
            isEmpty = false;
            imageIndex = 0;
            isPaired = false;
        }
    }
}
