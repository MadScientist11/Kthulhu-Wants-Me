using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudioManager
{
    [CreateAssetMenu(fileName = "Song", menuName = "SimpleAudioManager/Song", order = 0)]
    public class Song : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// The time before the end of a clip where a new loop of the clip will begin
        /// </summary>
        public float reverbTail = 0f;

        /// <summary>
        /// The different clips representing different intensities of the same composition
        /// </summary>
        public List<AudioClip> intensityClips = new List<AudioClip>();

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Convert to song data
        /// </summary>
        public Data ToSongData() => new Data {
            reverbTail = reverbTail,
            intensityClips = new List<AudioClip>(intensityClips)
        };

        #endregion

        #region PUBLIC CLASSES

        /// <summary>
        /// The usable struct which contains the relevant information for the song
        /// </summary>
        public struct Data
        {
            public float reverbTail;
            public List<AudioClip> intensityClips;
        }

        #endregion
    }
}

/*
 * Written by Ovani Sound & Brutiful Games
 * No credit required.
 * Revision: 06/20/2023
 */