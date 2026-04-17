using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

namespace Ubisoft.Systems.Audio
{
    // Written by Seth Riddensdale
    // 2/19/26
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("References")]
        [SerializeField]
        private AudioObject _soundPrefab;

        private ObjectPool<AudioObject> pool;
        private Dictionary<string, AudioObject> keyedItems;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                pool = new ObjectPool<AudioObject>(CreateSource, OnTakeFromPool, OnReturnToPool);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayAudio(AudioRequest request)
        {
            if (!CanPlay(request.stream)) return;

            if (request.delay > 0.0f) StartCoroutine(IPlayAudioDelayed(request, request.delay));
            else CreateAudio(request);
        }

        #region Helpers
        /// <summary>
        /// Determines whether or not a sound can be played
        /// </summary>
        private bool CanPlay(SoundStreamSO clip)
        {
            if (clip == null)
            {
                Debug.LogWarning($"Tried to play a null SoundStream");
                return false;
            }

            if (clip.GetClip() == null)
            {
                Debug.LogWarning($"Cannot play {clip.name} with a null clip!");
                return false;
            }

            return true;
        }

        private void CreateAudio(AudioRequest request)
        {
            // 1. Take an inactive object from the pool
            // 2. The request serves as the blueprint, and the key. The request sets the data of the object. We can use the request to access it if it needs to be stopped
            // 3. Start playing Audio

            AudioObject audioObject = SpawnAudioObject(request.stream);
            request.InitializeAudioObject(audioObject);
            audioObject.source.Play();
        }

        private IEnumerator IPlayAudioDelayed(AudioRequest request, float delay)
        {
            yield return new WaitForSeconds(delay);

            CreateAudio(request);
        }

        private AudioObject SpawnAudioObject(SoundStreamSO clip)
        {
            pool.Get(out AudioObject audioObj);

            return audioObj;
        }

        private AudioObject CreateSource()
        {
            var source = Instantiate(_soundPrefab);
            source.transform.parent = transform;

            return source;
        }

        private void OnTakeFromPool(AudioObject audioObj)
        {
            audioObj.gameObject.SetActive(true);
            audioObj.Initialize(pool);
        }

        private void OnReturnToPool(AudioObject audioObj)
        {
            audioObj.gameObject.SetActive(false);
            audioObj.source.Stop();
        }
        #endregion
    }
}
