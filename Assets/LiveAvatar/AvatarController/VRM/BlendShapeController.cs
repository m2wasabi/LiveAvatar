using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace LiveAvatar.AvatarController.VRM
{
    public class BlendShapeController : MonoBehaviour
    {
        public GameObject RightHandController;
        private SteamVR_Controller.Device _device_R;
        public Toggle FaceToggle;

        public float MouthOpen { get; set; }

        private VRMBlendShapeProxy m_blendShapePloxy;

        private bool _edittingFace = false;

        void Start () {
            if (RightHandController != null)
            {
                var controller_r = RightHandController.GetComponent<SteamVR_TrackedObject>();
                _device_R = SteamVR_Controller.Input((int)controller_r.index);
            }        
        }
	
        void Update () {
            if (m_blendShapePloxy)
            {
                if (FaceToggle.isOn)
                {
                    if (_device_R.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !_device_R.GetPress(SteamVR_Controller.ButtonMask.Grip))
                    {
                        Debug.Log("Into Face Con");
                        _edittingFace = true;
                    }
    
                    if (_edittingFace && _device_R.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        Debug.Log("Out Face Con");
                        _edittingFace = false;
                    }
                    if(_edittingFace) updateFacePose();
                }
                m_blendShapePloxy.SetValue(BlendShapePreset.A, MouthOpen);
            }
        }

        public void Setup()
        {
            m_blendShapePloxy = gameObject.GetComponentInChildren<VRMBlendShapeProxy>();
        }

        public void Setup(VRMBlendShapeProxy blendShapeProxy)
        {
            m_blendShapePloxy = blendShapeProxy;
        }

        private void updateFacePose()
        {
            var rightPos = _device_R.GetAxis();
            var FaceReset = new List<KeyValuePair<BlendShapeKey, float>>
            {
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.A),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.I),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.U),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.E),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.O),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Angry),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Fun),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Joy),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Blink),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Sorrow),0.0f),
            };
            m_blendShapePloxy.SetValues(FaceReset);
    
            var rightDeg = Vector2.Angle(new Vector2(-1, 0), rightPos);
            var faceMag = rightPos.magnitude;
            if (rightPos.y < 0)
            {
                rightDeg = 360 - rightDeg;
            }
    
            if (rightDeg >= 30 && rightDeg < 90)
            {
                // Smile
                m_blendShapePloxy.SetValue(BlendShapePreset.Joy, faceMag);
            }
            else if (rightDeg >= 90 && rightDeg < 150)
            {
                // Angry
                m_blendShapePloxy.SetValue(BlendShapePreset.Angry, faceMag);
            }
            else if (rightDeg >= 150 && rightDeg < 210)
            {
                // Sorrow
                m_blendShapePloxy.SetValue(BlendShapePreset.Sorrow, faceMag);
            }
            else if (rightDeg >= 210 && rightDeg < 270)
            {
                // EyeClose
                m_blendShapePloxy.SetValue(BlendShapePreset.Blink, faceMag);
            }
            else if (rightDeg >= 270 && rightDeg < 330)
            {
                // Fun
                m_blendShapePloxy.SetValue(BlendShapePreset.Fun, faceMag);
            }
        }
    }
}
