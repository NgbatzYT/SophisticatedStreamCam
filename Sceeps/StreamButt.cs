using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;


namespace SophisticatedStreamCam.Sceeps
{
    public class StreamButt : MonoBehaviour
    {
        public bool timer = true;
        public bool Streaming = false;
        private GameObject Stem;

        public void Start()
        {
            Stem = GameObject.Find("ind");
        }

        public void OnTriggerEnter(Collider col)
        {
            if(col.name == "RightHandTriggerCollider" && timer && !Streaming)
            {
                Streaming = true;
                timer = false;
                StartCoroutine(TimerCoroutine(5f));
                Plugin.streamer.StartStreaming(Plugin.currentStreamKey);
            }
            else if(col.name == "RightHandTriggerCollider" && timer && Streaming)
            {
                Streaming = false;
                timer = false;
                StartCoroutine(TimerCoroutine(5f));
                Plugin.streamer.StopStreaming();
            }
        }

        public void Update()
        {
            if(Streaming == true)
            {
                Stem.SetActive(true);
            }
            else
            {
                Stem.SetActive(false);
            }
        }

        private IEnumerator TimerCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            timer = true;
        }
    }
}