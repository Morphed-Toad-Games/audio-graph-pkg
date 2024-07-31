using UnityEngine;
using Josephus.NodeSystem;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Numerics/Random Number Generator")]
    public class RandomNumberGeneratorNode : BaseAudioNode
    {
        [Input(typeConstraint = TypeConstraint.Strict)] public float Min;
        [Input(typeConstraint = TypeConstraint.Strict)] public float Max;

        [Output(typeConstraint = TypeConstraint.Strict)] public float Number;

        public override object GetValue(NodePort port)
        {
            if (Application.isPlaying == false)
                return $"Random Number ({Min} to {Max})";

            Number = Random.Range(Min, Max);
            return Number;
        }
    }
}