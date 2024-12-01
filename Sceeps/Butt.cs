using System.Collections;
using UnityEngine;

namespace SophisticatedStreamCam.Sceeps
{
    public class Butt : GorillaPressableButton
    {
        public bool timer = true;
        private bool fp = false;

        public static GameObject fpp;

        public static GameObject bk;

        public override void Start()
        {
            gameObject.layer = 18;
        }
        public override void ButtonActivation()
        {
            GameObject fartas = GameObject.Find("frist");
            GameObject farta = GameObject.Find("bakc");
            if(timer && !fp)
            {
                timer = false;
                fp = true;
                bk.SetActive(false);
                fpp.SetActive(true);
                fartas.SetActive(true);
                farta.SetActive(false);
                StartCoroutine(TimerCoroutine(2f));
            }
            else if(timer && fp)
            {
                timer = false;
                fp = false;
                bk.SetActive(true);
                fpp.SetActive(false);
                farta.SetActive(true);
                fartas.SetActive(false);
                StartCoroutine(TimerCoroutine(2f));
            }
        }

        private IEnumerator TimerCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            timer = true;
        }
    }
}