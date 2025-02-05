using Josephus.AudioGraph.Nodes;
using Josephus.AudioGraph.ScriptableObjects;
using Josephus.NodeSystem.Editor;
using System.Linq;
using UnityEditor;

namespace Josephus.AudioGraph.Editor.Nodes
{
    [CustomNodeEditor(typeof(ParameterNode))]
    public class ParameterNodeEditor : NodeEditor
    {
        ParameterNode paramNode;

        public override void OnBodyGUI()
        {
            if (paramNode == null)
                paramNode = (ParameterNode)target;

            serializedObject.Update();
            string[] excludes = { "m_Script", "graph", "position", "ports" };

            // Iterate through serialized properties and draw them like the Inspector (But with ports)
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);
            }

            var valuePort = target.GetOutputPort("Value");
            switch (paramNode.Type)
            {
                case ParameterNodeType.Bool:
                    valuePort.ValueType = typeof(bool);
                    break;
                case ParameterNodeType.Float:
                    valuePort.ValueType = typeof(float);
                    break;
                case ParameterNodeType.Int:
                    valuePort.ValueType = typeof(int);
                    break;
                case ParameterNodeType.AudioGroup:
                    valuePort.ValueType = typeof(AudioGroup);
                    break;
            }


            NodeEditorGUILayout.PortField(valuePort);

            serializedObject.ApplyModifiedProperties();
        }
    }
}