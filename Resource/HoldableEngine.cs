using UnityEngine;

namespace SophisticatedStreamCam.Resource
{
    class HoldableEngine : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 18;
        }
        void OnTriggerStay(Collider col)
        {
            GameObject CameraController = GameObject.Find("HandleL");
            if (col.name.Contains("Left"))
            {
                
                if (ControllerInputPoller.instance.leftGrab)
                {
                    CameraController.transform.parent = GorillaTagger.Instance.leftHandTransform.gameObject.transform;
                }
            }
            if (!ControllerInputPoller.instance.leftGrab & CameraController.transform.parent == GorillaTagger.Instance.leftHandTransform.gameObject.transform)
            {
                CameraController.transform.parent = null;
            }
        }
    }
}