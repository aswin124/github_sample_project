using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Card.Data;
using System.Linq;
using System;
using UnityEngine.UI;

namespace Card
{
    /// <summary>
    /// Handles the game play
    /// </summary>
    public class GamePlayHandler : MonoBehaviour
    {
        #region Properties

        [SerializeField] private GameObject cardItemPrefab;
        [SerializeField] private Transform cardItemParent;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI timerText;

        private GamePlayState m_currentState;
        private LevelDataSO m_levelData;
        private List<Sprite> m_cardImageList;
        private GameData m_gameData;
        private List<int> m_flippedCardIdex;
        private List<CardItem> m_allCardItems;

        public Action<SfxType> PlaySfxAudio;
        public Action<GameData> SaveGameData;

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Initiallizes the game play with last game progress
        /// </summary>
        /// <param name="_gameData">last game progress data</param>
        /// <param name="_cardImageList">card images</param>
        public void Initialiaze(GameData _gameData, List<Sprite> _cardImageList)
        {
            this.gameObject.SetActive(true);
            m_gameData = _gameData;
            m_cardImageList = _cardImageList;
            scoreText.text = "";
            turnText.text = "";
            timerText.text = "";
            UpdateState(GamePlayState.OldGame);
        }

        /// <summary>
        /// Set up a new game
        /// </summary>
        /// <param name="_levelData"></param>
        /// <param name="_cardImageList"></param>
        public void Initialiaze(LevelDataSO _levelData, List<Sprite> _cardImageList)
        {
            this.gameObject.SetActive(true);
            m_levelData = _levelData;
            m_cardImageList = _cardImageList;
            scoreText.text = "Score : 0";
            turnText.text = "Turns : 0";
            timerText.text = "";
            UpdateState(GamePlayState.NewGame);
        }

        #endregion

        /// <summary>
        /// Execute specifiesd states
        /// </summary>
        /// <param name="_state"></param>
        public void UpdateState(GamePlayState _state)
        {
            m_currentState = _state;
            switch (_state)
            {
                case GamePlayState.NewGame:
                    GenerateGameData();
                    break;

                case GamePlayState.OldGame:
                    LoadOldGame();
                    break;

                case GamePlayState.StupUI:
                    SetUpUI();
                    break;

                case GamePlayState.CardReveal:
                    StartCoroutine(RevealCards());
                    break;

                case GamePlayState.Playing:
                    //Nothing to do here, keep a  track of current state
                    break;

                case GamePlayState.GameResult:
                    ShowGameResult();
                    break;

            }
        }

        private void ResetAll()
        {
            m_flippedCardIdex = new List<int>();
            if(m_allCardItems == null)
            {
                m_allCardItems = new List<CardItem>();
            }
            if(m_allCardItems.Count>0)
            {
                for (int i = 0; i < m_allCardItems.Count; i++)
                {
                    Destroy(m_allCardItems[i].gameObject);
                }
                m_allCardItems.Clear();
            }
        }

        #region New game state related methods

        /// <summary>
        /// Generate game data for new game
        /// </summary>
        private void GenerateGameData()
        {
            ResetAll();
            m_gameData = new GameData();
            int totalNumberOfCards = m_levelData.GetTotalNumberOfCards();
            int totalNumberOfEmptyCards = m_levelData.EmptyCellIndexes.Count;
            //each image will will be placed on 2 cards
            int numberofImageRequired = (totalNumberOfCards - totalNumberOfEmptyCards) / 2;
            List<int> randomImageList = GetRandomImageIndexList(numberofImageRequired);
            //Now we have random images for the cards, we need to set  these images on random cards now
            //Genrate cardIndex list of avialable card indexes
            List<int> randomCardIndexList = GetRandomCardIndexList(totalNumberOfCards);
            int cardIndex = 0;
            for (int i = 0; i < randomImageList.Count; i++)
            {
                //We need to add 2random card indexes for each image
                for (int j = cardIndex; j < cardIndex + 2; j++)
                {
                    CardStatus cardStatus = new CardStatus();
                    cardStatus.cardIndex = randomCardIndexList[j];
                    cardStatus.isEmpty = false;
                    cardStatus.imageIndex = randomImageList[i];
                    cardStatus.isPaired = false;
                    m_gameData.allCardStatus.Add(cardStatus);
                }
                cardIndex += 2;
            }
            //Add empty Cards
            for (int i = 0; i < m_levelData.EmptyCellIndexes.Count; i++)
            {
                CardStatus cardStatus = new CardStatus();
                cardStatus.cardIndex = m_levelData.EmptyCellIndexes[i];
                cardStatus.isEmpty = true;
                cardStatus.imageIndex = 0;
                cardStatus.isPaired = true;
                m_gameData.allCardStatus.Add(cardStatus);
            }
            //Now sort the card based on cardIndex
            m_gameData.allCardStatus = m_gameData.allCardStatus.OrderBy(x => x.cardIndex).ToList();
            //Todo need to save the data in memomory
            //Move to next state
            UpdateState(GamePlayState.StupUI);

        }

        /// <summary>
        /// Return random card image index list
        /// </summary>
        /// <param name="_numberofImageRequired"></param>
        /// <returns></returns>
        private List<int> GetRandomImageIndexList(int _numberofImageRequired)
        {
            List<int> imageIndexList = new List<int>();
            int totalNumberOfAvailableIndex = m_cardImageList.Count;
            //Create a list with the total aviable image count
            List<int> availableIdexes = new List<int>();
            for (int i = 0; i < totalNumberOfAvailableIndex; i++)
            {
                availableIdexes.Add(i);
            }
            //Now we nee to update the random index to imageIndexList 
            int randomIdex;
            while (availableIdexes.Count > 0)
            {
                randomIdex = UnityEngine.Random.Range(0, availableIdexes.Count);
                imageIndexList.Add(availableIdexes[randomIdex]);
                //Remove the added item from the list
                availableIdexes.Remove(availableIdexes[randomIdex]);
                if(imageIndexList.Count  >= _numberofImageRequired)
                {
                    return imageIndexList;
                }
                else if(availableIdexes.Count ==0)
                {
                    //This is to add duplicate images.
                    //For example we have a 4x4 grid, total number of image required are 8
                    //let say we have only 4 images available, in this case, in the first list will add all 4 images
                    //then the list will be empty, we need to refill the list untill we get the required number of images
                    for (int i = 0; i < totalNumberOfAvailableIndex; i++)
                    {
                        availableIdexes.Add(i);
                    }
                }
            }

            //Todo Need to update Random number generation based on game behaviour
            return imageIndexList;
        }

        /// <summary>
        ///  return random card index list
        /// </summary>
        /// <param name="_totalNumberOfCards"></param>
        /// <returns></returns>
        private List<int> GetRandomCardIndexList(int _totalNumberOfCards)
        {
            List<int> cardIndexList = new List<int>();
            //Create  list of card indexes
            List<int> tempCardIndexes = new List<int>();
            for (int i = 0; i < _totalNumberOfCards; i++)
            {
                tempCardIndexes.Add(i);
            }
            int randomIdex;
            //Randomise the order of card indexes in the list
            while (tempCardIndexes.Count > 0)
            {
                randomIdex = UnityEngine.Random.Range(0, tempCardIndexes.Count);
                //If its an empty card we dont need to add image to it.
                if (m_levelData.EmptyCellIndexes.Contains(tempCardIndexes[randomIdex]) == false)
                {
                    cardIndexList.Add(tempCardIndexes[randomIdex]);
                }
                //Remove the added item from the list
                tempCardIndexes.Remove(tempCardIndexes[randomIdex]);
            }
            return cardIndexList;
        }
        #endregion

        #region Old Game State Related Method

        private void LoadOldGame()
        {
            ResetAll();
            //We will get game data from saved data, so no need to generate it.
            //state is created for future use
            UpdateState(GamePlayState.StupUI);
        }

        #endregion

        #region SetUp UI state related methods
        /// <summary>
        /// Generate cards in the grid and assign card images
        /// </summary>
        private void SetUpUI()
        {
            //Set Grid layout
            gridLayoutGroup.cellSize = m_levelData.CellSize;
            gridLayoutGroup.constraintCount = m_levelData.FixedColumnCount;
            //Instaantiate cards
            for (int i = 0; i < m_gameData.allCardStatus.Count; i++)
            {
                CardItem cardItem = Instantiate(cardItemPrefab, cardItemParent).GetComponent<CardItem>();
                Sprite cardImage = m_cardImageList[m_gameData.allCardStatus[i].imageIndex];
                cardItem.Initialize(m_gameData.allCardStatus[i], cardImage, OnCardClicked, PlayCardFlipSfx);
                m_allCardItems.Add(cardItem);
            }
            UpdateState(GamePlayState.CardReveal);
        }

        private void PlayCardFlipSfx()
        {
            PlaySfxAudio?.Invoke(SfxType.CardFlip);
        }

        private void OnCardClicked(int _cardIndex)
        {
            m_flippedCardIdex.Add(_cardIndex);
            if (m_flippedCardIdex.Count >= 2)
            {
                PairCards(m_flippedCardIdex[0], m_flippedCardIdex[1]);
                m_flippedCardIdex.RemoveRange(0, 2);
            }
        }
        private void PairCards(int _cardIndex1, int _cardIndex2)
        {
            m_gameData.noOfTurns++;
            if (m_gameData.allCardStatus[_cardIndex1].imageIndex == m_gameData.allCardStatus[_cardIndex2].imageIndex)
            {
                //Card Successfully paired
                //Play paired Audio
                m_gameData.allCardStatus[_cardIndex1].isPaired = true;
                m_gameData.allCardStatus[_cardIndex2].isPaired = true;
                m_gameData.score++;
                //Save the data in local
                SaveGameData?.Invoke(m_gameData);
                PlaySfxAudio?.Invoke(SfxType.CardPaired);
                m_allCardItems[_cardIndex1].CardPairStatus(true);
                m_allCardItems[_cardIndex2].CardPairStatus(true);

                UpdateUI();
                //Check if all the cards are paired
                var cardStatusList = m_gameData.allCardStatus.Find(x => x.isPaired == false);
                if(cardStatusList == null)
                {
                    //Allm cards  are paired.
                    UpdateState(GamePlayState.GameResult);
                }
            }
            else
            {
                PlaySfxAudio?.Invoke(SfxType.CardNotPAired);
                m_allCardItems[_cardIndex1].CardPairStatus(false);
                m_allCardItems[_cardIndex2].CardPairStatus(false);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            scoreText.text = "Score : " + m_gameData.score.ToString();
            turnText.text = "Tutns : "+ m_gameData.noOfTurns.ToString();
        }

        #endregion

        IEnumerator RevealCards()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < m_allCardItems.Count; i++)
            {
                m_allCardItems[i].RevealCard();
            }
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < m_allCardItems.Count; i++)
            {
                m_allCardItems[i].HideCard();
            }
            UpdateState(GamePlayState.Playing);
        }

        private void ShowGameResult()
        {
            
        }

        #endregion

    }
}
