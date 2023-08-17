using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card.Data
{
    /// <summary>
    /// Carries all the audio used in the game
    /// </summary>
    [CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private AudioClip cardFlip;
        public AudioClip CardFlip => cardFlip;

        [SerializeField] private AudioClip combinationSucess;
        public AudioClip CombinationSucess => combinationSucess;

        [SerializeField] private AudioClip combinationError;
        public AudioClip CombinationError => combinationError;

        [SerializeField] private AudioClip gameWin;
        public AudioClip GameWin => gameWin;

        [SerializeField] private AudioClip bgMusic;
        public AudioClip BgMusic => bgMusic;

        [SerializeField] private AudioClip buttonClick;
        public AudioClip ButtonClick => buttonClick;
    }
}
