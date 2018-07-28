using UnityEngine;
using Valve.VR;

namespace LiveAvatar.AvatarController.Ai
{
    public class FaceControllerVive : MonoBehaviour {
    
        public GameObject leftHandController;
        public GameObject rightHandController;
    
        public bool EdittingFace = false;
    
        private SteamVR_Controller.Device _device_L, _device_R;
    
        private EyelidsControler eyelidsControler;
        private EyesController eyesController;
        private MouthController mouthController;
    
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
            eyelidsControler = GetComponentInChildren<EyelidsControler>();
            eyesController   = GetComponentInChildren<EyesController>();
            mouthController  = GetComponentInChildren<MouthController>();
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
            if(EdittingFace) updateFacePose();
    
            if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && !_device_L.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                eyesController.EyeTrace = !eyesController.EyeTrace;
            }
        }
    
        private void updateFacePose()
        {
    //        var leftPos = _device_L.GetAxis();
            var rightPos = _device_R.GetAxis();
            Debug.Log(rightPos);
    
            eyelidsControler.BrowSmile = 0;
            eyelidsControler.BrowSurprize = 0;
            eyelidsControler.BrowSad = 0;
            eyelidsControler.BrowAnger3 = 0;
            eyelidsControler.BrowAnger2 = 0;
            eyelidsControler.BrowAnger1 = 0;
            eyelidsControler.EyeSmile = 0;
            eyelidsControler.EyeSurprize = 0;
            eyelidsControler.EyeSad = 0;
            eyelidsControler.EyeAnger3 = 0;
            eyelidsControler.EyeAnger2 = 0;
            eyelidsControler.EyeAnger1 = 0;
            eyelidsControler.EyeClose = 0;
            eyesController.EmoAnger = 0;
            eyesController.EmoSad = 0;
            eyesController.EmoSurprize = 0;
            eyesController.EmoShirome = false;
            mouthController.MouthEmotion = MouthController.Emotion.Smile;
    
            var rightDeg = Vector2.Angle(new Vector2(-1, 0), rightPos);
            var faceMag = rightPos.magnitude;
            if (rightPos.y < 0)
            {
                rightDeg = 360 - rightDeg;
            }
    
            if (rightDeg >= 22 && rightDeg < 67)
            {
                eyelidsControler.BrowSurprize = 77 * faceMag;
                eyelidsControler.EyeSmile = 30 * faceMag;
                eyelidsControler.EyeSad = 70 * faceMag;
                eyesController.EmoSad = 100 * faceMag;
            }
            else if (rightDeg >= 67 && rightDeg < 112)
            {
                eyelidsControler.BrowSmile = 100 * faceMag;
                eyelidsControler.EyeSmile = 100 * faceMag;
            }
            else if(rightDeg >= 112 && rightDeg < 157)
            {
                eyelidsControler.BrowSurprize = 100 * faceMag;
                eyelidsControler.EyeSurprize = 100 * faceMag;
                eyesController.EmoSurprize = 100 * faceMag;
                mouthController.MouthEmotion = MouthController.Emotion.Surprize;
            }
            else if (rightDeg >= 157 && rightDeg < 202)
            {
                eyelidsControler.BrowAnger3 = 100 * faceMag;
                eyelidsControler.EyeAnger3 = 100 * faceMag;
                eyesController.EmoAnger = 100 * faceMag;
                mouthController.MouthEmotion = MouthController.Emotion.Ang3;
            }
            else if (rightDeg >= 202 && rightDeg < 247)
            {
                eyelidsControler.BrowSad = 100 * faceMag;
                eyelidsControler.EyeSad = 100 * faceMag;
                eyesController.EmoSad = 100 * faceMag;
                mouthController.MouthEmotion = MouthController.Emotion.Sad;
            }
            else if (rightDeg >= 247 && rightDeg < 292)
            {
                eyelidsControler.EyeClose = 100 * faceMag;
            }
            else if (rightDeg >= 292 && rightDeg < 335)
            {
                eyelidsControler.BrowSmile = 50 * faceMag;
                eyelidsControler.EyeAnger3 = 70 * faceMag;
                eyelidsControler.EyeClose = 30 * faceMag;
            }
    
    
/*
            var leftDeg  =  Vector2.Angle(new Vector2(1, 0), leftPos);
            if (leftPos.y < 0)
            {
                leftDeg = 360 - leftDeg;
            }
            Debug.Log(leftDeg);
*/
    
        }
    
    }

}
