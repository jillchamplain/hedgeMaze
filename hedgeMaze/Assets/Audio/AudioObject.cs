using UnityEngine;
using UnityEngine.Pool;

namespace Ubisoft.Systems.Audio
{
    // Written by Seth Riddensdale
    // 2/19/26

    public class AudioObject : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _source;

        ObjectPool<AudioObject> myPool;
        private bool initalized;
        private bool inPool;
        private Transform trackedObject;

        public AudioSource source { get { return _source; } }

        public delegate void Released();
        public Released OnReleased;

        /// <summary>
        /// This is called as soon as an object is initialized by the pool
        /// </summary>
        public void Initialize(ObjectPool<AudioObject> pool)
        {
            myPool = pool;

            initalized = true;
            inPool = false;
            trackedObject = null;
        }

        /// <summary>
        /// This function ensures the audio follows a target
        /// </summary>
        public void SetTrackedObject(Transform trackedObject)
        {
            this.trackedObject = trackedObject;
        }

        private void Update()
        {
            if (!initalized) return;

            // If there is an object to track, track it
            if (trackedObject != null)
            {
                transform.position = trackedObject.position;
            }

            // Return object to the pool
            if (!_source.isPlaying && !inPool)
            {
                myPool.Release(this);
                inPool = true;
                OnReleased?.Invoke();
            }
        }
    }
}
