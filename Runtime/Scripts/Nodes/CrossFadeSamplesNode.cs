using Josephus.AudioGraph.Models;
using Josephus.NodeSystem;
using UnityEngine;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Sampling/Cross Fade Samples")]
    public class CrossFadeSamplesNode : BaseAudioNode
    {
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioSample AudioSampleIn1;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioSample AudioSampleIn2;
        public float FadeBegin;
        public float FadeEnd;
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public float FadeTime;

        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioSample AudioSampleOut1;
        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioSample AudioSampleOut2;

        public override object GetValue(NodePort port)
        {
            var fadeTime = GetInputValue(nameof(FadeTime), FadeTime);
            if (port.fieldName == nameof(AudioSampleOut1))
            {
                var in1 = GetInputValue(nameof(AudioSampleIn1), AudioSampleIn1);
                var volume1 = Mathf.InverseLerp(FadeEnd, FadeBegin, fadeTime) * in1.Volume;
                AudioSampleOut1 = new AudioSample(in1.Clip, volume1, in1.Pitch, in1.Delay, in1.Loop);
                return AudioSampleOut1;
            }

            if (port.fieldName == nameof(AudioSampleOut2))
            {
                var in2 = GetInputValue(nameof(AudioSampleIn2), AudioSampleIn2);
                var volume2 = Mathf.InverseLerp(FadeBegin, FadeEnd, fadeTime) * in2.Volume;
                AudioSampleOut2 = new AudioSample(in2.Clip, volume2, in2.Pitch, in2.Delay, in2.Loop);
                return AudioSampleOut2;
            }

            return null;
        }
    }
}