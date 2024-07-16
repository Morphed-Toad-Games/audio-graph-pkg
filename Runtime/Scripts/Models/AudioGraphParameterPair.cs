using System;
using UnityEngine;

namespace Josephus.AudioGraph.Models
{
    [Serializable]
    public class AudioGraphParameterPair
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public float Value { get; set; }

        public AudioGraphParameterPair(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}