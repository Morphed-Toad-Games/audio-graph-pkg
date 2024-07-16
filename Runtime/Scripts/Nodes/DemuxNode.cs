using Josephus.AudioGraph.Models;
using UnityEngine;
using XNode;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Misc/Demux")]
    public class DemuxNode : BaseAudioNode
    {
        [Input(typeConstraint = TypeConstraint.Strict)] public AudioEvent Event;
        [Input(typeConstraint = TypeConstraint.Strict)] public float Value;

        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioEvent Trigger0;
        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public AudioEvent Trigger1;

        public NodePort GetTriggerPort()
        {
            if (Mathf.Approximately(GetInputValue("Value", Value), 1))
                return GetOutputPort("Trigger1");

            return GetOutputPort("Trigger0");
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}