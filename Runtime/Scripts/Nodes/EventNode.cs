using Josephus.AudioGraph.Models;
using Josephus.NodeSystem;
using static Josephus.NodeSystem.Node;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Event")]
    public class EventNode : BaseAudioNode
    {
        public string EventName;
        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Multiple)] public AudioEvent Event;

        public override object GetValue(NodePort port)
        {
            return Event;
        }
    }
}