using Josephus.AudioGraph.Helpers;
using System.Linq;
using UnityEngine;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Output/Spherical Output Node")]
    public class SphericalOutputNode : OutputNode
    {
        public const int STEPS = 5;

        public AnimationCurve FallOffCurve;
        public bool AutomaticallyGenerateCurve;
        public float MinDistance;
        public float MaxDistance;

        protected override void MutateAudioSource(ref AudioSource audioSource)
        {
            base.MutateAudioSource(ref audioSource);
            audioSource.maxDistance = FallOffCurve.keys.Last().time;
            audioSource.spatialBlend = 1.0f;
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, FallOffCurve);
        }

        private void OnValidate()
        {
            if (AutomaticallyGenerateCurve)
                FallOffCurve = CurveHelpers.GenerateLogCurve(MinDistance, MaxDistance, STEPS);
        }
    }
}