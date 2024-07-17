using Josephus.AudioGraph.Collections;
using Josephus.AudioGraph.Models;
using System;
using UnityEngine;
using Josephus.NodeSystem;

namespace Josephus.AudioGraph.Nodes
{
    [Serializable]
    [CreateNodeMenu("Output/Output Node")]
    public class OutputNode : BaseAudioNode
    {
        public string Description = "";

        [Input(typeConstraint = TypeConstraint.Strict)] public AudioEvent Event;
        [Input(typeConstraint = TypeConstraint.Strict)] public AudioSample Sample;
        public float Gain = 1;

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

            var linearGain = Mathf.Pow(10, Gain / 20);

            audioSource.clip = sample.Clip;
            audioSource.volume = sample.Volume * linearGain;
            audioSource.pitch = sample.Pitch;
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