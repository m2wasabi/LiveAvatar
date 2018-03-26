using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackingCalibrator : MonoBehaviour
{
    public bool calibratingMode = false;
    public GameObject leftHandController;
    public GameObject rightHandController;

    public GameObject leftHandGuide;
    public GameObject rightHandGuide;

    public GameObject trackingOffset;

    private SteamVR_Controller.Device _device_L,_device_R;

	void Start ()
	{
	    if (leftHandController != null)
	    {
	        var controller_l = leftHandController.GetComponent<SteamVR_TrackedObject>();
	        _device_L = SteamVR_Controller.Input((int)controller_l.index);
	    }
	    if (rightHandController != null)
	    {
	        var controller = rightHandController.GetComponent<SteamVR_TrackedObject>();
	        _device_R = SteamVR_Controller.Input((int) controller.index);
	    }

    }
	
	void Update () {
	    if (Input.GetKeyDown(KeyCode.I))
	    {
	        calibratingMode = !calibratingMode;
	        return;
	    }

	    if ((_device_L.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) &&
	         _device_R.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
	        || (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) &&
	            _device_L.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu)))
	    {
	        calibratingMode = true;
	        return;
        }

        if (calibratingMode && leftHandController != null && rightHandController != null && leftHandGuide != null && rightHandGuide != null && trackingOffset != null)
	    {

	        if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
	        {
                // 左手首→右手首の位置から首の位置を求める
                Vector3 guideNeckPos = Vector3.Lerp(leftHandGuide.transform.position, rightHandGuide.transform.position, 0.5f);
                Vector3 trackNeckPos = Vector3.Lerp(leftHandController.transform.position, rightHandController.transform.position, 0.5f);

	            // 左手首→右手首のベクトル を求めて、トラッキングと空間の回転角を出す
                Vector3 guideVectorL2R = rightHandGuide.transform.position - leftHandGuide.transform.position;
	            Vector3 trackVectorL2R = rightHandController.transform.position - leftHandController.transform.position;
                var _rot = Quaternion.FromToRotation(trackVectorL2R, guideVectorL2R);

                // transform
	            trackingOffset.transform.position += guideNeckPos - _rot * trackNeckPos;
                trackingOffset.transform.rotation = _rot * trackingOffset.transform.rotation ;

                calibratingMode = false;
            }
        }
    }
}
