using Josephus.AudioGraph.Nodes;
using Josephus.AudioGraph.ScriptableObjects;
using System;
using UnityEngine;

namespace Josephus.AudioGraph.Models
{
    [Serializable]
    public class AudioGraphParameterPair : ISerializationCallbackReceiver
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public object Value { get; set; }
        [field: SerializeField] public ParameterNodeType Type { get; set; }

        [SerializeField, HideInInspector]
        private string SerializedValue;
        [SerializeField, HideInInspector]
        private UnityEngine.Object SerializedValueUnity;

        public AudioGraphParameterPair(string name, object value, ParameterNodeType type)
        {
            Name = name;
            Type = type;
            Value = value;

            if (Value == null)
            {
                switch (Type)
                {
                    case ParameterNodeType.Bool:
                        Value = default(bool);
                        break;
                    case ParameterNodeType.Float:
                        Value = default(float);
                        break;
                    case ParameterNodeType.Int:
                        Value = default(int);
                        break;
                }
            }
        }


        public void OnBeforeSerialize()
        {
            switch (Type)
            {
                case ParameterNodeType.Bool:
                case ParameterNodeType.Float:
                case ParameterNodeType.Int:
                    SerializedValue = Value == null 
                        ? "" 
                        : Value.ToString();
                    break;
                case ParameterNodeType.AudioGroup:
                    SerializedValueUnity = (AudioGroup)Value;
                    break;
            }
        }

        public void OnAfterDeserialize()
        {
            switch (Type)
            {
                case ParameterNodeType.Bool:
                    if (bool.TryParse(SerializedValue, out var b))
                        Value = b;
                    else
                        Value = default(bool);
                    break;
                case ParameterNodeType.Float:
                    if (float.TryParse(SerializedValue, out var f))
                        Value = f;
                    else
                        Value = default(float);
                    break;
                case ParameterNodeType.Int:
                    if (int.TryParse(SerializedValue, out var i))
                        Value = i;
                    else
                        Value = default(int);
                    break;
                case ParameterNodeType.AudioGroup:
                    Value = SerializedValueUnity == null
                        ? null
                        : (object)(AudioGroup)SerializedValueUnity;
                    break;
            }
        }
    }
}