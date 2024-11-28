using System.Collections;
using UnityEngine;

namespace SophisticatedStreamCam.Sceeps
{
    public class Butt : MonoBehaviour
    {
        public bool timer = true;
        private bool fp = false;

        private GameObject fpp;

        private GameObject bk;

        public void Start()
        {
            fpp = GameObject.Find("firstp");
            bk = GameObject.Find("back");
        }
        public void OnTriggerEnter(Collider col)
        {
            GameObject fartas = GameObject.Find("frist");
            GameObject farta = GameObject.Find("bakc");
            if(col.name == "RightHandTriggerCollider" && timer && !fp)
            {
                timer = false;
                fp = true;
                bk.SetActive(false);
                fpp.SetActive(true);
                fartas.SetActive(true);
                farta.SetActive(false);
                StartCoroutine(TimerCoroutine(2f));
            }
            else if(col.name == "RightHandTriggerCollider" && timer && fp)
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