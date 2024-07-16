using UnityEngine;
using XNode;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Misc/Parameter")]
    public class ParameterNode : BaseAudioNode
    {
        [field: SerializeField] public string Name { get; private set; }
        [Output(typeConstraint = TypeConstraint.Strict)] public float Value;

        public override object GetValue(NodePort port)
        {
            return Value;
        }
    }
}