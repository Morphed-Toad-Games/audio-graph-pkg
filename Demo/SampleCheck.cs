using UnityEngine;

namespace Josephus.AudioGraph.Demo
{
    public class SampleCheck : MonoBehaviour
    {
        public AudioGraphSource graph;
        public KeyCode Key;
        public float TimeDiff = 0.075f;

        float lastFireTime;

        private void Update()
        {
            if (Input.GetKey(Key))
            {
                if (Time.time > lastFireTime + TimeDiff)
                {
                    graph.SendEvent("Fire");
                    lastFireTime = Time.time;
                }
            }
            else if (Input.GetKeyUp(Key))
            {
                graph.SendEvent("Fire_Stop");
            }
        }
    }
}