using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace LiveAvatar.AvatarController.VRM
{
    public class BlendShapeController : MonoBehaviour
    {

        public float MouthOpen { get; set; }

        private VRMBlendShapeProxy m_blendShapePloxy;
        void Start () {
		
        }
	
        void Update () {
            if (m_blendShapePloxy)
            {
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
    }
}
