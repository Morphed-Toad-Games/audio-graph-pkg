using XNode;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Misc/Get Distance To Listener")]
    public class GetDistanceToListenerNode : BaseAudioNode
    {
        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public float Distance;

        public override object GetValue(NodePort port)
        {
            return Graph.DistanceToListener;
        }
    }
}