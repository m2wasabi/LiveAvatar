using UnityEngine;
using Valve.VR;

public class ScenarioController : MonoBehaviour {

    public GameObject leftHandController;
    public GameObject rightHandController;

    private SteamVR_Controller.Device _device_L, _device_R;

    public GameObject ScrollViewUI;

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
        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && !_device_R.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            ScrollViewUI.SetActive(!ScrollViewUI.activeSelf);
        }

    }
}
