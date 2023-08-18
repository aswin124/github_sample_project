using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// Handle All the audio in the game
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Properties

        [SerializeField] private AudioSource bgMusicAudioSource;

        [SerializeField] private GameObject sfxAudioSourcePrefab;

        private List<AudioSource> m_sfxAudioSources;

        #endregion

        #region Methods

        #region  public Methods

        public void Initiaze()
        {
            m_sfxAudioSources = new List<AudioSource>();
            //Create one sfx audio source first and keep it in the list, later if required more,
            //then create new and add it into the list
            CreateSfxAudioSource();
        }

        public void PlayBgMusic(AudioClip _clip)
        {
            bgMusicAudioSource.clip = _clip;
            bgMusicAudioSource.Play();
        }

        public void PlaySfx(AudioClip _clip)
        {
            //Fetch a audio source from list and play it
            AudioSource audioSource = null;
            for (int i = 0; i < m_sfxAudioSources.Count; i++)
            {
                if(m_sfxAudioSources[i].isPlaying == false)
                {
                    audioSource = m_sfxAudioSources[i];
                    break;
                }
            }

            if(audioSource ==null)
            {
                CreateSfxAudioSource();
                audioSource = m_sfxAudioSources[m_sfxAudioSources.Count - 1];
            }

            audioSource.clip = _clip;
            audioSource.Play();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create new sfx audio source
        /// </summary>
        private void CreateSfxAudioSource()
        {
            if(m_sfxAudioSources == null)
            {
                m_sfxAudioSources = new List<AudioSource>();
            }    
            AudioSource audioSource = Instantiate(sfxAudioSourcePrefab, this.transform).GetComponent<AudioSource>();
            m_sfxAudioSources.Add(audioSource);
        }

        #endregion

        #endregion
    }
}
