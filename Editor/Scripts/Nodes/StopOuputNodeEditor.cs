using Josephus.AudioGraph.Nodes;
using Josephus.NodeSystem.Editor;
using UnityEditor;

[CustomNodeEditor(typeof(StopOutputNode))]
public class StopOuputNodeEditor : NodeEditor
{
    StopOutputNode outputNode;

    public override void OnBodyGUI()
    {
        if (outputNode == null)
            outputNode = (StopOutputNode)target;

        outputNode.Description = EditorGUILayout.TextField("Description", outputNode.Description);

        var eventInput = outputNode.GetInputPort("Event");
        NodeEditorGUILayout.PortField(eventInput);

        var stopOutputInput = outputNode.GetInputPort("OutputNodeReference");
        NodeEditorGUILayout.PortField(stopOutputInput);

    }
}
