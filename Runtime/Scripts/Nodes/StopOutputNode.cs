using Josephus.AudioGraph.Collections;
using Josephus.AudioGraph.Models;
using System;
using UnityEngine;

namespace Josephus.AudioGraph.Nodes
{
    [CreateNodeMenu("Output/Stop Output Node")]
    public class StopOutputNode : OutputNode
    {
        [Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] public OutputNode OutputNodeReference;

        public override void OnPlay(SerializableDictionary<SerializableGuid, AudioSource> pool)
        {
            var nodeRef = GetInputValue(nameof(OutputNodeReference), OutputNodeReference);
            if (nodeRef == null)
                throw new Exception("OutputNodeReference is null. We have nothing to stop!");

            if (pool.TryGetValue(nodeRef.NodeId, out var audioSource))
            {
                audioSource.Stop();
            }
        }
    }
}