using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace LiveAvatar.AvatarController.VRM
{
    public class BlendShapeController : MonoBehaviour
    {
        public enum EyeBlink
        {
            OFF,
            WebCamera
        }

        public BlendShapeKey EmotionStat;
        public float EmotionMag;
        public bool CameraMouth = true;
        public float MouthOpen { get; set; }
        public float LeftEyeOpen { get; set; }
        public float RightEyeOpen { get; set; }
        private EyeBlink _eyeBlink = EyeBlink.OFF;

        private VRMBlendShapeProxy m_blendShapePloxy;

        private List<KeyValuePair<BlendShapeKey, float>> m_faceReset;

        void Update () {
            if (m_blendShapePloxy != null)
            {
                UpdateFacePose();
            }
        }

        public void Setup()
        {
            m_blendShapePloxy = gameObject.GetComponentInChildren<VRMBlendShapeProxy>();
            InitializeFaceReset();
        }

        public void Setup(VRMBlendShapeProxy blendShapeProxy)
        {
            m_blendShapePloxy = blendShapeProxy;
            InitializeFaceReset();
        }

        private void UpdateFacePose()
        {
            m_blendShapePloxy.SetValues(m_faceReset);
            m_blendShapePloxy.SetValue(EmotionStat, EmotionMag);
            if (CameraMouth) m_blendShapePloxy.SetValue(BlendShapePreset.A, MouthOpen);
            if (EmotionStat.Name == "NEUTRAL" || EmotionStat.Name == "ANGRY")
            {
                if (_eyeBlink == EyeBlink.WebCamera)
                {
                    m_blendShapePloxy.SetValue(BlendShapePreset.Blink_L, 1.0f - LeftEyeOpen);
                    m_blendShapePloxy.SetValue(BlendShapePreset.Blink_R, 1.0f - RightEyeOpen);
                }
            }
        }

        private void InitializeFaceReset()
        {
            m_faceReset = new List<KeyValuePair<BlendShapeKey, float>>
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
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Blink_L),0.0f),
                new KeyValuePair<BlendShapeKey, float>(new BlendShapeKey(BlendShapePreset.Blink_R),0.0f),
            };
        }

        public void SetFaceExpression(BlendShapeKey blendShapeKey, float magnitude)
        {
            EmotionStat = blendShapeKey;
            EmotionMag = magnitude;
        }

        public void SetEyeBlink(int val)
        {
            _eyeBlink = (EyeBlink) val;
        }
    }
}
