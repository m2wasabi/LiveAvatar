using UnityEngine;
using Valve.VR;

public class FaceControllerVive : MonoBehaviour {

    public GameObject leftHandController;
    public GameObject rightHandController;

public bool EdittingFace = false;

    private SteamVR_Controller.Device _device_L, _device_R;

    void Start () {
        if (leftHandController != null)
        {
            var controller_l = leftHandController.GetComponent<SteamVR_TrackedObject>();
            _device_L = SteamVR_Controller.Input((int)controller_l.index);
        }
        if (rightHandController != null)
        {
            var controller_r = rightHandController.GetComponent<SteamVR_TrackedObject>();
            _device_R = SteamVR_Controller.Input((int)controller_r.index);
        }

    }

    void Update () {
        if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !_device_R.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log("Into Face Con");
            EdittingFace = true;
        }

        if (EdittingFace && _device_R.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("Out Face Con");
            EdittingFace = false;
        }
    }
}
