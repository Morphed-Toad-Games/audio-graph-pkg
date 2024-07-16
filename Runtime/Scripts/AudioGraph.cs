using Josephus.AudioGraph.Collections;
using Josephus.AudioGraph.Models;
using Josephus.AudioGraph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using XNode;

namespace Josephus.AudioGraph
{
    [CreateAssetMenu(menuName = "Audio/Graph")]
    public class AudioGraph : NodeGraph
    {
        public float DistanceToListener { get; set; }

        public bool IsInstance { get; set; }

        [SerializeField, HideInInspector] private SerializableDictionary<SerializableGuid, BaseAudioNode> nodesByGuid = new();

        public IReadOnlyList<OutputNode> GetEventOutputs(string eventName)
        {
            Profiler.BeginSample("GetEventOutputs");
            var list = new List<OutputNode>();
            var eventNodes = nodes
                .Where(x => x is EventNode eventNode && eventNode.EventName == eventName);

            if (eventNodes.Any() == false)
                throw new Exception($"Event {eventName} not found in Audio Graph {name}");

            var ports = eventNodes
                .Select(x => x.GetOutputPort("Event"))
                .SelectMany(x => x.GetConnections());

            foreach (var connections in ports)
            {
                var node = RecursivelyFindOutput(connections);
                if (node != null)
                    list.Add(node);
            }

            Profiler.EndSample();
            return list;
        }

        public override Node CopyNode(Node original)
        {
            var newNode = base.CopyNode(original);
            if (newNode is BaseAudioNode audioNode)
            {
                audioNode.GenerateGuid();
            }
            return newNode;
        }

        public override Node AddNode(Type type)
        {
            var node = base.AddNode(type);
            if (node is BaseAudioNode audioNode)
            {
                audioNode.GenerateGuid();
                nodesByGuid.Add(audioNode.NodeId, audioNode);
            }

            return node;
        }

        public override NodeGraph Copy()
        {
            // Instantiate a new nodegraph instance
            var graph = Instantiate(this);
            graph.nodesByGuid.Clear();

            // Instantiate all nodes inside the graph
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (node == null) continue;

                Node.graphHotfix = graph;
                var copiedNode = Instantiate(node);

                if(node is BaseAudioNode audioNode)
                {
                    var copiedAudioNode = copiedNode as BaseAudioNode;

                    copiedAudioNode.SetGuid(audioNode.NodeId);
                    graph.nodesByGuid.Add(copiedAudioNode.NodeId, copiedAudioNode);
                }

                copiedNode.graph = graph;
                graph.nodes[i] = copiedNode;
            }

            // Redirect all connections
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                if (graph.nodes[i] == null) continue;
                foreach (NodePort port in graph.nodes[i].Ports)
                {
                    port.Redirect(nodes, graph.nodes);
                }
            }

            return graph;
        }

        protected override void OnDestroy()
        {
            if (IsInstance)
                base.OnDestroy();

            //If we're not an instance, we don't want to do anything on destroy
        }

        public void SetParameter(string name, float value)
        {
            var parameterNode = nodes.FirstOrDefault(x => x is ParameterNode) as ParameterNode;
            if (parameterNode == null)
            {
                Debug.LogWarning($"Parameter {name} not found in Audio Graph");
                return;
            }

            parameterNode.Value = value;
        }

        public BaseAudioNode GetNode(SerializableGuid guid)
        {
            if (nodesByGuid.TryGetValue(guid, out var node))
                return node;

            return null;
        }

        OutputNode RecursivelyFindOutput(NodePort connection)
        {
            if (connection.node == null)
                return null;

            if (connection.node is OutputNode outputNode)
            {
                return outputNode;
            }
            else if (connection.node is DemuxNode demuxNode)
            {
                var port = demuxNode.GetTriggerPort().Connection;
                if (port == null)
                    return null;

                return RecursivelyFindOutput(port);
            }

            return null;
        }
    }
}