using System;
using UnityEngine;

namespace Josephus.AudioGraph.Models
{
    [Serializable]
    public class AudioSample
    {
        public AudioClip Clip { get; }
        public float Volume { get; }
        public float Pitch { get; }
        public float Delay { get; }
        public bool Loop { get; }

        public AudioSample(AudioClip clip, float volume, float pitch, float delay, bool loop)
        {
            Clip = clip;
            Volume = volume;
            Pitch = pitch;
            Delay = delay;
            Loop = loop;
        }
    }
}