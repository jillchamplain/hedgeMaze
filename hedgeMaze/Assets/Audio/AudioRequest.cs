using UnityEngine;

namespace Ubisoft.Systems.Audio
{
    // Written by Seth Riddensdale
    // 2/20/26
    public class AudioRequest
    {
        // References
        public AudioObject myAudioObject;
        public SoundStreamSO stream;

        // Basic settings
        public bool looping;
        public float delay;

        // 3D settings
        public bool isInitializedAtPoint;
        public Vector3 point;

        public bool isTracking;
        public Transform tracked;

        public AudioRequest(SoundStreamSO stream)
        {
            this.stream = stream;
        }

        public AudioRequest SetPoint(Vector3 point)
        {
            // Do not set point of an AudioRequest that is not 3D
            if (!stream.doSpatialAudio)
            {
                Debug.LogWarning($"[AudioManager]: Cannot set point of a 2D object! doSpatialAudio is not enabled for {stream.name}");
                return this;
            }

            // Set point
            isInitializedAtPoint = true;
            this.point = point;

            return this;
        }

        public AudioRequest SetTracked(Transform tracked)
        {
            // Do not set a tracker for an AudioRequest that is not 3D
            if (!stream.doSpatialAudio)
            {
                Debug.LogWarning($"[AudioManager]: Cannot set tracker of a 2D object! doSpatialAudio is not enabled for {stream.name}");
                return this;
            }

            // Enable tracking
            isTracking = true;
            this.tracked = tracked;

            return this;
        }

        public AudioRequest SetLooping(bool looping)
        {
            looping = true;
            return this;
        }

        public AudioRequest SetDelay(float delay = 0.0f)
        {
            this.delay = delay;
            return this;
        }

        // Initializes an AudioObject based on our request
        public void InitializeAudioObject(AudioObject audioObject)
        {
            // Set basic audio settings
            audioObject.source.clip = stream.GetClip();
            audioObject.source.pitch = stream.GetPitch();
            audioObject.source.volume = stream.volume;

            // We need to disable spatial blending if we don't want our AudioObject to be 3D
            if (!stream.doSpatialAudio)
            {
                audioObject.source.spatialBlend = 0;
            }
            // Enable spatial blending if AudioObject is 3D
            else
            {
                audioObject.source.spatialBlend = 1;
                audioObject.source.minDistance = stream.minDistance;
                audioObject.source.maxDistance = stream.maxDistance;

                // Initialize audio at specified point
                if (isInitializedAtPoint)
                {
                    audioObject.transform.position = point;
                }

                // Track transform
                if (isTracking)
                {
                    audioObject.SetTrackedObject(tracked);
                }
            }

            // Set AudioObject and hold onto it until it's released into the pool
            myAudioObject = audioObject;
            myAudioObject.OnReleased += UntrackRequest;
        }

        // Stop tracking this AudioSource, the request is fufilled
        private void UntrackRequest()
        {
            myAudioObject.OnReleased -= UntrackRequest;
            myAudioObject = null;
        }

        public void Stop()
        {
            if (myAudioObject == null) return;

            myAudioObject.source.Pause();
        }
    }
}
