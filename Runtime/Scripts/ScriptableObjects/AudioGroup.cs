using UnityEngine;

namespace Josephus.AudioGraph.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Audio/Group")]
    public class AudioGroup : ScriptableObject
    {
        public AudioClip[] AudioClips;
    }
}