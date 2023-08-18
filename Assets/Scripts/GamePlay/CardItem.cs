using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    /// <summary>
    /// Handles card based functionality
    /// </summary>
    public class CardItem : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Image cardItemImage;
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Animator animator;

        private CardStatus m_cardStatus;
        private Action<int> m_onCardSelected;
        private Action m_playCardFlipSfx;
        private bool m_isAnimating = false;

        #endregion

        #region Methods

        #region Public Methods

        public void Initialize(CardStatus _cardStatus, Sprite _cardSprite, Action<int> _onCardSelected, Action _playCardFlipSfx)
        {
            m_cardStatus = _cardStatus;
            m_onCardSelected = _onCardSelected;
            m_playCardFlipSfx = _playCardFlipSfx;
            //If the card is empty then set the alpha to 0 and do nothing.
            if (m_cardStatus.isEmpty || m_cardStatus.isPaired)
            {
                canvasGroup.alpha = 0;
                return;
            }
            canvasGroup.alpha = 1;
            cardItemImage.sprite = _cardSprite;
            // Add Button click CallBacks
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClicked);
        }

        public void RevealCard()
        {
            m_isAnimating = true;
            //play card flip anim
            animator.SetTrigger(GameConstants.FLIP_ANIM_TRIGGER_NAME);
        }

        public void HideCard()
        {
            StartCoroutine(PlayReverseFlipAnim());
        }

        public void CardPairStatus(bool _isPaired)
        {
            if(_isPaired)
            {
                //DEactivate the card from grid
                canvasGroup.alpha = 0;
                m_cardStatus.isPaired = true;
            }
            else
            {
                StartCoroutine(PlayReverseFlipAnim());
            }
        }



        #endregion

        #region Private Methods

        private void OnButtonClicked()
        {
            if(m_cardStatus.isEmpty || m_cardStatus.isPaired || m_isAnimating)
            {
                return;
            }
            m_playCardFlipSfx?.Invoke();
            StartCoroutine(PlayFlipAnim());
        }

        IEnumerator PlayFlipAnim()
        {
            //play card flip anim
            animator.SetTrigger(GameConstants.FLIP_ANIM_TRIGGER_NAME);
            m_isAnimating = true;
            yield return new WaitForSeconds(GameConstants.CARD_ANIM_DURATION);
            //Give a call back to parent class to handle the  pair condition
            m_onCardSelected?.Invoke(m_cardStatus.cardIndex);
            m_isAnimating = false;
        }

        IEnumerator PlayReverseFlipAnim()
        {
            m_isAnimating = true;
            //play card flip anim
            animator.SetTrigger(GameConstants.REVERSE_FLIP_ANIM_TRIGGER_NAME);
            m_isAnimating = true;
            yield return new WaitForSeconds(GameConstants.CARD_ANIM_DURATION);
            m_isAnimating = false;
        }

        #endregion

        #endregion
    }
}
