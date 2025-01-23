using Josephus.AudioGraph;
using Josephus.AudioGraph.ScriptableObjects;
using UnityEditor;

[CustomEditor(typeof(AudioGraphSource))]
public class AudioGraphSourceEditor : Editor
{
    AudioGraphSource audioGraphSource;

    private void Awake()
    {
        audioGraphSource = (AudioGraphSource)target;
        audioGraphSource.Apply();
    }

    public override void OnInspectorGUI()
    {
        var selectedGraph = (AudioGraph)EditorGUILayout.ObjectField("AudioGraph Blueprint", audioGraphSource.AudioGraphBlueprint, typeof(AudioGraph), false);
        if (selectedGraph != audioGraphSource.AudioGraphBlueprint)
        {
            audioGraphSource.AudioGraphBlueprint = selectedGraph;

            if (selectedGraph == null)
                OnGraphCleared();
            else
            {
                OnGraphChanged();
            }
        }

        if (selectedGraph != null)
            DrawGraph(selectedGraph);
    }

    void DrawGraph(AudioGraph graph)
    {
        EditorGUILayout.Separator();

        EditorGUI.BeginChangeCheck();

        foreach (var parameter in audioGraphSource.GraphInstanceParameters)
        {
            switch (parameter.Type)
            {
                case Josephus.AudioGraph.Nodes.ParameterNodeType.Bool:
                    parameter.Value = EditorGUILayout.Toggle(parameter.Name, (bool)parameter.Value);
                    break;
                case Josephus.AudioGraph.Nodes.ParameterNodeType.Float:
                    parameter.Value = EditorGUILayout.FloatField(parameter.Name, (float)parameter.Value);
                    break;
                case Josephus.AudioGraph.Nodes.ParameterNodeType.Int:
                    parameter.Value = EditorGUILayout.IntField(parameter.Name, (int)parameter.Value);
                    break;
                case Josephus.AudioGraph.Nodes.ParameterNodeType.AudioGroup:
                    parameter.Value = (AudioGroup)EditorGUILayout.ObjectField(parameter.Name, (AudioGroup)parameter.Value, typeof(AudioGroup), allowSceneObjects: false);
                    break;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(audioGraphSource);
        }

        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox($"Pooled audio sources: {audioGraphSource.GetAudioSourcePoolCount()}", MessageType.Info);
    }

    void OnGraphChanged()
    {
        audioGraphSource.Apply();
        EditorUtility.SetDirty(audioGraphSource);
    }

    void OnGraphCleared()
    {
        audioGraphSource.Apply();
        EditorUtility.SetDirty(audioGraphSource);
    }
}
