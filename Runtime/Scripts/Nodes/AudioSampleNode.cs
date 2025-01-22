using Josephus.AudioGraph.Models;
using Josephus.AudioGraph.ScriptableObjects;
using Josephus.NodeSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Sampling/Audio Sample")]
    public class AudioSampleNode : BaseAudioNode
    {
        int lastGroupIndex;

        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioGroup AudioGroup;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public float Volume = 1;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public float Pitch = 1;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public float Delay = 0;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public bool Loop = false;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public bool Shuffle = false;

        [Output(typeConstraint = TypeConstraint.Strict)] public AudioSample AudioSample;

        public override object GetValue(NodePort port)
        {
            if (port.ValueType == typeof(AudioSample))
            {
                if (lastGroupIndex > AudioGroup.AudioClips.Length - 1)
                    lastGroupIndex = 0;

                AudioResource clip;
                if (Shuffle)
                    clip = AudioGroup.AudioClips[Random.Range(0, AudioGroup.AudioClips.Length)];
                else if (AudioGroup.AudioClips.Length > 0)
                    clip = AudioGroup.AudioClips[lastGroupIndex++];
                else
                    clip = null;

                return new AudioSample(
                    clip,
                    GetInputValue("Volume", Volume),
                    GetInputValue("Pitch", Pitch),
                    GetInputValue("Delay", Delay),
                    GetInputValue("Loop", Loop)
                );
            }

            return null;
        }
    }
}