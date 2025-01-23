using Josephus.AudioGraph;
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
            parameter.Value = EditorGUILayout.FloatField(parameter.Name, parameter.Value);
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
