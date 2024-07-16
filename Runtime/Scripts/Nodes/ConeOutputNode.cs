using System.Linq;
using UnityEngine;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Output/Cone Output Node")]
    public class ConeOutputNode : OutputNode
    {
        public const int STEPS = 5;

        public AnimationCurve FallOffCurve;
        public bool AutomaticallyGenerateCurve;
        public float MinDistance;
        public float MaxDistance;

        public float MaxAngle = 360;
        public float MinAngle = 360;

        protected override void MutateAudioSource(ref AudioSource audioSource)
        {
            base.MutateAudioSource(ref audioSource);
            audioSource.maxDistance = FallOffCurve.keys.Last().time;
            audioSource.spatialBlend = 1.0f;
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, FallOffCurve);
        }

        public override Component OnCreateAudioSource(AudioSource audioSource)
        {
            var coneController = audioSource.gameObject.AddComponent<AudioSourceVolumeCone>();
            coneController.SetAngles(MinAngle, MaxAngle);
            coneController.SetAudioSource(audioSource);
            coneController.hideFlags = HideFlags.HideInInspector;

            return coneController;
        }

        private void OnValidate()
        {
            if (AutomaticallyGenerateCurve)
                FallOffCurve = CurveHelpers.GenerateLogCurve(MinDistance, MaxDistance, STEPS);
        }
    }
}