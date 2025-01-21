using Josephus.AudioGraph.Collections;
using Josephus.AudioGraph.Models;
using System;
using UnityEngine;
using Josephus.NodeSystem;
using UnityEngine.Audio;

namespace Josephus.AudioGraph.Nodes
{
    [Serializable]
    [CreateNodeMenu("Output/Output Node")]
    public class OutputNode : BaseAudioNode
    {
        public string Description = "";
        public AudioMixerGroup AudioMixerGroup;
        public float Gain = 0;

        [Input(typeConstraint = TypeConstraint.Strict)] public AudioEvent Event;
        [Input(typeConstraint = TypeConstraint.Strict)] public AudioSample Sample;

        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public OutputNode NodeReference;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(NodeReference))
                return this;

            return GetInputValue("Sample", Sample);
        }

        public AudioSample GetSample()
            => GetInputValue("Sample", Sample);

        protected virtual void MutateAudioSource(ref AudioSource audioSource)
        {
            var sample = GetInputValue("Sample", Sample);

            audioSource.clip = sample.Clip;
            audioSource.volume = sample.Volume + Gain;
            audioSource.pitch = sample.Pitch;
            audioSource.outputAudioMixerGroup = AudioMixerGroup;
        }

        public virtual void OnPlay(SerializableDictionary<SerializableGuid, AudioSource> pool)
        {
            var sample = GetSample();
            if (sample.Volume == 0)
                return;

            if (pool.TryGetValue(NodeId, out var audioSource))
            {
                MutateAudioSource(ref audioSource);
                audioSource.PlayDelayed(sample.Delay);
            }
        }

        public virtual Component OnCreateAudioSource(AudioSource audioSource)
        {
            return null;
        }
    }
}