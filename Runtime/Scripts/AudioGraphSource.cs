using Josephus.AudioGraph.Collections;
using Josephus.AudioGraph.Models;
using Josephus.AudioGraph.Nodes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Josephus.AudioGraph
{
    public class AudioGraphSource : MonoBehaviour
    {
        private static AudioListener cachedAudioListener;

        public AudioGraph AudioGraphBlueprint;
        public List<AudioGraphParameterPair> GraphInstanceParameters = new();

        [SerializeField] private SerializableDictionary<SerializableGuid, AudioSource> audioSourcePool = new();
        [SerializeField] private List<Component> additionalComponents = new();

        private AudioGraph audioGraphInstance;
        private Transform audioTransform;

        private void Start()
        {
            audioTransform = transform;

            Apply();
        }

        public static AudioListener GetActiveAudioListener()
        {
            if (cachedAudioListener == null)
                cachedAudioListener = FindFirstObjectByType<AudioListener>();

            return cachedAudioListener;
        }

        public int GetAudioSourcePoolCount()
            => audioSourcePool.Count;

        public void Apply()
        {
            if (AudioGraphBlueprint == null)
            {
                ClearPooledAudioSources();
                GraphInstanceParameters.Clear();
            }
            else
            {
                audioGraphInstance = (AudioGraph)AudioGraphBlueprint.Copy();
                audioGraphInstance.IsInstance = true;
                SetupParameters();
                PoolAudioSources();
            }
        }

        void ClearPooledAudioSources()
        {
            Profiler.BeginSample("ClearPooledAudioSources");

            foreach (var item in additionalComponents)
                DestroyImmediate(item);

            audioSourcePool.Clear();
            additionalComponents.Clear();

            Profiler.EndSample();
        }

        void PoolAudioSources()
        {
            Profiler.BeginSample("PoolAudioSources");

            ClearPooledAudioSources();

            var outputNodes = AudioGraphBlueprint.nodes
                .Where(x => x is OutputNode)
                .Select(x => x as OutputNode);

            foreach (var node in outputNodes)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.hideFlags = HideFlags.HideInInspector;
                audioSourcePool.Add(node.NodeId, source);
                var extra = node.OnCreateAudioSource(source);

                additionalComponents.Add(source);
                if (extra != null)
                    additionalComponents.Add(extra);
            }

            Profiler.EndSample();
        }

        void SetupParameters()
        {
            var parameters = AudioGraphBlueprint.nodes
                .Where(x => x is ParameterNode)
                .Select(x => (ParameterNode)x)
                .ToArray();

            var oldParams = GraphInstanceParameters.ToArray();
            GraphInstanceParameters.Clear();
            foreach (var parameter in parameters)
            {
                var value = oldParams.Any(x => x.Name == parameter.Name)
                    ? oldParams.First(x => x.Name == parameter.Name).Value
                    : 0;

                var localParam = new AudioGraphParameterPair(parameter.Name, value);
                GraphInstanceParameters.Add(localParam);
            }
        }

        public void SendEvent(string eventName)
        {
            Profiler.BeginSample("SendEvent");

            Profiler.BeginSample("Calculate Distance");

                var distance = Vector3.Distance(audioTransform.position, GetActiveAudioListener().transform.position);
                audioGraphInstance.DistanceToListener = distance;

            Profiler.EndSample();

            Profiler.BeginSample("Update Params");

                foreach (var param in GraphInstanceParameters)
                {
                    audioGraphInstance.SetParameter(param.Name, param.Value);
                }

            Profiler.EndSample();

            Profiler.BeginSample("Get Outputs");

                var samples = audioGraphInstance.GetEventOutputs(eventName);

            Profiler.EndSample();

            Profiler.BeginSample("Play Samples");

                foreach (var output in samples)
                {
                    output.OnPlay(audioSourcePool);
                }

            Profiler.EndSample();

            Profiler.EndSample();
        }
    }
}