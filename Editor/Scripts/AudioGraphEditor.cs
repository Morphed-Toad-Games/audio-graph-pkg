using Josephus.AudioGraph;
using Josephus.AudioGraph.Nodes;
using Josephus.AudioGraph.ScriptableObjects;
using System;
using UnityEngine;
using XNode;
using XNode.NodeGroups;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(AudioGraph))]
public class AudioGraphEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(Type type)
    {
        if (typeof(BaseAudioNode).IsAssignableFrom(type) || type == typeof(NodeGroup))
        {
            return base.GetNodeMenuName(type);
        }
        else return null;
    }

    public override void OnDropObjects(UnityEngine.Object[] objects)
    {
        foreach (var obj in objects)
        {
            if(obj is AudioGroup group)
            {
                Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
                var node = CreateNode(typeof(AudioSampleNode), pos) as AudioSampleNode;
                node.AudioGroup = group;
            }
        }
    }
}
