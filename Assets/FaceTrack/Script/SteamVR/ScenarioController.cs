using UnityEngine;
using Valve.VR;

public class ScenarioController : MonoBehaviour {

    public GameObject leftHandController;
    public GameObject rightHandController;

    private SteamVR_Controller.Device _device_L, _device_R;

    public GameObject ScrollViewUI;
    private ScenarioLoader _scenarioLoader;

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
        _scenarioLoader = GetComponent<ScenarioLoader>();
    }

    void Update () {
        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && !_device_R.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            ScrollViewUI.SetActive(!ScrollViewUI.activeSelf);
        }

        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !_device_L.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            var touchPos = _device_L.GetAxis();

            if (Mathf.Abs(touchPos.x) < Mathf.Abs(touchPos.y))
            {
                if (touchPos.y > 0.3)
                {
                    _scenarioLoader.MoveUp();
                }
                else if (touchPos.y < -0.3)
                {
                    _scenarioLoader.MoveDOwn();
                }
            }
        }

        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && ScrollViewUI.activeSelf)
        {
            _scenarioLoader.ToggleAction();
        }

    }
}
