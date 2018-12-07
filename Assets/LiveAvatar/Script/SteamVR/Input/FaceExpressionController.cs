using LiveAvatar.AvatarController.VRM;
using UnityEngine;
using VRM;

public class FaceExpressionController : MonoBehaviour {

    public GameObject RightHandController;
    public bool AutoEmote = true;

    private SteamVR_Controller.Device _device_R;

    private bool _edittingFace = false;

    private BlendShapeController _blendShapeController;

    public BlendShapeKey AutoDetectedFace;

    void Start ()
    {
        AutoDetectedFace = new BlendShapeKey(BlendShapePreset.Neutral);
        _blendShapeController = GetComponent<BlendShapeController>();
        if (RightHandController != null)
        {
            var controller_r = RightHandController.GetComponent<SteamVR_TrackedObject>();
            _device_R = SteamVR_Controller.Input((int)controller_r.index);
        }
    }

    void Update ()
    {
        if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) AutoEmote = !AutoEmote;
        
        if (AutoEmote)
        {
            _blendShapeController.SetFaceExpression(AutoDetectedFace, 1.0f);
        }
        else
        {
            if(_device_R.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                ResetFaceExpression();
            }

            if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && _device_R.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Into Face Con");
                _edittingFace = true;
            }

            if (_edittingFace && _device_R.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Debug.Log("Out Face Con");
                _edittingFace = false;
            }
            if(_edittingFace) UpdateFaceExpression();
        }

    }

    void ResetFaceExpression()
    {
        _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Neutral), 0);
    }

    void UpdateFaceExpression()
    {
        var rightPos = _device_R.GetAxis();
        var rightDeg = Vector2.Angle(new Vector2(-1, 0), rightPos);
        var faceMag = rightPos.magnitude;
        if (rightPos.y < 0)
        {
            rightDeg = 360 - rightDeg;
        }
    
        if (rightDeg >= 30 && rightDeg < 90)
        {
            // Smile
            _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Joy), faceMag);
        }
        else if (rightDeg >= 90 && rightDeg < 150)
        {
            // Angry
            _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Angry), faceMag);
        }
        else if (rightDeg >= 150 && rightDeg < 210)
        {
            // Sorrow
            _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Sorrow), faceMag);
        }
        else if (rightDeg >= 210 && rightDeg < 270)
        {
            // EyeClose
            _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Blink), faceMag);
        }
        else if (rightDeg >= 270 && rightDeg < 330)
        {
            // Fun
            _blendShapeController.SetFaceExpression(new BlendShapeKey(BlendShapePreset.Fun), faceMag);
        }
    }
}
