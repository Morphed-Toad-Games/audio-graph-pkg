using Josephus.AudioGraph.Models;
using System;
using UnityEngine;
using XNode;

namespace Josephus.AudioGraph
{
    public abstract class BaseAudioNode : Node
    {
        protected AudioGraph Graph => graph as AudioGraph;

        [field: SerializeField] public SerializableGuid NodeId { get; private set; }

        public void GenerateGuid()
            => NodeId = new(Guid.NewGuid());

        public void SetGuid(SerializableGuid guid)
            => NodeId = guid;
    }
}