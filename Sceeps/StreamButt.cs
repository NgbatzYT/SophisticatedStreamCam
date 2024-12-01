using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;


namespace SophisticatedStreamCam.Sceeps
{
    public class StreamButt : GorillaPressableButton
    {
        public bool timer = true;
        public bool Streaming = false;
        public static GameObject Stem;

        public override void Start()
        {
            Stem = GameObject.Find("ind");
        }

        public override void ButtonActivation()
        {
            if(timer && !Streaming)
            {
                Streaming = true;
                timer = false;
                StartCoroutine(TimerCoroutine(5f));
                Plugin.streamered.StartStreaming(Plugin.currentStreamKey);
            }
            else if(timer && Streaming)
            {
                Streaming = false;
                timer = false;
                StartCoroutine(TimerCoroutine(5f));
                Plugin.streamered.StopStreaming();
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