using UnityEngine;
using Valve.VR;

public class ScenarioController : MonoBehaviour {

    public GameObject leftHandController;

    private SteamVR_Controller.Device _device_L;

    public GameObject ScrollViewUI;
    private ScenarioLoader _scenarioLoader;

    [SerializeField] private bool IsEnabled = true;

    void Start () {
        if (leftHandController != null)
        {
            var controller_l = leftHandController.GetComponent<SteamVR_TrackedObject>();
            _device_L = SteamVR_Controller.Input((int)controller_l.index);
        }
        _scenarioLoader = GetComponent<ScenarioLoader>();
    }

    void Update () {
        if (!IsEnabled) { return; }
        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            ScrollViewUI.SetActive(!ScrollViewUI.activeSelf);
        }

        if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && _device_L.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
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

        if (_device_L.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && ScrollViewUI.activeSelf)
        {
            _scenarioLoader.ToggleAction();
        }

    }

    public void SetEnabled(bool flag)
    {
        IsEnabled = flag;
        if (!flag)
        {
            ScrollViewUI.SetActive(false);
        }
    }
}
