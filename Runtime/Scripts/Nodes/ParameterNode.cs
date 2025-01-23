using Josephus.AudioGraph.ScriptableObjects;
using Josephus.NodeSystem;
using UnityEngine;

namespace Josephus.AudioGraph.Nodes
{
    public enum ParameterNodeType
    {
        Bool,
        Float,
        Int,
        AudioGroup
    }

    [CreateNodeMenu("Misc/Parameter")]
    public class ParameterNode : BaseAudioNode, ISerializationCallbackReceiver
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ParameterNodeType Type { get; private set; }
        [Output(typeConstraint = TypeConstraint.Strict)] public object Value;

        [SerializeField, HideInInspector]
        private string SerializedValue;
        [SerializeField, HideInInspector]
        private Object SerializedValueUnity;

        public override object GetValue(NodePort port)
        {
            switch (Type)
            {
                case ParameterNodeType.Bool:
                    Value ??= default(bool);
                    return (bool)Value;
                case ParameterNodeType.Float:
                    Value ??= default(float);
                    return (float)Value;
                case ParameterNodeType.Int:
                    Value ??= default(int);
                    return (int)Value;
                case ParameterNodeType.AudioGroup:
                    return (AudioGroup)Value;
            }

            return Value;
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