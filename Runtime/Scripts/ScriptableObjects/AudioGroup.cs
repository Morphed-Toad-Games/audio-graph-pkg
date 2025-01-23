using UnityEngine;
using UnityEngine.Audio;

namespace Josephus.AudioGraph.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Audio/Group")]
    public class AudioGroup : ScriptableObject
    {
        [SerializeField] private AudioResource[] audioClips;

        public virtual AudioResource[] AudioClips => audioClips;
    }
}