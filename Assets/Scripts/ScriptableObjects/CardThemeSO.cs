using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card.Data
{
    /// <summary>
    /// Carries all the card icons
    /// </summary>
    [CreateAssetMenu(fileName = "CardTheme", menuName = "ScriptableObjects/CardTheme")]
    public class CardThemeSO : ScriptableObject
    {
        [SerializeField] private string themeName;
        public string ThemeName => themeName;
        [SerializeField] private List<Sprite> cardSprites;
        public List<Sprite> CardSpeites => cardSprites;
    }
}
