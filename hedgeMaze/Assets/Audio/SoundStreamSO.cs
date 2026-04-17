using UnityEngine;
using NaughtyAttributes;

namespace Ubisoft.Systems.Audio
{
    // Written by Seth Riddensdale
    // 2/20/26

    [CreateAssetMenu(menuName = "Sound Stream", fileName = "New Sound Stream")]
    public class SoundStreamSO : ScriptableObject
    {
        [SerializeField]
        private AudioClip[] clip;

        [Header("Default Settings")]
        [Tooltip("Controls the volume scale of the sound")]
        public float volume;

        [SerializeField]
        private bool doRandomPitch;

        [SerializeField, Range(-3, 3), HideIf(nameof(doRandomPitch))]
        private float pitch = 1.0f;

        [SerializeField, MinMaxSlider(-3, 3), ShowIf(nameof(doRandomPitch))]
        private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

        [Header("Spatial Audio (3D Sound)")]
        [Tooltip("Spatial Audio allows you to modify the minimum and maximum distance from the camera that the sound can be heard")]
        public bool doSpatialAudio;

        [ShowIf(nameof(doSpatialAudio))]
        public int minDistance;
        [ShowIf(nameof(doSpatialAudio))]
        public int maxDistance;

        public float GetPitch()
        {
            if (doRandomPitch) return Random.Range(pitchRange.x, pitchRange.y);
            else return pitch;
        }

        public AudioClip GetClip()
        {
            return clip[Random.Range(0, clip.Length)];
        }
    }
}